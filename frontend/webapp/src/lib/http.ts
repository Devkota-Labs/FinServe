import { getAccessToken, clearAccessToken } from "./auth"
import { refreshAccessToken } from "./refreshClient"
import { normalizeHeaders } from "./utils"
import { ApiError } from "./errors"
import { ApiResponse } from "./types"

export const API_BASE_URL ="https://tzrhqvey9d.execute-api.us-east-1.amazonaws.com/prod/api"
//export const API_BASE_URL="http://54.81.12.127:5005/api"
// --------------------------------
// RAW REQUEST
// --------------------------------
async function rawRequest(path: string, options: RequestInit = {}) {
  const token = getAccessToken()

  const headers: Record<string, string> = {
    "Content-Type": "application/json",
    ...normalizeHeaders(options.headers || {}),
  }
  if (token) {
    headers.Authorization = `Bearer ${token}`
  }

  return fetch(`${API_BASE_URL}${path}`, {
    ...options,
    headers,
    credentials: "include", // refresh token cookie
  })
}
// --------------------------------
// MAIN REQUEST
// --------------------------------
export async function request<T = any>(
  path: string,
  options: RequestInit = {}
): Promise<ApiResponse<T>> {
  try {
    let res = await rawRequest(path, options)

    if (res.status === 401) {
      try {
        await refreshAccessToken()
        res = await rawRequest(path, options)
      } catch {
        clearAccessToken()
        throw new ApiError("Session expired", 401)
      }
    }
    if (!res.ok) {
      const text = await res.text()
      throw new ApiError(`HTTP ${res.status}`, res.status, text)
    }
    const result: ApiResponse<T> = await res.json()

    if (!result.success) {
      throw new ApiError(
        result.message ?? "Something went wrong",
        res.status,
        result.data
      )
    }
    return result
  } catch (err: any) {
    console.error("API ERROR:", path, err)

    if (err instanceof ApiError) {
      throw err
    }

    throw new ApiError("Network / Server error")
  }
}
// --------------------------------
// SAFE REQUEST (NO THROW)
// --------------------------------
export async function requestSafe<T = any>(
  path: string,
  options: RequestInit = {}
): Promise<T | null> {
  try {
    const res = await request<T>(path, options)
    return res.data ?? null
  } catch {
    return null
  }
}
