import React from "react";
import { AppHeader, AppFooter } from "@packages/ui";
// import { AppSidebar } from "./components/AppSidebar";

type Props = {
  children: React.ReactNode;
  userName: string;
  onLogout: () => void;
  navItems: { label: string; href: string }[];
  LinkComponent: React.ElementType;
};

export const ProtectedLayout = ({
  children,
  userName,
  onLogout,
  // navItems,
  // LinkComponent,
}: Props) => {
  return (
    <div className="flex min-h-screen">
      Protected Layout
      {/* <AppSidebar navItems={navItems} LinkComponent={LinkComponent} /> */}

      <div className="flex flex-col flex-1">
        <AppHeader userName={userName} onLogout={onLogout} />

        <main className="flex-1 p-6 bg-gray-100">{children}</main>

        <AppFooter />
      </div>
    </div>
  );
};
