// import { ProtectedLayout } from "@packages/ui/layouts";
// import { RequireAuth } from "@packages/app-core/auth";

// export default function ProtectedRootLayout({
//   children,
// }: {
//   children: React.ReactNode;
// }) {
//   return (
//     <RequireAuth>
//       <ProtectedLayout>{children}</ProtectedLayout>
//     </RequireAuth>
//   );
// }


// "use client";

// import Link from "next/link";
// import { ProtectedLayout } from "@packages/ui/layouts";
// import { RequireAuth, useAuth } from "@packages/app-core/auth";

// const navItems = [
//   { label: "Dashboard", href: "/dashboard" },
//   { label: "Users", href: "/users" },
//   { label: "Settings", href: "/settings" },
// ];

// export default function ProtectedRootLayout({ children }) {
//   const { user, logout } = useAuth();

//   return (
//     <RequireAuth>
//       <ProtectedLayout
//         userName={user?.name ?? ""}
//         onLogout={logout}
//         navItems={navItems}
//         LinkComponent={Link}
//       >
//         {children}
//       </ProtectedLayout>
//     </RequireAuth>
//   );
// }
