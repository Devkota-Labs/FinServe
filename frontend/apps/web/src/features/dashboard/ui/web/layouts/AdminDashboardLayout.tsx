"use client";

// import AdminSidebar from "@/components/dashboards/AdminSidebar";
import { PropsWithChildren } from "react";


type AdminDashboardLayoutProps = PropsWithChildren<{
  // add explicit props here later
}>;

export default function AdminLayout({ children }: AdminDashboardLayoutProps) {
  return (
    <div>
      <main>
        {children}
      </main>
    </div>
  );
}
