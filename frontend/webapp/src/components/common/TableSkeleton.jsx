// components/common/TableSkeleton.jsx
import { Skeleton } from "@/components/ui/skeleton";

export default function TableSkeleton({ rows = 5 }) {
  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <Skeleton className="h-10 w-64" />
        <Skeleton className="h-10 w-28" />
      </div>

      <div className="border rounded-lg p-4 space-y-4 bg-white shadow-sm">
        {[...Array(rows)].map((_, i) => (
          <div key={i} className="grid grid-cols-3 gap-4 p-3 border-b">
            <Skeleton className="h-6 w-40" />
            <Skeleton className="h-6 w-20" />
            <Skeleton className="h-8 w-32 justify-self-end" />
          </div>
        ))}
      </div>
    </div>
  );
}
