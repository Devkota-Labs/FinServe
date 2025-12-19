"use client";
import { useState, useMemo, useEffect } from "react";
import { Card, CardHeader, CardTitle, CardContent, CardFooter } from "@/components/ui/card";
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

export default function CountriesPage() {
  const { countries, loading, reload } = useCountries();
  // UI
  const [showForm, setShowForm] = useState(false);
  const [search, setSearch] = useState("");
  // Pagination
  const [page, setPage] = useState(1);
  const PAGE_SIZE = 8;
  // Form Fields
  const [countryName, setCountryName] = useState("");
  const [isoCode, setIsoCode] = useState("");
  const [mobileCode, setMobileCode] = useState("");
  const [isActive, setIsActive] = useState(true);
  // Alerts
  const [errorMsg, setErrorMsg] = useState("");
  const [successMsg, setSuccessMsg] = useState("");
  /** Reset Form */
  function resetForm() {
    setCountryName("");
    setIsoCode("");
    setMobileCode("");
    setIsActive(true);
  }
  /** Submit */
  async function handleSubmit(e) {
    e.preventDefault();
    setErrorMsg("");
    setSuccessMsg("");
    try {
      const res = await api.addCountrys({
        name: countryName,
        isoCode,
        mobileCode,
        isActive,
      });
      if (res?.success == true) {
        setSuccessMsg("Country added successfully!");
        reload();
        resetForm();
        setShowForm(false);
      } else {
        setErrorMsg(res.message || "Failed to add country!");
      }
    } catch (err) {
      setErrorMsg(err.message || "Failed to add country!");
    }
  }
  /** Auto Hide Alerts */
  useEffect(() => {
    if (errorMsg || successMsg) {
      const t = setTimeout(() => {
        setErrorMsg("");
        setSuccessMsg("");
      }, 2500);
      return () => clearTimeout(t);
    }
  }, [errorMsg, successMsg]);
  /** Filter + paginate */
  const filtered = useMemo(
    () => countries.filter((c) =>
      c.name.toLowerCase().includes(search.toLowerCase())
    ),
    [countries, search]
  );
  const paginated = filtered.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);
  const totalPages = Math.ceil(filtered.length / PAGE_SIZE);
  return (
    <div>
      <Card className="w-full shadow-md border rounded-xl">
        <CardHeader className="flex justify-between items-center">
          <PageHeader title="Country Management"></PageHeader>
        </CardHeader>
        <CardContent>
          {errorMsg && <AppAlert type="error" message={errorMsg} />}
          {successMsg && <AppAlert type="success" message={successMsg} />}
          {/* TOP BAR */}
          {!showForm && !loading && (
            <div className="flex justify-between items-center mb-6">
              <SearchBar value={search} onChange={setSearch} placeholder="Search country..." />
              <div className="flex gap-3">
                <Button  className="bg-black hover:bg-gray-800" onClick={() => exportToExcel(countries, "Country Details")}>
                  Export Excel
                </Button>
                <Button className="bg-blue-600" onClick={() => setShowForm(true)}>+ Add Country </Button>
              </div>
            </div>
          )}
          {/* FORM */}
          {showForm && (
            <div className="mb-8 bg-gray-50 p-6 rounded-lg border">
              <h2 className="text-lg font-semibold mb-4">Add Country</h2>
              <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                  <label className="text-sm font-medium">Country Name</label>
                  <Input value={countryName} onChange={(e) => setCountryName(e.target.value)} required/>
                </div>
                <div>
                  <label className="text-sm font-medium">ISO Code</label>
                  <Input value={isoCode} onChange={(e) => setIsoCode(e.target.value)} required />
                </div>
                <div>
                  <label className="text-sm font-medium">Mobile Code</label>
                  <Input value={mobileCode} onChange={(e) => setMobileCode(e.target.value)}  required />
                </div>
                <div className="flex justify-end gap-3">
                  <Button type="button" variant="outline" onClick={() => {resetForm();setShowForm(false);}}>
                    Cancel
                  </Button>
                  <Button className="bg-blue-600 text-white" type="submit">Save Country</Button>
                </div>
              </form>
            </div>
          )}
          {/* TABLE / SKELETON */}
          {!showForm && (loading ? (<TableSkeleton rows={5} />) : (
            <div className="overflow-x-auto rounded-lg border shadow-sm bg-white">
              <table className="min-w-[800px] w-full">
                <thead className="bg-gray-100">
                  <tr>
                    <th className="px-4 py-3">Country</th>
                    <th className="px-4 py-3">Status</th>
                    <th className="px-4 py-3 text-right">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {paginated.length === 0 && (
                    <tr>
                      <td className="py-6 text-center text-gray-500 italic" colSpan={3}>
                        No countries found
                      </td>
                    </tr>
                  )}
                  {paginated.map((c) => (
                    <tr key={c.id} className="border-b hover:bg-gray-50">
                      <td className="px-4 py-3">{c.name}</td>
                      <td className="px-4 py-3">
                        {c.isActive ? (
                          <span className="px-3 py-1 text-xs rounded-full bg-green-100 text-green-700">
                            Active
                          </span>
                        ) : (
                          <span className="px-3 py-1 text-xs rounded-full bg-red-100 text-red-700">
                            Inactive
                          </span>
                        )}
                      </td>
                      <td className="px-4 py-3 text-right space-x-2">
                        <Button size="sm" className="bg-yellow-600 hover:bg-yellow-700">Edit</Button>
                        <Button size="sm" variant={c.isActive ? "destructive" : "default"} className={c.isActive ? "" : "bg-green-600 hover:bg-green-700"}>
                          {c.isActive ? "Deactivate" : "Activate"}
                        </Button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          ))}
        </CardContent>
        {/* PAGINATION */}
        {!showForm && (
          <Pagination page={page} totalPages={totalPages} onPrev={() => setPage(page - 1)} onNext={() => setPage(page + 1)}/>
         )}
      </Card>
    </div>
  );
}
