"use client";

import { useState, useMemo, useEffect } from "react";
import { Card, CardHeader, CardContent } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Search } from "lucide-react";
import { api } from "@/lib/api";
import AppAlert from "@/components/common/AppAlert";
import { exportToExcel } from "@/lib/exportExcel";
import { useStates } from "@/hooks/master/useStates";
import SearchBar from "@/components/common/SearchBar";
import Pagination from "@/components/common/Pagination";
import TableSkeleton from "@/components/common/TableSkeleton";
import PageHeader from "@/components/common/PageHeader";
import SelectDialog from "@/components/common/SelectDialog";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import { toast } from "@/hooks/use-toast";

export default function StatesPage() {
  const { states, loading, reload } = useStates();

  /* UI */
  const [showForm, setShowForm] = useState(false);
  const [search, setSearch] = useState("");

  /* Pagination */
  const [page, setPage] = useState(1);
  const PAGE_SIZE = 8;

  /* Form fields */
  const [stateName, setStateName] = useState("");
  const [stateCode, setStateCode] = useState("");
  const [countryId, setCountryId] = useState<number | null>(null);
  const [countryName, setCountryName] = useState("");

  /* Edit mode */
  const [editId, setEditId] = useState<number | null>(null);
  const [originalState, setOriginalState] = useState<any>(null);

  /* Alerts */
  const [successMsg, setSuccessMsg] = useState("");
  const [errorMsg, setErrorMsg] = useState("");

  /* Dialogs */
  const [countryModal, setCountryModal] = useState(false);
  const [deleteDialog, setDeleteDialog] =
    useState<{ id: number; name: string } | null>(null);

  /* ---------- Helpers ---------- */

  function resetForm() {
    setStateName("");
    setStateCode("");
    setCountryId(null);
    setCountryName("");
    setEditId(null);
    setOriginalState(null);
  }

  function handleEdit(state: any) {
    setStateName(state.name);
    setStateCode(state.stateCode);
    setCountryId(state.countryId);
    setCountryName(state.countryName);
    setEditId(state.id);
    setOriginalState(state);
    setShowForm(true);
  }

  function getChangedFields() {
    if (!originalState) return {};
    const payload: any = {};
    if (stateName !== originalState.name)
      payload.name = stateName;
    if (stateCode !== originalState.stateCode)
      payload.stateCode = stateCode;
    if (countryId !== originalState.countryId)
      payload.countryId = countryId;
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
        res = await api.updateStates(editId, payload);
      } else {
        res = await api.addStates({
          name: stateName,
          stateCode,
          countryId,
        });
      }
      if (res?.success) {
        setSuccessMsg(
          editId ? "State updated successfully!" : "State added successfully!"
        );
        reload();
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
      await api.deleteStates(deleteDialog.id);
      toast({
        title: "Deleted",
        description: `${deleteDialog.name} deleted successfully`,
      });
      reload();
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
    () => states.filter(s => s.name.toLowerCase().includes(search.toLowerCase())),
    [states, search]
  );
  const paginated = filtered.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);
  const totalPages = Math.ceil(filtered.length / PAGE_SIZE);
  /* ---------- UI ---------- */
  return (
    <Card className="shadow-md rounded-xl">
      <CardHeader className="flex justify-between items-center">
        <PageHeader title="State Management" />
      </CardHeader>

      <CardContent>
        {errorMsg && <AppAlert type="error" message={errorMsg} />}
        {successMsg && <AppAlert type="success" message={successMsg} />}

        {!showForm && !loading && (
          <div className="flex justify-between mb-6">
            <SearchBar
              value={search}
              onChange={setSearch}
              placeholder="Search state..."
            />
            <div className="flex gap-3">
              <Button onClick={() => exportToExcel(states, "State Details")}>
                Export Excel
              </Button>
              <Button className="bg-blue-600" onClick={() => setShowForm(true)}>
                + Add State
              </Button>
            </div>
          </div>
        )}

        {showForm && (
          <div className="bg-gray-50 p-6 rounded-lg border mb-6">
            <h2 className="font-semibold mb-4">
              {editId ? "Edit State" : "Add State"}
            </h2>

            <form onSubmit={handleSubmit} className="space-y-4">
              <Input
                placeholder="State Name"
                value={stateName}
                onChange={e => setStateName(e.target.value)}
                required
              />

              <Input
                placeholder="State Code"
                value={stateCode}
                onChange={e => setStateCode(e.target.value)}
                required
              />

              <div className="relative">
                <Input
                  value={countryName}
                  readOnly
                  placeholder="Select Country"
                  className="cursor-pointer pr-10"
                  onClick={() => setCountryModal(true)}
                  required
                />
                <Search
                  size={18}
                  className="absolute right-3 top-1/2 -translate-y-1/2 cursor-pointer"
                  onClick={() => setCountryModal(true)}
                />
              </div>

              <div className="flex justify-end gap-3">
                <Button
                  type="button"
                  variant="outline"
                  onClick={() => {
                    resetForm();
                    setShowForm(false);
                  }}
                >
                  Cancel
                </Button>
                <Button type="submit" className="bg-blue-600">
                  {editId ? "Update State" : "Save State"}
                </Button>
              </div>
            </form>
          </div>
        )}

        {!showForm &&
          (loading ? (
            <TableSkeleton rows={3} />
          ) : (
            <div className="border rounded-lg">
              <table className="w-full">
                <thead className="bg-gray-100">
                  <tr>
                    <th className="p-3">State</th>
                    <th className="p-3">Status</th>
                    <th className="p-3 text-right">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {paginated.map(s => (
                    <tr key={s.id} className="border-b">
                      <td className="p-3">{s.name}</td>
                      <td className="p-3">
                        {s.isActive ? "Active" : "Inactive"}
                      </td>
                      <td className="p-3 text-right space-x-2">
                        <Button size="sm" className="bg-yellow-600 hover:bg-yellow-700" onClick={() => handleEdit(s)}>
                          Edit
                        </Button>
                        <Button
                          size="sm"
                          variant="destructive"
                          onClick={() =>
                            setDeleteDialog({ id: s.id, name: s.name })
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

      {/* Country selector */}
      <SelectDialog
        open={countryModal}
        onClose={() => setCountryModal(false)}
        title="Select Country"
        fetchData={api.GetCountry}
        searchKey="name"
        labelKey="name"
        valueKey="id"
        onSelect={country => {
          setCountryId(country.id);
          setCountryName(country.id);
          setCountryModal(false);
        }}
      />

      {deleteDialog && (
        <ConfirmDialog
          open
          title="Delete State"
          description={`Delete "${deleteDialog.name}" permanently?`}
          confirmText="Delete"
          onConfirm={handleDeleteConfirm}
          onCancel={() => setDeleteDialog(null)}
        />
      )}
    </Card>
  );
}
