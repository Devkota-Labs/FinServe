"use client";

import { useEffect, useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { Button } from "@/components/ui/button";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { api } from "@/lib/api";
import { CalendarDays, Mail, Phone, MapPin, User } from "lucide-react";

export default function ProfilePage() {
  const [user, setUser] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.getProfile().then((res) => {
      setUser(res.data);
      setLoading(false);
    });
  }, []);
  
  if (loading) return <ProfileSkeleton />;
  const initials =
    `${user.firstName?.[0] || ""}${user.lastName?.[0] || ""}`.toUpperCase();

  return (
    <div className="p-6 max-w-5xl mx-auto w-full">
      <Card className="shadow-md border rounded-xl">
        <CardHeader className="border-b py-6">
          <CardTitle className="text-2xl font-semibold">My Profile</CardTitle>
        </CardHeader>

        <CardContent className="p-6 space-y-8">
          {/* TOP SECTION */}
          <div className="flex flex-col sm:flex-row items-center gap-6">
            
            {/* Avatar */}
            <Avatar className="h-28 w-28">
              {user.profileImageUrl ? (
                <AvatarImage src={user.profileImageUrl} alt="Profile" />
              ) : (
                <AvatarFallback className="text-3xl bg-blue-600 text-white">
                  {initials}
                </AvatarFallback>
              )}
            </Avatar>

            {/* Name + Email */}
            <div>
              <h2 className="text-2xl font-semibold">
                {user.firstName} {user.middleName || ""} {user.lastName}
              </h2>

              {user.email && (
                <p className="flex items-center gap-2 text-gray-600 mt-1">
                  <Mail size={16} /> {user.email}
                </p>
              )}

              {user.mobile && (
                <p className="flex items-center gap-2 text-gray-600 mt-1">
                  <Phone size={16} /> {user.mobile}
                </p>
              )}
            </div>

            <div className="flex-1" />

            {/* EDIT BUTTON */}
            <Button className="bg-blue-600 text-white px-5">Edit Profile</Button>
          </div>

          {/* DETAILS GRID */}
          <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-5">
            <InfoItem label="Country" value={user.countryName} />
            <InfoItem label="State" value={user.stateName} />
            <InfoItem label="City" value={user.cityName} />
            <InfoItem label="Address" value={user.address} />
            <InfoItem label="Role" value={user.roleName || "N/A"} />
            <InfoItem
              label="Created At"
              value={new Date(user.createdAt).toLocaleString()}
            />
            <InfoItem
              label="Last Updated"
              value={new Date(user.updatedAt).toLocaleString()}
            />
          </div>
        </CardContent>
      </Card>
    </div>
  );
}

/* ----------- INFO ITEM COMPONENT ----------- */
function InfoItem({ label, value }: any) {
  return (
    <div className="p-4 bg-gray-50 rounded-lg border">
      <p className="text-xs text-gray-500 uppercase">{label}</p>
      <p className="mt-1 font-medium text-gray-800">{value || "—"}</p>
    </div>
  );
}

/* ----------- SKELETON ----------- */
function ProfileSkeleton() {
  return (
    <div className="p-6 max-w-5xl mx-auto w-full">
      <Card className="shadow-md border rounded-xl p-6">
        <div className="flex gap-6 items-center">
          <Skeleton className="h-28 w-28 rounded-full" />
          <div className="space-y-3">
            <Skeleton className="h-6 w-40" />
            <Skeleton className="h-5 w-60" />
            <Skeleton className="h-5 w-52" />
          </div>
        </div>

        <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-5 mt-8">
          {[...Array(6)].map((_, i) => (
            <Skeleton key={i} className="h-20 w-full rounded-lg" />
          ))}
        </div>
      </Card>
    </div>
  );
}
