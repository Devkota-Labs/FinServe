'use client';

// import { useRouter } from 'next/navigation';
import { PublicLayout } from "../../components/layouts/PublicLayout";

export default function PublicRootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  // const router = useRouter();

  return (
    <div>
      <PublicLayout>
        {children}
      </PublicLayout>
    </div>
  );
}
