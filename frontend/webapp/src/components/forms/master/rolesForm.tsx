"use client";

import { useEffect, useMemo, useState } from "react";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Checkbox } from "@/components/ui/checkbox";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { Plus } from "lucide-react";
import { api } from "@/lib/api";
import AppAlert from "@/components/common/AppAlert";
import { exportToExcel } from "@/lib/exportExcel";
import SearchBar from "@/components/common/SearchBar";
import Pagination from "@/components/common/Pagination";
import TableSkeleton from "@/components/common/TableSkeleton";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import { toast } from "@/hooks/use-toast";

export default function RolesPage() {
  /* ---------- Data ---------- */
  const [roles, setRoles] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  /* ---------- UI ---------- */
  const [showForm, setShowForm] = useState(false);
  const [search, setSearch] = useState("");

  /* ---------- Pagination ---------- */
  const [page, setPage] = useState(1);
  const PAGE_SIZE = 8;

  /* ---------- Form ---------- */
  const [roleName, setRoleName] = useState("");
  const [description, setDescription] = useState("");
  const [isActive, setIsActive] = useState(true);

  /* ---------- Edit mode ---------- */
  const [editId, setEditId] = useState<number | null>(null);
  const [originalRole, setOriginalRole] = useState<any>(null);

  /* ---------- Alerts ---------- */
  const [successMsg, setSuccessMsg] = useState("");
  const [errorMsg, setErrorMsg] = useState("");

  /* ---------- Delete ---------- */
  const [deleteDialog, setDeleteDialog] =
    useState<{ id: number; name: string } | null>(null);

  /* ---------- Load ---------- */

  async function loadRoles() {
    setLoading(true);
    const res = await api.getRoles();
    setRoles(res?.data || []);
    setLoading(false);
  }

  useEffect(() => {
    loadRoles();
  }, []);

  /* ---------- Helpers ---------- */

  function resetForm() {
    setRoleName("");
    setDescription("");
    setIsActive(true);
    setEditId(null);
    setOriginalRole(null);
  }

  function handleEdit(role: any) {
    setRoleName(role.name);
    setDescription(role.description);
    setIsActive(role.isActive);

    setEditId(role.id);
    setOriginalRole(role);
    setShowForm(true);
  }

  function getChangedFields() {
    if (!originalRole) return {};

    const payload: any = {};

    if (roleName !== originalRole.name)
      payload.name = roleName;

    if (description !== originalRole.description)
      payload.description = description;

    if (isActive !== originalRole.isActive)
      payload.isActive = isActive;

    return payload;
  }

  /* ---------- Submit ---------- */

  async function handleSubmit(e: any) {
    e.preventDefault();
    setErrorMsg("");
    setSuccessMsg("");

    try {
      let res;
      if (editId) {
        const payload = getChangedFields();
        if (Object.keys(payload).length === 0) {
          setErrorMsg("No changes detected");
          return;
        }
        res = await api.updateRoles(editId, payload);
      } else {
        res = await api.addRoles({
          name: roleName,
          description,
          isActive,
        });
      }

      if (res?.success) {
        setSuccessMsg(
          editId ? "Role updated successfully!" : "Role added successfully!"
        );
        loadRoles();
        resetForm();
        setShowForm(false);
      } else {
        setErrorMsg(res?.message || "Operation failed");
      }
    } catch (err: any) {
      setErrorMsg(err.message || "Operation failed");
    }
  }

  /* ---------- Delete ---------- */

  async function handleDeleteConfirm() {
    if (!deleteDialog) return;

    try {
      await api.deleteRoles(deleteDialog.id);
      toast({
        title: "Deleted",
        description: `${deleteDialog.name} deleted successfully`,
      });
      loadRoles();
    } catch (err: any) {
      toast({
        title: "Error",
        description: err.message,
        variant: "destructive",
      });
    } finally {
      setDeleteDialog(null);
    }
  }

  /* ---------- Auto-hide alerts ---------- */

  useEffect(() => {
    if (!errorMsg && !successMsg) return;
    const t = setTimeout(() => {
      setErrorMsg("");
      setSuccessMsg("");
    }, 2500);
    return () => clearTimeout(t);
  }, [errorMsg, successMsg]);

  /* ---------- Filter + Pagination ---------- */

  const filtered = useMemo(
    () =>
      roles.filter(
        r =>
          r.name.toLowerCase().includes(search.toLowerCase()) ||
          r.description.toLowerCase().includes(search.toLowerCase())
      ),
    [roles, search]
  );

  const paginated = filtered.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);
  const totalPages = Math.ceil(filtered.length / PAGE_SIZE);

  /* ---------- UI ---------- */

  return (
    <Card className="shadow-md rounded-xl">
      <CardHeader className="flex justify-between items-center">
        <CardTitle className="text-2xl">Roles Management</CardTitle>
      </CardHeader>

      <CardContent>
        {errorMsg && <AppAlert type="error" message={errorMsg} />}
        {successMsg && <AppAlert type="success" message={successMsg} />}

        {!showForm && !loading && (
          <div className="flex justify-between mb-6">
            <SearchBar
              value={search}
              onChange={setSearch}
              placeholder="Search role..."
            />

            <div className="flex gap-3">
              <Button
                className="bg-black hover:bg-gray-800"
                onClick={() => exportToExcel(roles, "Roles")}
              >
                Export Excel
              </Button>

              <Button
                className="bg-blue-600 flex gap-2"
                onClick={() => setShowForm(true)}
              >
                <Plus size={18} />
                Add Role
              </Button>
            </div>
          </div>
        )}

        {showForm && (
          <div className="bg-gray-50 p-6 rounded-lg border mb-6">
            <h2 className="font-semibold mb-4">
              {editId ? "Edit Role" : "Create Role"}
            </h2>

            <form onSubmit={handleSubmit} className="space-y-4">
              <Input
                placeholder="Role Name"
                value={roleName}
                onChange={e => setRoleName(e.target.value)}
                required
              />

              <Textarea
                placeholder="Role Description"
                value={description}
                onChange={e => setDescription(e.target.value)}
                required
              />

              <div className="flex items-center gap-2">
                <Checkbox
                  checked={isActive}
                  onCheckedChange={v => setIsActive(!!v)}
                />
                <span className="text-sm font-medium">Is Active</span>
              </div>

              <div className="flex justify-end gap-3">
                <Button
                  variant="outline"
                  type="button"
                  onClick={() => {
                    resetForm();
                    setShowForm(false);
                  }}
                >
                  Cancel
                </Button>

                <Button type="submit" className="bg-blue-600">
                  {editId ? "Update Role" : "Save Role"}
                </Button>
              </div>
            </form>
          </div>
        )}

        {!showForm &&
          (loading ? (
            <TableSkeleton rows={5} />
          ) : (
            <div className="border rounded-xl overflow-hidden">
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
                  {paginated.map(role => (
                    <tr key={role.id} className="border-t">
                      <td className="p-3">{role.name}</td>
                      <td className="p-3">{role.description}</td>
                      <td className="p-3">
                        {role.isActive ? "Active" : "Inactive"}
                      </td>
                      <td className="p-3 text-right space-x-2">
                        <Button size="sm"  className="bg-yellow-600 hover:bg-yellow-700" onClick={() => handleEdit(role)}>
                          Edit
                        </Button>
                        <Button
                          size="sm"
                          variant="destructive"
                          onClick={() =>
                            setDeleteDialog({ id: role.id, name: role.name })
                          }
                        >
                          Delete
                        </Button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          ))}

        {!showForm && (
          <Pagination
            page={page}
            totalPages={totalPages}
            onPrev={() => setPage(p => p - 1)}
            onNext={() => setPage(p => p + 1)}
          />
        )}
      </CardContent>

      {deleteDialog && (
        <ConfirmDialog
          open
          title="Delete Role"
          description={`Delete "${deleteDialog.name}" permanently?`}
          confirmText="Delete"
          onConfirm={handleDeleteConfirm}
          onCancel={() => setDeleteDialog(null)}
        />
      )}
    </Card>
  );
}
