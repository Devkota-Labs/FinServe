// components/common/Pagination.jsx
import { Button } from "@/components/ui/button";

export default function Pagination({ page, totalPages, onPrev, onNext }) {
  return (
    <div className="flex justify-between items-center p-4">
      <Button disabled={page === 1} onClick={onPrev}>Previous</Button>

      <p className="text-gray-600 font-medium">
        Page {page} / {totalPages}
      </p>

      <Button disabled={page === totalPages} onClick={onNext}>Next</Button>
    </div>
  );
}
