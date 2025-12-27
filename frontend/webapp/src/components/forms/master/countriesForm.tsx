"use client";

import { useState, useMemo, useEffect } from "react";
import { Card, CardHeader, CardContent } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useCountries } from "@/hooks/master/useCountries";
import { exportToExcel } from "@/lib/exportExcel";
import AppAlert from "@/components/common/AppAlert";
import { api } from "@/lib/api";
import SearchBar from "@/components/common/SearchBar";
import Pagination from "@/components/common/Pagination";
import TableSkeleton from "@/components/common/TableSkeleton";
import PageHeader from "@/components/common/PageHeader";
import { toast } from "@/hooks/use-toast";
import ConfirmDialog from "@/components/ui/ConfirmDialog";

export default function CountriesPage() {
  const { countries, loading, reload } = useCountries();

  /* UI */
  const [showForm, setShowForm] = useState(false);
  const [search, setSearch] = useState("");

  /* Pagination */
  const [page, setPage] = useState(1);
  const PAGE_SIZE = 8;

  /* Form */
  const [countryName, setCountryName] = useState("");
  const [isoCode, setIsoCode] = useState("");
  const [mobileCode, setMobileCode] = useState("");
  const [isActive, setIsActive] = useState(true);

  /* Edit mode */
  const [editId, setEditId] = useState<number | null>(null);
  const [originalCountry, setOriginalCountry] = useState<any>(null);

  /* Alerts */
  const [errorMsg, setErrorMsg] = useState("");
  const [successMsg, setSuccessMsg] = useState("");

  /* Delete dialog */
  const [deleteDialog, setDeleteDialog] = useState<{ id: number; name: string } | null>(null);

  /* ---------- Helpers ---------- */

  function resetForm() {
    setCountryName("");
    setIsoCode("");
    setMobileCode("");
    setIsActive(true);
    setEditId(null);
    setOriginalCountry(null);
  }

  function handleEdit(country: any) {
    setCountryName(country.name);
    setIsoCode(country.isoCode);
    setMobileCode(country.mobileCode);
    setIsActive(country.isActive);

    setEditId(country.id);
    setOriginalCountry(country);
    setShowForm(true);
  }

  function getChangedFields() {
    if (!originalCountry) return {};
    const payload: any = {};
    if (countryName !== originalCountry.name)
      payload.name = countryName;
    if (isoCode !== originalCountry.isoCode)
      payload.isoCode = isoCode;
    if (mobileCode !== originalCountry.mobileCode)
      payload.mobileCode = mobileCode;
    if (isActive !== originalCountry.isActive)
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
        res = await api.updateCountrys(editId, payload);
      } else {
        res = await api.addCountrys({
          name: countryName,
          isoCode,
          mobileCode,
          isActive,
        });
      }
      if (res?.success) {
        setSuccessMsg(editId ? "Country updated successfully!" : "Country added successfully!");
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
      await api.deleteCountrys(deleteDialog.id);
      toast({ title: "Deleted", description: `${deleteDialog.name} deleted successfully` });
      reload();
    } catch (err: any) {
      toast({ title: "Error", description: err.message, variant: "destructive" });
    } finally {
      setDeleteDialog(null);
    }
  }
  /* ---------- Alerts Auto Hide ---------- */
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
    () => countries.filter(c => c.name.toLowerCase().includes(search.toLowerCase())),
    [countries, search]
  );

  const paginated = filtered.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);
  const totalPages = Math.ceil(filtered.length / PAGE_SIZE);

  /* ---------- UI ---------- */

  return (
    <Card className="shadow-md rounded-xl">
      <CardHeader className="flex justify-between items-center">
        <PageHeader title="Country Management" />
      </CardHeader>

      <CardContent>
        {errorMsg && <AppAlert type="error" message={errorMsg} />}
        {successMsg && <AppAlert type="success" message={successMsg} />}

        {!showForm && !loading && (
          <div className="flex justify-between mb-6">
            <SearchBar value={search} onChange={setSearch} placeholder="Search country..." />
            <div className="flex gap-3">
              <Button onClick={() => exportToExcel(countries, "Country Details")}>
                Export Excel
              </Button>
              <Button className="bg-blue-600" onClick={() => setShowForm(true)}>
                + Add Country
              </Button>
            </div>
          </div>
        )}

        {showForm && (
          <div className="bg-gray-50 p-6 rounded-lg border mb-6">
            <h2 className="font-semibold mb-4">
              {editId ? "Edit Country" : "Add Country"}
            </h2>

            <form onSubmit={handleSubmit} className="space-y-4">
              <Input placeholder="Country Name" value={countryName} onChange={e => setCountryName(e.target.value)} required />
              <Input placeholder="ISO Code" value={isoCode} onChange={e => setIsoCode(e.target.value)} required />
              <Input placeholder="Mobile Code" value={mobileCode} onChange={e => setMobileCode(e.target.value)} required />

              <div className="flex justify-end gap-3">
                <Button variant="outline" type="button" onClick={() => { resetForm(); setShowForm(false); }}>
                  Cancel
                </Button>
                <Button type="submit" className="bg-blue-600">
                  {editId ? "Update Country" : "Save Country"}
                </Button>
              </div>
            </form>
          </div>
        )}

        {!showForm && (
          loading ? <TableSkeleton rows={5} /> :
          <div className="border rounded-lg">
            <table className="w-full">
              <thead className="bg-gray-100">
                <tr>
                  <th className="p-3">Country</th>
                  <th className="p-3">Status</th>
                  <th className="p-3 text-right">Actions</th>
                </tr>
              </thead>
              <tbody>
                {paginated.map(c => (
                  <tr key={c.id} className="border-b">
                    <td className="p-3">{c.name}</td>
                    <td className="p-3">{c.isActive ? "Active" : "Inactive"}</td>
                    <td className="p-3 text-right space-x-2">
                      <Button size="sm" className="bg-yellow-600 hover:bg-yellow-700" onClick={() => handleEdit(c)}>Edit</Button>
                      <Button size="sm" variant="destructive" onClick={() => setDeleteDialog({ id: c.id, name: c.name })}>
                        Delete
                      </Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {!showForm && (
          <Pagination page={page} totalPages={totalPages} onPrev={() => setPage(p => p - 1)} onNext={() => setPage(p => p + 1)} />
        )}
      </CardContent>

      {deleteDialog && (
        <ConfirmDialog
          open
          title="Delete Country"
          description={`Delete "${deleteDialog.name}" permanently?`}
          confirmText="Delete"
          onConfirm={handleDeleteConfirm}
          onCancel={() => setDeleteDialog(null)}
        />
      )}
    </Card>
  );
}
