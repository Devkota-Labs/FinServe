"use client";

import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import {
  Users,
  UserCheck,
  UserX,
  ShieldCheck,
} from "lucide-react";
import Link from "next/link";

export default function AdminDashboard() {
  return (
    <div className="space-y-8">

      {/* Header */}
      <div>
        <h1 className="text-3xl font-semibold">Admin Dashboard</h1>
        <p className="text-gray-600 mt-1">
          User management, approvals, and role administration overview.
        </p>
      </div>
      {/* Stats Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-6">
        {/* Total Users */}
        <StatCard
          title="Total Users"
          value="1,245"
          icon={<Users />}
          color="bg-blue-50 text-blue-600"
        />
        {/* Active Users */}
        <StatCard
          title="Active Users"
          value="1,108"
          icon={<UserCheck />}
          color="bg-green-50 text-green-600"
        />
        {/* Pending Approvals */}
        <StatCard
          title="Pending for Approvals"
          value="37"
          href="/admin/user-management/approve-user"
          icon={<UserX />}
          color="bg-yellow-50 text-yellow-600"
        />

        {/* Roles Assigned */}
        <StatCard
          title="Pending for Roles Assignment"
          value="5"
          icon={<ShieldCheck />}
          color="bg-purple-50 text-purple-600"
        />
      </div>
    </div>
  );
}

function StatCard({
  title,
  value,
  icon,
  color,
  href,
}: {
  title: string;
  value: string;
  icon: React.ReactNode;
  color: string;
  href?: string;
}) {
  const CardWrapper = href ? Link : "div";

  return (
    <CardWrapper href={href ?? ""} className="block">
      <div className="rounded-xl border bg-white shadow-sm hover:shadow-md transition cursor-pointer">
        <div className="flex items-center justify-between p-6">
          <div>
            <p className="text-sm text-gray-500 mb-1">{title}</p>
            <p className="text-3xl font-semibold">{value}</p>
          </div>

          <div className={`p-3 rounded-full ${color}`}>
            {icon}
          </div>
        </div>
      </div>
    </CardWrapper>
  );
}