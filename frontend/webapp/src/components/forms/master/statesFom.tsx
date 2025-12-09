"use client";
import { useState, useMemo, useEffect } from "react";
import { Card, CardHeader, CardTitle, CardContent, CardFooter } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
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

export default function StatesPage() {

  const { states, loading, reload } = useStates();
  // UI states
  const [showForm, setShowForm] = useState(false);
  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const PAGE_SIZE = 8;
  // Form fields
  const [stateName, setStateName] = useState("");
  const [stateCode, setStateCode] = useState("");
  const [countryId, setCountryId] = useState("");
  const [countryName, setCountryName] = useState("");

  // Alert
  const [successMsg, setSuccessMsg] = useState("");
  const [errorMsg, setErrorMsg] = useState("");

  // Country Selector
  const [countryModal, setCountryModal] = useState(false);

  /** Reset Form */
  function resetForm() {
    setStateName("");
    setStateCode("");
    setCountryId("");
    setCountryName("");
  }
  /** Handle Form Submit */
  async function handleSubmit(e) {
    e.preventDefault();
    setErrorMsg("");
    setSuccessMsg("");

    try {
      const res = await api.addStates({
        name: stateName,
        stateCode,
        countryId
      });
      if (res?.statusCode === 200 || res?.statusCode === 201) {
        setSuccessMsg("State added successfully!");
        reload();
        resetForm();
        setShowForm(false);
      } else {
        setErrorMsg(res.message || "Failed to add state.");
      }

    } catch (err) {
      setErrorMsg(err.message || "Failed to add state!");
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
  /** Filter + Pagination */
  const filtered = useMemo(
    () => states.filter(s =>
      s.name.toLowerCase().includes(search.toLowerCase())
    ),
    [states, search]
  );
  const paginated = filtered.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);
  const totalPages = Math.ceil(filtered.length / PAGE_SIZE);
  return (
    <div>
      <Card className="w-full shadow-md border rounded-xl">
        <CardHeader className="flex justify-between items-center">
          <PageHeader title="State Management"></PageHeader>
        </CardHeader>
        <CardContent>
          {errorMsg && <AppAlert type="error" message={errorMsg} />}
          {successMsg && <AppAlert type="success" message={successMsg} />}
          {/* TOP BAR */}
          {!showForm && !loading && (
            <div className="flex justify-between items-center mb-6">
              {/* Search Box with Icon */}
              <SearchBar value={search} onChange={setSearch} placeholder="Search state..." />
              <div className="flex gap-3">
                <Button
                  className="bg-black hover:bg-gray-800"
                  onClick={() => exportToExcel(states, "State Details")}
                >
                  Export Excel
                </Button>
                <Button className="bg-blue-600" onClick={() => setShowForm(true)}>
                  + Add State
                </Button>
              </div>
            </div>
          )}
          {/* FORM */}
          {showForm && (
            <div className="mb-8 bg-gray-50 p-6 rounded-lg border">
              <h2 className="text-lg font-semibold mb-4">Add State</h2>
              <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                  <label className="text-sm font-medium">State Name</label>
                  <Input
                    value={stateName}
                    onChange={(e) => setStateName(e.target.value)}
                    required
                  />
                </div>
                <div>
                  <label className="text-sm font-medium">State Code</label>
                  <Input
                    value={stateCode}
                    onChange={(e) => setStateCode(e.target.value)}
                    required
                  />
                </div>
                <div>
                  <label className="text-sm font-medium">Select Country</label>
                  <div className="relative">
                    <Input
                      value={countryName}
                      readOnly
                      placeholder="Select country"
                      className="cursor-pointer pr-10"
                      onClick={() => setCountryModal(true)}
                      required
                    />
                    <Search
                      size={18}
                      className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-500 cursor-pointer"
                      onClick={() => setCountryModal(true)}
                    />
                  </div>
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
                  <Button className="bg-blue-600 text-white" type="submit">
                    Save State
                  </Button>
                </div>
              </form>
            </div>
          )}
          {/* TABLE / SKELETON */}
          {!showForm && (loading ? (
            <TableSkeleton rows={3} />
          ) : (
            <div className="overflow-x-auto rounded-lg border shadow-sm bg-white">
              <table className="min-w-[800px] w-full">
                <thead className="bg-gray-100">
                  <tr>
                    <th className="px-4 py-3">State</th>
                    <th className="px-4 py-3">Status</th>
                    <th className="px-4 py-3 text-right">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {paginated.length === 0 && (
                    <tr>
                      <td className="py-6 text-center text-gray-500 italic" colSpan={3}>
                        No states found
                      </td>
                    </tr>
                  )}
                  {paginated.map((s) => (
                    <tr key={s.id} className="border-b hover:bg-gray-50">
                      <td className="px-4 py-3">{s.name}</td>

                      <td className="px-4 py-3">
                        {s.isActive ? (
                          <span className="px-3 py-1 text-xs rounded-full bg-green-100 text-green-700">Active</span>
                        ) : (
                          <span className="px-3 py-1 text-xs rounded-full bg-red-100 text-red-700">Inactive</span>
                        )}
                      </td>
                      <td className="px-4 py-3 text-right space-x-2">
                        <Button size="sm" className="bg-yellow-600 hover:bg-yellow-700">Edit</Button>
                        <Button
                          size="sm"
                          variant={s.isActive ? "destructive" : "default"}
                          className={s.isActive ? "" : "bg-green-600 hover:bg-green-700"}
                        >
                          {s.isActive ? "Deactivate" : "Activate"}
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
          <Pagination
            page={page}
            totalPages={totalPages}
            onPrev={() => setPage(page - 1)}
            onNext={() => setPage(page + 1)}
          />
        )}
      </Card>
      {/* COUNTRY MODAL */}
      <SelectDialog
        open={countryModal}
        onClose={() => setCountryModal(false)}
        title="Select Country"
        fetchData={api.GetCountry}
        searchKey="name"
        labelKey="name"
        valueKey="id"
        onSelect={(country) => {
          setCountryId(country.id);
          setCountryName(country.id);
        }}
      />
    </div>
  );
}