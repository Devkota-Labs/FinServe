"use client";

import { useState, useMemo, useEffect } from "react";
import { Card, CardHeader, CardTitle, CardContent, CardFooter } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Table, TableHead, TableHeader, TableRow, TableCell, TableBody } from "@/components/ui/table";
import { useGetAllUsers } from "@/hooks/user-management/useGetAllUsers";
import { exportToExcel } from "@/lib/exportExcel";
import { useToast } from "@/hooks/use-toast";
import SearchBar from "@/components/common/SearchBar";
import Pagination from "@/components/common/Pagination";
import TableSkeleton from "@/components/common/TableSkeleton";

import { FileSpreadsheet } from "lucide-react";

export default function AllUsersPage() {
  const { users, loading, reload } = useGetAllUsers();
  const { toast } = useToast();

  // Search + Pagination
  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const PAGE_SIZE = 5;

  const filtered = useMemo(() => {
    return users.filter(
      (u) =>
        u.fullName.toLowerCase().includes(search.toLowerCase()) ||
        u.email.toLowerCase().includes(search.toLowerCase())
    );
  }, [users, search]);

  const paginated = filtered.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);
  const totalPages = Math.ceil(filtered.length / PAGE_SIZE);

  // Dialog State
  const [dialog, setDialog] = useState<any>(null);

  function exportData() {
    exportToExcel(users, "All-Users");
    toast({ title: "Exported", description: "Excel downloaded" });
  }
  return (
    <div>
      <Card className="w-full shadow-md border rounded-xl">
        {/* PAGE HEADER */}
        <CardHeader className="flex justify-between items-center">
          <CardTitle className="text-2xl">All Users</CardTitle>
        </CardHeader>
        <CardContent>
          {/* SEARCH + EXPORT */}
          <div className="flex justify-between items-center mb-6">
            <SearchBar
              value={search}
              placeholder="Search by name or email..."
              onChange={(val) => {
                setSearch(val);
                setPage(1);
              }}
            />
            <Button
              onClick={exportData}
              className="bg-black hover:bg-gray-800 flex items-center gap-2"
            >
              <FileSpreadsheet size={18} />
              Export Excel
            </Button>
          </div>
          {/* TABLE SECTION */}
          {loading ? (
            <TableSkeleton rows={5} cols={4} />
          ) : (
            <div className="overflow-x-auto rounded-xl border shadow-sm bg-white">
              <Table className="min-w-[900px]">
                <TableHeader>
                  <TableRow className="bg-gray-100">
                    <TableHead className="font-semibold text-gray-700">Name</TableHead>
                    <TableHead className="font-semibold text-gray-700">Email</TableHead>
                    <TableHead className="font-semibold text-gray-700">Created</TableHead>
                    <TableHead className="font-semibold text-gray-700">Gender</TableHead>
                    <TableHead className="font-semibold text-gray-700">Mobile Number</TableHead>
                    <TableHead className="font-semibold text-gray-700">Country</TableHead>
                    <TableHead className="font-semibold text-gray-700">State</TableHead>
                    <TableHead className="font-semibold text-gray-700">City</TableHead>
                    <TableHead className="font-semibold text-gray-700">Address</TableHead>
                    <TableHead className="font-semibold text-gray-700">PinCode</TableHead>
                    <TableHead className="font-semibold text-gray-700">Is-Active</TableHead>
                    <TableHead className="font-semibold text-gray-700">Is-Approve</TableHead>
                    <TableHead className="font-semibold text-gray-700">Is-Mobile Verify</TableHead>
                    <TableHead className="font-semibold text-gray-700">Is-Email Verified</TableHead>
                    <TableHead className="font-semibold text-gray-700">Roles</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {paginated.length === 0 && (
                    <TableRow>
                      <TableCell colSpan={4} className="py-6 text-center text-gray-500 italic">
                        No users found
                      </TableCell>
                    </TableRow>
                  )}
                  {paginated.map((u) => (
                    <TableRow key={u.id} className="hover:bg-gray-50 transition">
                      <TableCell className="font-medium">{u.fullName}</TableCell>
                      <TableCell>{u.email}</TableCell>
                      <TableCell>{new Date(u.createdAt).toLocaleString()}</TableCell>
                      <TableCell>{u.gender}</TableCell>
                      <TableCell>{u.mobile}</TableCell>
                      <TableCell>{u.country}</TableCell>
                      <TableCell>{u.state}</TableCell>
                      <TableCell>{u.city}</TableCell>
                      <TableCell>{u.address}</TableCell>
                      <TableCell>{u.pinCode}</TableCell>
                      <TableCell>{u.isActive ? "Yes" : "No"}</TableCell>
                      <TableCell>{u.isApproved ? "Yes" : "No"}</TableCell>
                      <TableCell>{u.emailVerified ? "Yes" : "No"}</TableCell>
                      <TableCell>{u.mobileVerified ? "Yes" : "No"}</TableCell>
                      <TableCell>
                        {Array.isArray(u.userRoles) && u.userRoles.length > 0
                          ? u.userRoles.map((r) => r.name || r).join(", ")
                          : "--"}
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </div>
          )}
        </CardContent>
        {/* PAGINATION */}
        <Pagination
          page={page}
          totalPages={totalPages}
          onPrev={() => setPage(page - 1)}
          onNext={() => setPage(page + 1)}
        />
      </Card>
    </div>
  );
}
