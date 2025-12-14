"use client";

import { useEffect, useState } from "react";
import { Card, CardHeader, CardTitle, CardContent, CardFooter } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Checkbox } from "@/components/ui/checkbox";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { api } from "@/lib/api";
import AppAlert from "@/components/common/AppAlert";
import { exportToExcel } from "@/lib/exportExcel";
import SearchBar from "@/components/common/SearchBar";
import Pagination from "@/components/common/Pagination";
import TableSkeleton from "@/components/common/TableSkeleton";

import {
  Search,
  Plus,
  ChevronLeft,
  ChevronRight,
} from "lucide-react";

export default function RolesPage() {
  // FORM STATES
  const [roleName, setRoleName] = useState("");
  const [description, setDescription] = useState("");
  const [isActive, setIsActive] = useState(true);
  const [isEditing, setIsEditing] = useState(false);
  const [editRoleId, setEditRoleId] = useState("");
  // LOADING
  const [loading, setLoading] = useState(true);
  // UI
  const [showForm, setShowForm] = useState(false);
  // MESSAGES
  const [successMsg, setSuccessMsg] = useState("");
  const [errorMsg, setErrorMsg] = useState("");
  // SEARCH
  const [search, setSearch] = useState("");
  // PAGINATION
  const [page, setPage] = useState(1);
  const PAGE_SIZE = 8;
  // ROLES LIST
  const [roles, setRoles] = useState([]);
  async function loadRoles() {
    setLoading(true);
    const res = await api.getRoles();
    setRoles(res?.data || []);
    setLoading(false);
  }

  useEffect(() => {
    loadRoles();
  }, []);

  // AUTO CLEAR MESSAGES
  useEffect(() => {
    if (successMsg || errorMsg) {
      const t = setTimeout(() => {
        setSuccessMsg("");
        setErrorMsg("");
      }, 2500);
      return () => clearTimeout(t);
    }
  }, [successMsg, errorMsg]);
  async function handleSubmit(e) {
    e.preventDefault();
    setSuccessMsg("");
    setErrorMsg("");

    try {
      if (isEditing) {
        // await api.updateRole(editRoleId, { name: roleName, description, isActive });
        setSuccessMsg("Role updated successfully!");
      } else {
        await api.addRoles({ name: roleName, description, isActive });
        setSuccessMsg("Role added successfully!");
      }

      loadRoles();
      resetForm();
      setShowForm(false);

    } catch (err) {
      setErrorMsg(err.message || "Something went wrong");
    }
  }

  function resetForm() {
    setRoleName("");
    setDescription("");
    setIsActive(true);
    setIsEditing(false);
    setEditRoleId("");
  }

  function handleEdit(role) {
    setRoleName(role.name);
    setDescription(role.description);
    setIsActive(role.isActive);
    setEditRoleId(role.id);

    setIsEditing(true);
    setShowForm(true);
  }
  function handleToggle(role) {
    // api.updateRole(role.id, { ...role, isActive: !role.isActive });
    loadRoles();
  }
  // FILTER
  const filteredRoles = roles.filter((r) =>
    r.name.toLowerCase().includes(search.toLowerCase()) ||
    r.description.toLowerCase().includes(search.toLowerCase())
  );

  const paginated = filteredRoles.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);
  const totalPages = Math.ceil(filteredRoles.length / PAGE_SIZE);
  return (
    <div>
      <Card className="w-full shadow-md border rounded-xl">
        <CardHeader className="flex justify-between items-center">
          <CardTitle className="text-2xl">Roles Management</CardTitle>
        </CardHeader>

        <CardContent>

          {errorMsg && <AppAlert type="error" message={errorMsg} />}
          {successMsg && <AppAlert type="success" message={successMsg} />}

          {/* TOP BAR */}
          {!showForm && !loading && (
            <div className="flex justify-between items-center mb-6">
              <SearchBar value={search} onChange={setSearch} placeholder="Search country..." />
              {/* ADD BUTTON */}
              <div className="flex gap-3">
                <Button  className="bg-black hover:bg-gray-800" onClick={() => exportToExcel(roles, "Roles Details")}>
                  Export Excel
                </Button>
                <Button onClick={() => {resetForm();setShowForm(true);}} className="bg-blue-600 flex items-center gap-2">
                <Plus size={18} /> Add Role
              </Button>
              </div>
            </div>
          )}

          {/* TOP BAR SKELETON */}
          {!showForm && loading && (
            <div className="flex justify-between items-center mb-6">
              <Skeleton className="h-10 w-64" />
              <Skeleton className="h-10 w-28" />
            </div>
          )}
          {showForm && !loading && (
            <div className="mb-8 bg-gray-50 p-6 rounded-lg border">
              <h2 className="text-xl font-semibold mb-4">
                {isEditing ? "Edit Role" : "Create Role"}
              </h2>

              <form onSubmit={handleSubmit} className="space-y-4">

                <div className="space-y-2">
                  <label className="text-sm font-medium">Role Name</label>
                  <Input
                    value={roleName}
                    onChange={(e) => setRoleName(e.target.value)}
                    required
                  />
                </div>

                <div className="space-y-2">
                  <label className="text-sm font-medium">Role Description</label>
                  <Textarea
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    required
                  />
                </div>

                <div className="flex items-center gap-2">
                  <Checkbox checked={isActive} onCheckedChange={(v) => setIsActive(!!v)} />
                  <span className="text-sm font-medium">Is Active</span>
                </div>

                <div className="flex justify-end gap-3 pt-2">
                  <Button
                    variant="outline"
                    onClick={() => {
                      resetForm();
                      setShowForm(false);
                    }}
                  >
                    Cancel
                  </Button>

                  <Button type="submit" className="bg-blue-600 text-white">
                    {isEditing ? "Update Role" : "Save Role"}
                  </Button>
                </div>

              </form>
            </div>
          )}
          {!showForm && (loading ? (<TableSkeleton rows={5} />):(
            <div className="rounded-xl border overflow-hidden">
              <table className="w-full">
                <thead className="bg-gray-100">
                  <tr>
                    <th className="p-3 text-left">Role</th>
                    <th className="p-3 text-left">Description</th>
                    <th className="p-3 text-left">Status</th>
                    <th className="p-3 text-right">Actions</th>
                  </tr>
                </thead>

                <tbody>
                  {paginated.map((role) => (
                    <tr key={role.id} className="border-t hover:bg-gray-50">
                      <td className="p-3">{role.name}</td>
                      <td className="p-3">{role.description}</td>
                      <td className="p-3">
                        {role.isActive ? (
                          <span className="text-green-700 font-medium">Active</span>
                        ) : (
                          <span className="text-red-700 font-medium">Inactive</span>
                        )}
                      </td>

                      <td className="p-3 text-right space-x-2">
                        <Button size="sm" variant="secondary" onClick={() => handleEdit(role)}>
                          Edit
                        </Button>

                        <Button
                          size="sm"
                          variant={role.isActive ? "destructive" : "default"}
                          onClick={() => handleToggle(role)}
                        >
                          {role.isActive ? "Deactivate" : "Activate"}
                        </Button>
                      </td>
                    </tr>
                  ))}
                </tbody>

              </table>
            </div>
          ))}
        </CardContent>
        {!showForm && (
            <Pagination page={page} totalPages={totalPages} onPrev={() => setPage(page - 1)} onNext={() => setPage(page + 1)}/>
        )}
      </Card>
    </div>
  );
}
