"use client";

import { useRouter } from "next/navigation";
import { PublicLayout } from '@packages/ui';

export default function PublicRootLayout({
  children,
}: {
  children: React.ReactNode;
}) 
{
  const router = useRouter();

  return (
    <PublicLayout onLoginClick={() => router.push("/login")}>
      {children}
    </PublicLayout>
  );
}
