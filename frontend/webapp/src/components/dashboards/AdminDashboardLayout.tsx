"use client";

import AdminSidebar from "@/components/dashboards/AdminSidebar";

export default function AdminLayout({ children }: any) {
  return (
    <div>
      <main>
        {children}
      </main>
    </div>
  );
}
