"use client";

import { useState } from "react";
import { api } from "@/lib/api";
import { validateField } from "@/lib/validators";
import { patterns } from "@/lib/patterns";
import { useRouter } from "next/navigation";

export function useRegistration(setErrorMsg: any, setSuccessMsg: any) {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  // ***** FORM SUBMIT FUNCTION *****
  async function registerUser(form: any) {
    setLoading(true);
    setErrorMsg("");
    setSuccessMsg("");
    try {
      const payload = {
        userName: form.userName.trim(),
        email: form.email.trim(),
        mobile: form.mobile ,
        gender: form.gender, 
        dateOfBirth: form.dateOfBirth,
        firstName: form.firstName ? form.firstName.trim() : null, 
        middleName: form.middleName.trim(),
        lastName: form.lastName.trim(),
        countryId: Number(form.countryId), 
        cityId: Number(form.cityId), 
        stateId: Number(form.stateId),    
        address: form.address.trim(),
        pinCode: form.pinCode.trim(),
        password: form.password,
      };
      let res=await api.register(payload);
      if(res.success===true)
      {
        if(res?.message)
        {
          setSuccessMsg(res.message);
        }
        return  true;
      }
    } 
    catch (err: any) {
      setErrorMsg(err?.message || "Something went wrong, Please Try again later.");
      return false;
    }
    finally {
      setLoading(false);
    }
  }
  return { registerUser, loading };
}
