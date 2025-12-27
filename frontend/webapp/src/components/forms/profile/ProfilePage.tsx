"use client";

import { useEffect, useState } from "react";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import SelectDialog from "@/components/common/SelectDialog";
import AppAlert from "@/components/common/AppAlert";
import { api } from "@/lib/api";
import { toast } from "@/hooks/use-toast";

/* ---------------- TYPES ---------------- */

type Address = {
  id: number;
  addressLine1: string;
  addressLine2: string;
  countryId: number;
  country: string;
  stateId: number;
  state: string;
  cityId: number;
  city: string;
  pinCode: string;
  isPrimary: boolean;
};

export default function ProfilePage() {
  const [user, setUser] = useState<any>(null);
  const [originalUser, setOriginalUser] = useState<any>(null);
  const [primaryAddress, setPrimaryAddress] = useState<Address | null>(null);

  const [loading, setLoading] = useState(true);
  const [editMode, setEditMode] = useState(false);

  /* PROFILE */
  const [firstName, setFirstName] = useState("");
  const [middleName, setMiddleName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [mobile, setMobile] = useState("");

  /* ADDRESS */
  const [addressLine1, setAddressLine1] = useState("");
  const [addressLine2, setAddressLine2] = useState("");
  const [pinCode, setPinCode] = useState("");

  const [countryId, setCountryId] = useState<number | null>(null);
  const [countryName, setCountryName] = useState("");
  const [stateId, setStateId] = useState<number | null>(null);
  const [stateName, setStateName] = useState("");
  const [cityId, setCityId] = useState<number | null>(null);
  const [cityName, setCityName] = useState("");

  /* VERIFICATION */
  const [isEmailVerified, setIsEmailVerified] = useState(true);
  const [isMobileVerified, setIsMobileVerified] = useState(true);
  const [verifyingEmail, setVerifyingEmail] = useState(false);
  const [verifyingMobile, setVerifyingMobile] = useState(false);
  const [emailVerificationPending, setEmailVerificationPending] = useState(false);
  const [mobileOtpPending, setMobileOtpPending] = useState(false);
  const [sendOtpVisible, setSendOtpVisible] = useState(false);

  /* OTP MODAL (NEW) */
  const [otpModalOpen, setOtpModalOpen] = useState(false);
  const [otp, setOtp] = useState("");
  const [verifyingOtp, setVerifyingOtp] = useState(false);
  const [resendTimer, setResendTimer] = useState(30);
  const [canResendOtp, setCanResendOtp] = useState(false);

  /* MODALS */
  const [countryModal, setCountryModal] = useState(false);
  const [stateModal, setStateModal] = useState(false);
  const [cityModal, setCityModal] = useState(false);

  const [errorMsg, setErrorMsg] = useState("");
  const [successMsg, setSuccessMsg] = useState("");

  /* ---------------- LOAD PROFILE ---------------- */

  useEffect(() => {
    loadProfile();
  }, []);

  useEffect(() => {
    if (!emailVerificationPending) return;

    const interval = setInterval(async () => {
      const res = await api.getProfile();
      if (res.data.isEmailVerified) {
        setIsEmailVerified(true);
        setEmailVerificationPending(false);
        toast({ title: "Email verified successfully 🎉" });
        clearInterval(interval);
      }
    }, 5000);

    return () => clearInterval(interval);
  }, [emailVerificationPending]);

  useEffect(() => {
    if (!mobileOtpPending) return;

    const interval = setInterval(async () => {
      const res = await api.getProfile();
      if (res.data.isMobileVerified) {
        setIsMobileVerified(true);
        setMobileOtpPending(false);
        setSendOtpVisible(false);
        toast({ title: "Mobile verified successfully 🎉" });
        clearInterval(interval);
      }
    }, 5000);

    return () => clearInterval(interval);
  }, [mobileOtpPending]);

  useEffect(() => {
    if (!otpModalOpen || canResendOtp) return;

    const timer = setInterval(() => {
      setResendTimer(prev => {
        if (prev <= 1) {
          clearInterval(timer);
          setCanResendOtp(true);
          return 0;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(timer);
  }, [otpModalOpen, canResendOtp]);

  async function loadProfile() {
    setLoading(true);
    const res = await api.getProfile();
    setUser(res.data);
    setOriginalUser(res.data);
    hydrateForm(res.data);
    setLoading(false);
  }

  function hydrateForm(u: any) {
    setFirstName(u.firstName || "");
    setMiddleName(u.middleName || "");
    setLastName(u.lastName || "");
    setEmail(u.email || "");
    setMobile(u.mobile || "");

    const addr = u.addresses?.find((a: Address) => a.isPrimary) || null;
    setPrimaryAddress(addr);

    if (addr) {
      setAddressLine1(addr.addressLine1);
      setAddressLine2(addr.addressLine2);
      setPinCode(addr.pinCode);
      setCountryId(addr.countryId);
      setCountryName(addr.country);
      setStateId(addr.stateId);
      setStateName(addr.state);
      setCityId(addr.cityId);
      setCityName(addr.city);
    }
  }

  async function verifyEmail() {
    try {
      setVerifyingEmail(true);
      setEmailVerificationPending(true);
      await api.updateEmail(user.id, email);
      toast({ title: "Verification email sent" });
    } finally {
      setVerifyingEmail(false);
    }
  }

  async function sendMobileOtp() {
    try {
      setVerifyingMobile(true);
      setMobileOtpPending(true);
      await api.updateMobile(user.id, mobile);

      toast({ title: "OTP sent to mobile number" });

      setOtp("");
      setOtpModalOpen(true);
      setResendTimer(30);
      setCanResendOtp(false);
    } finally {
      setVerifyingMobile(false);
    }
  }

  async function verifyOtp() {
    try {
      setVerifyingOtp(true);
      //await api.verifyMobileOtp({ userId: user.id, otp });
      setOtpModalOpen(false);
      setOtp("");
      toast({ title: "OTP verified successfully" });
    } finally {
      setVerifyingOtp(false);
    }
  }

  async function resendOtp() {
    setResendTimer(30);
    setCanResendOtp(false);
    await api.updateMobile(user.id, mobile);
    toast({ title: "OTP resent" });
  }

  if (loading) return <ProfileSkeleton />;

  const initials =
    `${user.firstName?.[0] || ""}${user.lastName?.[0] || ""}`.toUpperCase();

  return (
    <Card className="rounded-xl shadow-md">
      <CardHeader className="flex justify-between">
        <CardTitle className="text-2xl">My Profile</CardTitle>
        {!editMode && <Button onClick={() => setEditMode(true)}>Edit</Button>}
      </CardHeader>

      <CardContent className="space-y-6">
        {errorMsg && <AppAlert type="error" message={errorMsg} />}
        {successMsg && <AppAlert type="success" message={successMsg} />}

        {/* PROFILE */}
        <div className="flex gap-6">
          <Avatar className="h-28 w-28">
            {user.profileImageUrl ? (
              <AvatarImage src={user.profileImageUrl} />
            ) : (
              <AvatarFallback className="bg-blue-600 text-white text-3xl">
                {initials}
              </AvatarFallback>
            )}
          </Avatar>

          <div className="grid sm:grid-cols-3 gap-3 flex-1">
            <Input value={firstName} disabled={!editMode} onChange={e => setFirstName(e.target.value)} />
            <Input value={middleName} disabled={!editMode} onChange={e => setMiddleName(e.target.value)} />
            <Input value={lastName} disabled={!editMode} onChange={e => setLastName(e.target.value)} />
          </div>
        </div>

        {/* EMAIL */}
        <div className="space-y-1">
          <Input value={email} disabled={!editMode || verifyingEmail}
            onChange={e => {
              setEmail(e.target.value);
              setIsEmailVerified(false);
            }} />

          <div className="flex gap-2 items-center">
            {editMode && email !== originalUser.email && !isEmailVerified && (
              <Button size="sm" disabled={verifyingEmail || emailVerificationPending} onClick={verifyEmail}>
                {verifyingEmail ? "Sending..." : "Verify"}
              </Button>
            )}
            <span className="text-xs">
              {isEmailVerified ? "Email verified" : emailVerificationPending ? "Verification pending..." : "Email not verified"}
            </span>
          </div>
        </div>

        {/* MOBILE */}
        <div className="space-y-1">
          <Input value={mobile} disabled={!editMode || verifyingMobile} maxLength={10}
            onChange={e => {
              const value = e.target.value.replace(/\D/g, "");
              setMobile(value);
              setIsMobileVerified(false);
              setSendOtpVisible(value.length === 10);
            }} />

          <div className="flex gap-2 items-center">
            {editMode && mobile !== originalUser.mobile && sendOtpVisible && !isMobileVerified && (
              <Button size="sm" disabled={verifyingMobile} onClick={sendMobileOtp}>
                {verifyingMobile ? "Sending OTP..." : "Send OTP"}
              </Button>
            )}
            <span className="text-xs">
              {isMobileVerified ? "Mobile verified" : "Mobile not verified"}
            </span>
          </div>
        </div>

        {/* ADDRESS */}
        <div className="border p-4 rounded-lg space-y-3">
          <Input value={addressLine1} disabled={!editMode} onChange={e => setAddressLine1(e.target.value)} />
          <Input value={addressLine2} disabled={!editMode} onChange={e => setAddressLine2(e.target.value)} />

          <div className="grid sm:grid-cols-3 gap-3">
            <Input readOnly value={countryName} disabled={!editMode} onClick={() => editMode && setCountryModal(true)} />
            <Input readOnly value={stateName} disabled={!editMode || !countryId} onClick={() => editMode && countryId && setStateModal(true)} />
            <Input readOnly value={cityName} disabled={!editMode || !stateId} onClick={() => editMode && stateId && setCityModal(true)} />
          </div>

          <Input value={pinCode} disabled={!editMode} onChange={e => setPinCode(e.target.value)} />
        </div>
      </CardContent>

      {/* OTP MODAL */}
      {otpModalOpen && (
        <div className="fixed inset-0 bg-black/40 flex items-center justify-center z-50">
          <div className="bg-white rounded-lg shadow-lg w-full max-w-sm p-6 space-y-4">
            <h3 className="text-lg font-semibold text-center">Verify OTP</h3>

            <Input value={otp} maxLength={6} placeholder="Enter 6-digit OTP"
              onChange={e => setOtp(e.target.value.replace(/\D/g, ""))} />

            <div className="flex justify-between text-sm">
              {canResendOtp ? (
                <button className="text-blue-600" onClick={resendOtp}>
                  Resend OTP
                </button>
              ) : (
                <span className="text-muted-foreground">
                  Resend OTP in {resendTimer}s
                </span>
              )}
            </div>

            <div className="flex justify-end gap-3">
              <Button variant="outline" onClick={() => setOtpModalOpen(false)}>
                Cancel
              </Button>
              <Button disabled={otp.length !== 6 || verifyingOtp} onClick={verifyOtp}>
                {verifyingOtp ? "Verifying..." : "Verify OTP"}
              </Button>
            </div>
          </div>
        </div>
      )}

      {/* SELECT MODALS */}
      <SelectDialog
        open={countryModal}
        onClose={() => setCountryModal(false)}
        title="Select Country"
        fetchData={api.GetCountry}
        labelKey="name"
        valueKey="id"
        onSelect={c => {
          setCountryId(c.id);
          setCountryName(c.name);
          setStateId(null);
          setStateName("");
          setCityId(null);
          setCityName("");
          setCountryModal(false);
        }}
      />

      <SelectDialog
        open={stateModal}
        onClose={() => setStateModal(false)}
        title="Select State"
        fetchData={() => api.GetStateByContryID(countryId)}
        labelKey="name"
        valueKey="id"
        onSelect={s => {
          setStateId(s.id);
          setStateName(s.name);
          setCityId(null);
          setCityName("");
          setStateModal(false);
        }}
      />

      <SelectDialog
        open={cityModal}
        onClose={() => setCityModal(false)}
        title="Select City"
        fetchData={() => api.GetCityByStateID(stateId)}
        labelKey="name"
        valueKey="id"
        onSelect={c => {
          setCityId(c.id);
          setCityName(c.name);
          setCityModal(false);
        }}
      />
    </Card>
  );
}

function ProfileSkeleton() {
  return (
    <Card className="p-6 space-y-4">
      <Skeleton className="h-28 w-28 rounded-full" />
      <Skeleton className="h-6 w-1/3" />
    </Card>
  );
}
