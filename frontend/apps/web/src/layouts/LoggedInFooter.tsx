"use client";

export default function LoggedInFooter() {
  return (
    <>
      {/* Authenticated Footer */}
        <footer className="py-6 text-center text-sm text-gray-500 border-t bg-gray-50 mt-auto">
          © {new Date().getFullYear()} FinServe. All rights reserved.
        </footer>
    </>
  );
}
