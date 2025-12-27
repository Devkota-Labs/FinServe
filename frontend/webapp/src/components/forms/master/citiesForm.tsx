"use client";

import { useState, useMemo, useEffect } from "react";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Search, FileSpreadsheet, Plus } from "lucide-react";
import { useToast } from "@/hooks/use-toast";
import { useCities } from "@/hooks/master/useCities";
import { exportToExcel } from "@/lib/exportExcel";
import AppAlert from "@/components/common/AppAlert";
import TableSkeleton from "@/components/common/TableSkeleton";
import SearchBar from "@/components/common/SearchBar";
import Pagination from "@/components/common/Pagination";
import SelectDialog from "@/components/common/SelectDialog";
import ConfirmDialog from "@/components/ui/ConfirmDialog";
import { api } from "@/lib/api";

export default function CityPage() {
  const { toast } = useToast();
  const { cities, loading, reload } = useCities();

  /* UI */
  const [showForm, setShowForm] = useState(false);
  const [search, setSearch] = useState("");

  /* Pagination */
  const [page, setPage] = useState(1);
  const PAGE_SIZE = 8;

  /* Form fields */
  const [cityName, setCityName] = useState("");
  const [stateId, setStateId] = useState<number | null>(null);
  const [stateName, setStateName] = useState("");

  /* Edit mode */
  const [editId, setEditId] = useState<number | null>(null);
  const [originalCity, setOriginalCity] = useState<any>(null);

  /* Dialogs */
  const [stateModal, setStateModal] = useState(false);
  const [deleteDialog, setDeleteDialog] =
    useState<{ id: number; name: string } | null>(null);

  /* Alerts */
  const [errorMsg, setErrorMsg] = useState("");
  const [successMsg, setSuccessMsg] = useState("");

  /* ---------- Helpers ---------- */

  function resetForm() {
    setCityName("");
    setStateId(null);
    setStateName("");
    setEditId(null);
    setOriginalCity(null);
  }

  function handleEdit(city: any) {
    setCityName(city.name);
    setStateId(city.stateId);
    setStateName(city.stateName);

    setEditId(city.id);
    setOriginalCity(city);
    setShowForm(true);
  }

  function getChangedFields() {
    if (!originalCity) return {};

    const payload: any = {};

    if (cityName !== originalCity.name)
      payload.name = cityName;

    if (stateId !== originalCity.stateId)
      payload.stateId = stateId;

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

        res = await api.updateCity(editId, payload);
      } else {
        res = await api.addCity({
          name: cityName,
          stateId,
        });
      }

      if (res?.success) {
        setSuccessMsg(
          editId ? "City updated successfully!" : "City added successfully!"
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
      await api.deleteCity(deleteDialog.id);
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
    }, 3000);
    return () => clearTimeout(t);
  }, [errorMsg, successMsg]);

  /* ---------- Filter + Pagination ---------- */

  const filtered = useMemo(
    () =>
      cities.filter(c =>
        c.name.toLowerCase().includes(search.toLowerCase())
      ),
    [cities, search]
  );

  const paginated = filtered.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);
  const totalPages = Math.ceil(filtered.length / PAGE_SIZE);

  /* ---------- UI ---------- */

  return (
    <Card className="shadow-md rounded-xl">
      <CardHeader className="flex justify-between items-center">
        <CardTitle className="text-2xl">City Management</CardTitle>
      </CardHeader>

      <CardContent>
        {errorMsg && <AppAlert type="error" message={errorMsg} />}
        {successMsg && <AppAlert type="success" message={successMsg} />}

        {!showForm && (
          <div className="flex justify-between mb-6">
            <SearchBar
              value={search}
              onChange={setSearch}
              placeholder="Search city..."
            />

            <div className="flex gap-3">
              <Button
                className="bg-black hover:bg-gray-800 flex gap-2"
                onClick={() => exportToExcel(cities, "Cities")}
              >
                <FileSpreadsheet size={18} />
                Export Excel
              </Button>

              <Button
                className="bg-blue-600 flex gap-2"
                onClick={() => setShowForm(true)}
              >
                <Plus size={18} />
                Add City
              </Button>
            </div>
          </div>
        )}

        {showForm && (
          <div className="bg-gray-50 p-6 rounded-lg border mb-6">
            <h2 className="font-semibold mb-4">
              {editId ? "Edit City" : "Add City"}
            </h2>

            <form onSubmit={handleSubmit} className="space-y-4">
              <Input
                placeholder="City Name"
                value={cityName}
                onChange={e => setCityName(e.target.value)}
                required
              />

              <div className="relative">
                <Input
                  value={stateName}
                  readOnly
                  placeholder="Select State"
                  className="cursor-pointer pr-10"
                  onClick={() => setStateModal(true)}
                  required
                />
                <Search
                  size={18}
                  className="absolute right-3 top-1/2 -translate-y-1/2 cursor-pointer"
                  onClick={() => setStateModal(true)}
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
                  {editId ? "Update City" : "Save City"}
                </Button>
              </div>
            </form>
          </div>
        )}

        {!showForm &&
          (loading ? (
            <TableSkeleton rows={5} />
          ) : (
            <div className="border rounded-lg">
              <table className="w-full">
                <thead className="bg-gray-100">
                  <tr>
                    <th className="p-3 text-left">City</th>
                    <th className="p-3 text-left">Status</th>
                    <th className="p-3 text-right">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {paginated.map(c => (
                    <tr key={c.id} className="border-b">
                      <td className="p-3">{c.name}</td>
                      <td className="p-3">
                        {c.isActive ? "Active" : "Inactive"}
                      </td>
                      <td className="p-3 text-right space-x-2">
                        <Button size="sm" className="bg-yellow-600 hover:bg-yellow-700" onClick={() => handleEdit(c)}>
                          Edit
                        </Button>
                        <Button
                          size="sm"
                          variant="destructive"
                          onClick={() =>
                            setDeleteDialog({ id: c.id, name: c.name })
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

      {/* State selector */}
      <SelectDialog
        open={stateModal}
        onClose={() => setStateModal(false)}
        title="Select State"
        fetchData={api.GetAllState}
        searchKey="name"
        labelKey="name"
        valueKey="id"
        onSelect={state => {
          setStateId(state.id);
          setStateName(state.id);
          setStateModal(false);
        }}
      />
      {deleteDialog && (
        <ConfirmDialog
          open
          title="Delete City"
          description={`Delete "${deleteDialog.name}" permanently?`}
          confirmText="Delete"
          onConfirm={handleDeleteConfirm}
          onCancel={() => setDeleteDialog(null)}
        />
      )}
    </Card>
  );
}
