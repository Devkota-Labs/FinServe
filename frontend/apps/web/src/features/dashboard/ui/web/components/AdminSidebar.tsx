// "use client";

// import { useAuthStore } from "@/store/useAuthStore";
// import { Button } from "../../../../../../../../packages/ui";
// import Link from "next/link";
// import { Avatar, AvatarFallback } from "../../../../../../../../packages/ui";
// import { usePathname } from "next/navigation";

// import {
//   ChevronDown,
//   ChevronRight,
//   LogOut,
//   Users,
//   UserCog,
//   Truck,
//   BarChart3
// } from "lucide-react";

// import { useState } from "react";

// // Map icons by dynamic string
// const ICONS: Record<string, any> = {
//   Users,
//   UserCog,
//   Truck,
//   BarChart3,
// };

// export default function AdminSidebar() {
//   const user = useAuthStore((state) => state.user);
//   const logout = useAuthStore((state) => state.logout);
//   const pathname = usePathname();

//   const menu = user?.menus || [];

//   const initials = user?.fullName
//     ?.split(" ")
//     .map((w: string) => w[0])
//     .join("")
//     .toUpperCase();

//   const [openMenu, setOpenMenu] = useState<string | null>(null);

//   const toggleMenu = (name: string) => {
//     setOpenMenu(openMenu === name ? null : name);
//   };

//   return (
//     <div className="p-6 flex flex-col h-full justify-between border-r bg-white">

//       {/* USER INFO */}
//       <div>
//         <div className="flex items-center gap-3 mb-8">
//           <Avatar className="h-14 w-14">
//             <AvatarFallback>{initials}</AvatarFallback>
//           </Avatar>

//           <div>
//             <h3 className="font-semibold">{user?.fullName}</h3>
//             <p className="text-sm text-gray-500">Administrator</p>
//           </div>
//         </div>

//         {/* DYNAMIC MENU GROUPS */}
//         <div className="space-y-6">
//           {menu.map((group: any, idx: number) => {
//             const Icon = ICONS[group.icon] || Users;
//             const isOpen = openMenu === group.name;

//             return (
//               <div key={idx}>
//                 {/* GROUP HEADER / ACCORDION BUTTON */}
//                 <button
//                   onClick={() => toggleMenu(group.name)}
//                   className="w-full flex justify-between items-center text-left py-2 text-gray-700 font-medium"
//                 >
//                   <span className="flex items-center gap-2">
//                     <Icon size={16} />
//                     {group.name}
//                   </span>
//                   {isOpen ? (
//                     <ChevronDown size={16} />
//                   ) : (
//                     <ChevronRight size={16} />
//                   )}
//                 </button>

//                 {/* SUBMENU */}
//                 {isOpen && (
//                   <div className="ml-6 mt-2 space-y-1">
//                     {group.children?.map((item: any, i: number) => {
//                       const href = item.route || "/admin/dashboard";
//                       const isActive = pathname === href;

//                       return (
//                         <Link key={i} href={href}>
//                           <Button
//                             variant="ghost"
//                             className={`w-full justify-start rounded-lg ${
//                               isActive
//                                 ? "bg-blue-100 text-blue-600 font-medium"
//                                 : "text-gray-700 hover:bg-blue-50 hover:text-blue-600"
//                             }`}
//                           >
//                             {item.name}
//                           </Button>
//                         </Link>
//                       );
//                     })}
//                   </div>
//                 )}
//               </div>
//             );
//           })}
//         </div>
//       </div>

//       {/* LOGOUT BUTTON */}
//       <div>
//         <Button
//           variant="ghost"
//           className="w-full justify-start text-red-600 hover:bg-red-50"
//           onClick={logout}
//         >
//           <LogOut size={16} className="mr-2" />
//           Logout
//         </Button>
//       </div>
//     </div>
//   );
// }


export default function AdminSideBar() {
  return (
    <div className="min-h-screen flex items-start justify-center px-4 py-12 bg-gradient-to-b from-gray-50 to-white">
      {}
    </div>
  );
}
