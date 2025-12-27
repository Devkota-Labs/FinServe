"use client";
import { useState } from "react";
import { api } from "@/lib/api";
export function useRegistration(setErrorMsg: any, setSuccessMsg: any) {
  const [loading, setLoading] = useState(false);
  async function registerUser(payload: any) {
    setLoading(true);
    setErrorMsg("");
    setSuccessMsg("");
    try {
      const res = await api.register(payload);
      if (res?.success === true) {
        setSuccessMsg(res?.message || "Registration successful");
        return true;
      }
      setErrorMsg(res?.message || "Registration failed");
      return false;
    } catch (err: any) {
      setErrorMsg(
        err?.message || "Something went wrong, Please try again later."
      );
      return false;
    } finally {
      setLoading(false);
    }
  }
  return { registerUser, loading };
}
