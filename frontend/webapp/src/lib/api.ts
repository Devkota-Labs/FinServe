// lib/api.ts
import { request, requestSafe } from "./http";

export const api = {
  /* =======================
          AUTH
  ======================= */

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
    request("/v1/Auth/forgot-password", {
      method: "POST",
      body: JSON.stringify(data),
    }),

  register: (data: any) =>
    request("/v1/Auth/register", {
      method: "POST",
      body: JSON.stringify(data),
    }),

  emailVerification: (id: number | string) =>
    request(`/v1/Auth/send-verification-email/${id}`, {
      method: "POST",
    }),

  mobileVerification: (id: number | string) =>
    request(`/v1/Auth/send-otp/${id}`, {
      method: "POST",
    }),

  updateEmail: (userId: number | string, email: string) =>
    request(`/v1/Auth/update-email/${userId}`, {
      method: "PATCH",
      body: JSON.stringify({ newEmail: email }),
    }),

  updateMobile: (userId: number | string, mobile: string) =>
    request(`/v1/Auth/update-mobile/${userId}`, {
      method: "PATCH",
      body: JSON.stringify({ newMobile: mobile }),
    }),

  /* =======================
          USER
  ======================= */

  getRoles: () =>
    request("/v1/Roles", { method: "GET" }),

  getModules: () =>
    request("/v1/User/modules", { method: "GET" }),

  getMenu: () =>
    request("/v1/Getmenu", { method: "GET" }),

  getProfile: () =>
    request("/v1/Users/profile", { method: "GET" }),

  /* =======================
          ADMIN
  ======================= */

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
    request("/v1/Admin/pending-users", { method: "GET" }),

  getAllUsers: () =>
    request("/v1/Users", { method: "GET" }),

  assignRoles: (userId: number | string, roleIds: number[]) =>
    request(`/v1/Admin/assign/${userId}`, {
      method: "PATCH",
      body: JSON.stringify({
        roleIds: roleIds,
      }),
    }),
  updateRoles: (id: number | string, data: any) =>
    request(`/v1/Roles/${id}`, {
      method: "PATCH",
      body: JSON.stringify(data),
    }),

  approveUser: (userId: number | string) =>
    request(`/v1/Admin/approve/${userId}`, {
      method: "PATCH",
      body: JSON.stringify({}),
    }),

  unlockUser: (userId: number | string) =>
    request(`/v1/unlock/${userId}`, {
      method: "PATCH",
      body: JSON.stringify({}),
    }),
  /* =======================
          MASTERS
  ======================= */
  GetCountry: () =>
    request("/v1/Countries", { method: "GET" }),

  addCountrys: (data: any) =>
    request("/v1/Countries", {
      method: "POST",
      body: JSON.stringify(data),
    }),

  deleteCountrys: (id: number | string) =>
    request(`/v1/Countries/${id}`, {
      method: "DELETE",
    }),

  updateCountrys: (id: number | string, data: any) =>
    request(`/v1/Countries/${id}`, {
      method: "PATCH",
      body: JSON.stringify(data),
    }),

  GetStateByContryID: (id: number | string) =>
    request(`/v1/States/country/${id}`, { method: "GET" }),

  GetAllState: () =>
    request("/v1/States", { method: "GET" }),

  addStates: (data: any) =>
    request("/v1/States", {
      method: "POST",
      body: JSON.stringify(data),
    }),

  deleteStates: (id: number | string) =>
    request(`/v1/States/${id}`, { method: "DELETE" }),

  updateStates: (id: number | string, data: any) =>
    request(`/v1/States/${id}`, {
      method: "PATCH",
      body: JSON.stringify(data),
    }),

  GetCityByStateID: (id: number | string) =>
    request(`/v1/Cities/state/${id}`, { method: "GET" }),

  GetAllCity: () =>
    request("/v1/Cities", { method: "GET" }),

  addCity: (data: any) =>
    request("/v1/Cities", {
      method: "POST",
      body: JSON.stringify(data),
    }),

  deleteCity: (id: number | string) =>
    request(`/v1/Cities/${id}`, { method: "DELETE" }),

  updateCity: (id: number | string, data: any) =>
    request(`/v1/Cities/${id}`, {
      method: "PATCH",
      body: JSON.stringify(data),
    }),

  GetGender: () =>
    request("/v1/Lookups/genders", { method: "GET" }),

  addRoles: (data: any) =>
    request("/v1/Roles", {
      method: "POST",
      body: JSON.stringify(data),
    }),

  deleteRoles: (id: number | string) =>
    request(`/v1/Roles/${id}`, { method: "DELETE" }),

  getMenus: () =>
    request("/v1/Menus", { method: "GET" }),

  GetAddressType: () =>
    request("/v1/Lookups/addressTypes", { method: "GET" }),

  /* =======================
        SAFE CALL EXAMPLE
  ======================= */

  safeGetMenu: () =>
    requestSafe<any[]>("/v1/Getmenu", { method: "GET" }),
};
