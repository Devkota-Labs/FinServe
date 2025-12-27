"use client";

import { useState, useEffect, useCallback } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { User, Eye, EyeOff } from "lucide-react";

import AppAlert from "@/components/common/AppAlert";
import { useRegistration } from "@/hooks/useRegistration";
import { useFormMessages } from "@/hooks/useFormMessages";
import { validateField } from "@/lib/validators";
import { patterns } from "@/lib/patterns";
import { api } from "@/lib/api";

export default function RegisterForm() {
  const router = useRouter();

  const { errorMsg, setErrorMsg, successMsg, setSuccessMsg } =
    useFormMessages();

  const { registerUser, loading } = useRegistration(
    setErrorMsg,
    setSuccessMsg
  );

  const [showPassword, setShowPassword] = useState(false);
  const [errors, setErrors] = useState<Record<string, string>>({});

  /* ================= FORM ================= */
  const [form, setForm] = useState({
    userName: "",
    email: "",
    mobile: "",
    gender: "",
    dateOfBirth: "",
    firstName: "",
    middleName: "",
    lastName: "",
    password: "",
  });

  const [AddressForm, setAddressForm] = useState({
    addressType: "",
    countryId: "",
    stateId: "",
    cityId: "",
    addressLine1: "",
    addressLine2: "",
    pinCode: "",
    isPrimary: true,
  });

  /* ================= MASTER DATA ================= */
  const [gender, setGender] = useState<
    { code: string; displayName: string }[]
  >([]);
  const [countries, setCountries] = useState<{ id: string; name: string }[]>(
    []
  );
  const [states, setStates] = useState<{ id: string; name: string }[]>([]);
  const [cities, setCities] = useState<{ id: string; name: string }[]>([]);
  const [addressTypes, setAddressTypes] = useState<
    { code: string; displayName: string }[]
  >([]);

  /* ================= HELPERS ================= */
  const updateField = useCallback((field: string, value: string) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  }, []);

  const updateAddressField = useCallback((field: string, value: any) => {
    setAddressForm((prev) => ({ ...prev, [field]: value }));
  }, []);

  /* ================= LOAD MASTER DATA ================= */
  useEffect(() => {
    api.GetGender().then((res) => setGender(res?.data || []));
    api.GetCountry().then((res) => setCountries(res?.data || []));
    api.GetAddressType().then((res) => setAddressTypes(res?.data || []));
  }, []);

  useEffect(() => {
    if (!AddressForm.countryId) {
      setStates([]);
      updateAddressField("stateId", "");
      updateAddressField("cityId", "");
      return;
    }

    api.GetStateByContryID(AddressForm.countryId).then((res) => {
      setStates(res?.data || []);
    });
  }, [AddressForm.countryId, updateAddressField]);

  useEffect(() => {
    if (!AddressForm.stateId) {
      setCities([]);
      updateAddressField("cityId", "");
      return;
    }

    api.GetCityByStateID(AddressForm.stateId).then((res) => {
      setCities(res?.data || []);
    });
  }, [AddressForm.stateId, updateAddressField]);

  /* ================= SUBMIT ================= */
  async function handleSubmit(e: any) {
    e.preventDefault();
    setErrors({});
    setErrorMsg("");

    let newErrors: Record<string, string> = {};

    /* -------- FORM VALIDATIONS -------- */
    const formValidations = [
      { field: "email", label: "Email", pattern: patterns.email },
      { field: "mobile", label: "Phone", pattern: patterns.phone },
      { field: "gender", label: "Gender", pattern: patterns.id },
      { field: "dateOfBirth", label: "Date of Birth", pattern: patterns.dob },
      { field: "firstName", label: "First Name", pattern: patterns.name },
      { field: "lastName", label: "Last Name", pattern: patterns.name },
      { field: "userName", label: "User Name", pattern: patterns.address },
      { field: "password", label: "Password", pattern: patterns.password },
    ];

    formValidations.forEach(({ field, label, pattern }) => {
      const result = validateField(form[field as keyof typeof form], label, pattern);
      if (result) newErrors[field] = result;
    });

    /* -------- ADDRESS VALIDATIONS -------- */
    if (!AddressForm.addressType)
      newErrors.addressType = "Please select address type";

    const addressValidations = [
      { field: "countryId", label: "Country", pattern: patterns.id },
      { field: "stateId", label: "State", pattern: patterns.id },
      { field: "cityId", label: "City", pattern: patterns.id },
      { field: "pinCode", label: "Pin Code", pattern: patterns.pinCode },
    ];

    addressValidations.forEach(({ field, label, pattern }) => {
      const value = AddressForm[field as keyof typeof AddressForm];
      const result = validateField(
        String(value ?? ""),
        label,
        pattern
      );

      if (result) newErrors[field] = result;
    });

    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    const payload = {
      userName: form.userName.trim(),
      email: form.email.trim(),
      mobile: form.mobile.trim(),
      gender: form.gender,
      dateOfBirth: formatDateOnly(form.dateOfBirth),
      firstName: form.firstName.trim(),
      middleName: form.middleName?.trim() || "",
      lastName: form.lastName.trim(),
      addresses: [
        {
          addressType: AddressForm.addressType || "Unknown",
          countryId: Number(AddressForm.countryId),
          stateId: Number(AddressForm.stateId),
          cityId: Number(AddressForm.cityId),
          addressLine1: AddressForm.addressLine1.trim(),
          addressLine2: AddressForm.addressLine2 || "N/A",
          pinCode: AddressForm.pinCode.trim(),
          isPrimary: AddressForm.isPrimary,
        },
      ],
      password: form.password,
    };
    const res = await registerUser(payload);
    if (res) router.push("/login");
  }

  /* ================= UI ================= */
  return (
    <form className="space-y-10 px-2" onSubmit={handleSubmit}>
      {errorMsg && <AppAlert type="error" message={errorMsg} />}
      {successMsg && <AppAlert type="success" message={successMsg} />}

      {/* PERSONAL DETAILS */}
      {/* PERSONAL DETAILS */}
      <section>
        <h3 className="text-xl font-semibold mb-3">Personal Details</h3>

        {/* ROW 1 */}
        <div className="grid md:grid-cols-3 gap-4">
          <Input
            placeholder="First Name"
            value={form.firstName}
            onChange={(e) => updateField("firstName", e.target.value)}
          />

          <Input
            placeholder="Middle Name"
            value={form.middleName}
            onChange={(e) => updateField("middleName", e.target.value)}
          />

          <Input
            placeholder="Last Name"
            value={form.lastName}
            onChange={(e) => updateField("lastName", e.target.value)}
          />
        </div>

        {/* ROW 2 */}
        <div className="grid md:grid-cols-3 gap-4 mt-4">
          <Input
            placeholder="User Name"
            value={form.userName}
            onChange={(e) => updateField("userName", e.target.value)}
          />

          <Input
            type="date"
            value={form.dateOfBirth}
            onChange={(e) => updateField("dateOfBirth", e.target.value)}
          />

          <select
            className="w-full border rounded-md p-2"
            value={form.gender}
            onChange={(e) => updateField("gender", e.target.value)}
          >
            <option value="">Select Gender</option>
            {gender.map((g) => (
              <option key={g.code} value={g.code}>
                {g.displayName}
              </option>
            ))}
          </select>
        </div>
      </section>

      {/* CONTACT */}
      <section>
        <h3 className="text-xl font-semibold mb-3">Contact Details</h3>
        <Input
          placeholder="Email"
          value={form.email}
          onChange={(e) => updateField("email", e.target.value)}
        />
        <Input
          placeholder="Mobile"
          value={form.mobile}
          onChange={(e) => updateField("mobile", e.target.value)}
        />
      </section>

      {/* ADDRESS */}
      <section>
        <h3 className="text-xl font-semibold mb-3">Address Details</h3>

        <select
          value={AddressForm.addressType}
          onChange={(e) =>
            updateAddressField("addressType", e.target.value)
          }
        >
          <option value="">Select Address Type</option>
          {addressTypes.map((t) => (
            <option key={t.code} value={t.displayName}>
              {t.displayName}
            </option>
          ))}
        </select>

        {AddressForm.addressType && (
          <>
            <Input
              placeholder="Address Line 1"
              value={AddressForm.addressLine1}
              onChange={(e) =>
                updateAddressField("addressLine1", e.target.value)
              }
            />

            <div className="flex gap-4">
              <select
                value={AddressForm.countryId}
                onChange={(e) =>
                  updateAddressField("countryId", e.target.value)
                }
              >
                <option value="">Country</option>
                {countries.map((c) => (
                  <option key={c.id} value={c.id}>
                    {c.name}
                  </option>
                ))}
              </select>

              <select
                value={AddressForm.stateId}
                onChange={(e) =>
                  updateAddressField("stateId", e.target.value)
                }
              >
                <option value="">State</option>
                {states.map((s) => (
                  <option key={s.id} value={s.id}>
                    {s.name}
                  </option>
                ))}
              </select>

              <select
                value={AddressForm.cityId}
                onChange={(e) =>
                  updateAddressField("cityId", e.target.value)
                }
              >
                <option value="">City</option>
                {cities.map((c) => (
                  <option key={c.id} value={c.id}>
                    {c.name}
                  </option>
                ))}
              </select>
            </div>

            <Input
              placeholder="Pin Code"
              value={AddressForm.pinCode}
              onChange={(e) =>
                updateAddressField("pinCode", e.target.value)
              }
            />

            <label className="flex items-center gap-2">
              <input
                type="checkbox"
                checked={AddressForm.isPrimary}
                onChange={(e) =>
                  updateAddressField("isPrimary", e.target.checked)
                }
              />
              Primary Address
            </label>
          </>
        )}
      </section>

      {/* SECURITY */}
      <section>
        <h3 className="text-xl font-semibold mb-3">Security</h3>
        <div className="relative">
          <Input
            type={showPassword ? "text" : "password"}
            value={form.password}
            onChange={(e) => updateField("password", e.target.value)}
          />
          <button
            type="button"
            onClick={() => setShowPassword(!showPassword)}
            className="absolute right-3 top-3"
          >
            {showPassword ? <EyeOff /> : <Eye />}
          </button>
        </div>
      </section>

      <Button disabled={loading} className="w-full">
        {loading ? "Creating..." : "Create Account"}
      </Button>

      <div className="text-center text-sm">
        Already have an account?{" "}
        <Link href="/login" className="text-blue-600">
          Login
        </Link>
      </div>
    </form>
  );
}
function formatDateOnly(date: string) {
  if (!date) return "";
  return new Date(date).toISOString().split("T")[0];
}
