'use client';

// import { useState, useEffect } from 'react';
// import { usePathname } from "next/navigation";
// import { useAuthStore } from '@packages/app-core/src/features/auth/domain/auth.store';
// import Footer from '../layouts/Footer';
// import Header from '../layouts/Header';
import { Toaster } from "../../../../packages/ui/src/toast/Toaster";
// import AdminSidebar from '../features/dashboard/ui/web/components/AdminSidebar';
import { ErrorBoundary } from "../../../../packages/app-core/logging/ErrorBoundary";

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  // const [sidebarOpen, setSidebarOpen] = useState(false);
  // const user = useAuthStore((state) => state.user);
  // const pathname = usePathname();
  // const logout = useAuthStore((s) => s.logout);

  // useEffect(() => {
  //   const handler = () => logout();
  //   window.addEventListener('app:unauthorized', handler);
  //   return () => window.removeEventListener('app:unauthorized', handler);
  // }, [logout]);

  return (
    <html lang="en">
      <body className="min-h-screen flex flex-col">
        {/* <AuthProvider> */}
          {/* <Header sidebarOpen={sidebarOpen} setSidebarOpen={setSidebarOpen} /> */}

          {/* {user ? ( */}
            <div className="flex flex-col md:flex-row flex-1">
              {/* SIDEBAR */}
              {/* <div
                className={`
                  bg-white border-r shadow-lg transition-all duration-300
                  md:w-1/4
                  ${sidebarOpen ? "block" : "hidden md:block"}
                `}
              >
                <AdminSidebar />
              </div> */}

              {/* MAIN */}
              <main className="flex-1 bg-gray-50 p-6">
                {/* {children} */}
                <ErrorBoundary>{children}</ErrorBoundary>
              </main>
            </div>
          {/* ) : (
            <main className="flex-1">{children}</main>
          )} */}

          {/* <Footer /> */}
        {/* </AuthProvider> */}

        {/* ✅ THIS WAS MISSING */}
        <Toaster />
      </body>
    </html>
  );
}
