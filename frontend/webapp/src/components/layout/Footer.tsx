"use client";

import { useAuthStore } from "@/store/useAuthStore";

export default function Footer() {
  const user = useAuthStore((state) => state.user);

  return (
    <>
      {!user ? (
        /* Public Footer */
        <footer className="bg-gray-900 text-gray-300 mt-auto">
          <div className="px-4 md:px-20 py-10 grid grid-cols-1 md:grid-cols-3 gap-8">

            {/* About */}
            <div>
              <h4 className="text-white font-medium mb-3">About FinServe</h4>
              <p className="text-sm">
                FinServe is a secure financial management platform designed to
                simplify and safeguard financial operations.
              </p>
            </div>

            {/* Contact */}
            <div>
              <h4 className="text-white font-medium mb-3">Contact</h4>
              <p className="text-sm">Email: support@finserve.com</p>
              <p className="text-sm">Phone: +91 9XXXXXXXXX</p>
            </div>

            {/* Compliance */}
            <div>
              <h4 className="text-white font-medium mb-3">Compliance</h4>
              <ul className="text-sm space-y-1">
                <li>Privacy Policy</li>
                <li>Terms & Conditions</li>
                <li>Data Protection</li>
                <li>Regulatory Compliance</li>
              </ul>
            </div>
          </div>

          <div className="border-t border-gray-700 text-center py-4 text-xs">
            © {new Date().getFullYear()} FinServe. All rights reserved.
          </div>
        </footer>
      ) : (
        /* Authenticated Footer */
        <footer className="py-6 text-center text-sm text-gray-500 border-t bg-gray-50 mt-auto">
          © {new Date().getFullYear()} FinServe. All rights reserved.
        </footer>
      )}
    </>
  );
}
