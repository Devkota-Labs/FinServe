// types.ts
export type ApiResponse<T = any> = {
  success: boolean
  message?: string
  data?: T
}
