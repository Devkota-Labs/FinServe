"use client";

import { ReactNode } from "react";
import { AuthProvider } from "@packages/app-core";

export const Providers = ({ children }: { children: ReactNode }) => {
return <AuthProvider>{children}</AuthProvider>;
};
