export const AppFooter = () => {
  return (
    /* Authenticated Footer */
    <footer className="py-6 text-center text-sm text-gray-500 border-t bg-gray-50 mt-auto">
      Secure Admin Portal • FinServe 
      © {new Date().getFullYear()} FinServe. All rights reserved.
    </footer>
  );
};
