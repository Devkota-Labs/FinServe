// lib/api.ts
import { getAccessToken, setAccessToken, clearAccessToken } from "./auth";
import { refreshAccessToken } from "./refreshClient";
import { normalizeHeaders } from "./utils";
export const API_BASE_URL = "http://54.81.12.127:5005/api"
//export const API_BASE_URL = "https://tzrhqvey9d.execute-api.us-east-1.amazonaws.com/prod/api";//"http://54.81.12.127:5005/api";////"http://54.81.12.127:5005/api";//;//"https://localhost:5005/api"; //"https://tzrhqvey9d.execute-api.us-east-1.amazonaws.com/prod/api"; //

// -----------------------------
// RAW REQUEST (no retry logic)
// -----------------------------
async function rawRequest(path: string, options: RequestInit = {}) {
  const token = getAccessToken();
  const baseHeaders: Record<string, string> = {
    ...normalizeHeaders(options.headers || {}),
  };
  baseHeaders["Content-Type"] = "application/json";
  if (token) {
    baseHeaders["Authorization"] = `Bearer ${token}`;
  }
  const res = await fetch(`${API_BASE_URL}${path}`, {
    ...options,
    headers: baseHeaders,
    credentials: "include", // send refresh cookie
  });

  return res;
}


// -----------------------------
// MAIN REQUEST WRAPPER
// -----------------------------

async function request(path: string, options: RequestInit = {}) {
  let errMsg = "";
  let res = await rawRequest(path, options);
  let result = await res.json();
  console.log(result);
  // let data=result.data;
  // if(res.success===true)
  //   {

  //   }
  if (result.success !== true) {
    if (path === "/v1/Auth/login") {
      if (!result.data.user.emailVerified || !result.data.user.mobileVerified) return result;
    }
    errMsg = result.message ?? "Something went wrong.";
    throw new Error(errMsg);
  }
  return result;
}


// -----------------------------
// EXPORT API METHODS
// -----------------------------
export const api = {
  /*
         ------------------------------Auth-----------------------------------------
  */
  login: (data: any) =>
    request("/v1/Auth/login", {
      method: "POST",
      body: JSON.stringify(data),
    }),

  me: () =>
    request("/User/profile", {
      method: "GET",
    }),

  logout: () =>
    request("/v1/Auth/logout", {
      method: "POST",
    }),

  refresh: () =>
    request("/v1/Auth/refresh", {
      method: "POST",
    }),

  forgotPassword: (data: any) =>
  request(`/v1/Auth/forgot-password`, {
    method: "POST",
    body: JSON.stringify(data),
  }),

  register: (data: any) =>
    request("/v1/Auth/register", {   //api/v1/Auth/register
      method: "POST",
      body: JSON.stringify(data),
    }),

  emailVerification: (Id:number|string) =>
    request(`/v1/Auth/send-verification-email/${Id}`, { ///api/v1/Auth/send-verification-email/{userId}
      method: "POST",
    }),
  mobileVerification: (Id:number|string) =>
    request(`/v1/Auth/send-otp/${Id}`, { 
      method: "POST",
    }),
  /*
         ------------------------------User-----------------------------------------
  */
  getRoles: () =>
    request("/v1/Roles", { method: "GET" }),

  getModules: () =>
    request("/v1/User/modules", { method: "GET" }),

  getMenu: () =>
    request("/v1/Getmenu", {
      method: "GET",
    }),
  /*
         ------------------------------Admin-----------------------------------------
  */
  getLastModuleOrder: () =>
    request("/Admin/GetModulorder", { method: "GET" }),

  addModule: (payload: any) =>
    request("/Admin/Addmodules", {
      method: "POST",
      body: JSON.stringify(payload),
    }),

  getLastActivityOrder: () =>
    request("/Admin/GetActivityOrder", { method: "GET" }),

  addActivity: (payload: any) =>
    request("/Admin/AddActivities", {
      method: "POST",
      body: JSON.stringify(payload),
    }),
  getPendingUsers: () =>
    request("/v1/Admin/pending-users", {   ///api/v1/Admin/pending-users
      method: "GET",
    }),
  getAllUsers: () =>
    request("/v1/Users", {
      method: "GET",
    }),
    assingRoles: (userId:number|string,roles:any) =>
    request(`/v1/Admin/assign${userId}`, {  ///api/v1/Admin/assign/{userId}
      method: "POST",
      body: JSON.stringify(roles),
    }),
  /*
       ------------------------------Masters-----------------------------------------
*/
  GetCountry: () =>
    request("/v1/Countries", {
      method: "GET",
    }),

  // Get states by countryId
  GetStateByContryID: (Id: string | number) =>
    request(`/v1/States/country/${Id}`, {   //GetCityByStateID /api/v1/States/country/{countryId}
      method: "GET",
    }),
  GetAllState: () =>
    request(`/v1/States`, {
      method: "GET",
    }),
  // Get cities by stateId
  GetCityByStateID: (Id: string | number) =>
    request(`/v1/Cities/state/${Id}`, {   ///api/v1/Cities/state/{stateId}
      method: "GET",
    }),
  GetAllCity: () =>
    request(`/v1/Cities`, {
      method: "GET",
    }),
  //Get Genders for registration
  GetGender: () =>
    request("/v1/Lookups/genders", {
      method: "GET",
    }),
  addCountrys: (data: any) =>
    request("/v1/Countries", {
      method: "POST",
      body: JSON.stringify(data),
    }),
  addStates: (data: any) =>
    request("/v1/States", {
      method: "POST",
      body: JSON.stringify(data),
    }),
  addCity: (data: any) =>
    request("/v1/Cities", {
      method: "POST",
      body: JSON.stringify(data),
    }),
  addRoles: (data: any) =>
    request("/v1/Roles", {
      method: "POST",
      body: JSON.stringify(data),
    }),
  getMenus: () =>
    request("/v1/Menus", {   ///api/v1/Menus
      method: "GET",
    }),
  getProfile: () =>
    request("/v1/User/profile", {
      method: "GET",
    }),
};

