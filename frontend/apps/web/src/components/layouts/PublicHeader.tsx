// import { Link } from 'lucide-react';
import Link from "next/link";
import { useRouter } from "next/navigation";
import { Menu, X } from "lucide-react";
import { Button } from '@packages/ui';

export const PublicHeader = () => {
  return (
    <nav className="w-full py-5 px-6 md:px-20 flex justify-between items-center bg-white/80 backdrop-blur border-b">
      <h1 className="text-2xl font-bold tracking-tight text-gray-900">
          <Link href="/">FinServe</Link>
        </h1>
      <div className="flex items-center gap-4">
        {/* LOGIN/LOGOUT BUTTONS */}
        <Link href="/login">
              <Button variant="outline">Login</Button>
            </Link>
            <Link href="/register">
              <Button>Register</Button>
            </Link>
      </div>
    </nav>
  );
};
