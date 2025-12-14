"use client";

import { useState, useMemo, useEffect } from "react";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useToast } from "@/hooks/use-toast";
import { useCities } from "@/hooks/master/useCities";
import { exportToExcel } from "@/lib/exportExcel";
import AppAlert from "@/components/common/AppAlert";
import TableSkeleton from "@/components/common/TableSkeleton";
import SearchBar from "@/components/common/SearchBar";
import Pagination from "@/components/common/Pagination";
import SelectDialog from "@/components/common/SelectDialog";
import { api } from "@/lib/api";
import { Search, FileSpreadsheet, Plus } from "lucide-react";

export default function CityPage() {
    const { toast } = useToast();
    const { cities, loading, reload } = useCities();

    const [search, setSearch] = useState("");
    const [page, setPage] = useState(1);
    const PAGE_SIZE = 8;

    const filtered = useMemo(
        () => cities.filter((c) => c.name.toLowerCase().includes(search.toLowerCase())),
        [cities, search]
    );

    const paginated = filtered.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE);
    const totalPages = Math.ceil(filtered.length / PAGE_SIZE);

    // FORM STATES
    const [showForm, setShowForm] = useState(false);
    const [name, setCityName] = useState("");
    const [stateId, setStateId] = useState("");
    const [selectedStateName, setSelectedStateName] = useState("");

    const [stateModal, setStateModal] = useState(false);

    // ALERTS
    const [errorMsg, setErrorMsg] = useState("");
    const [successMsg, setSuccessMsg] = useState("");

    function resetForm() {
        setCityName("");
        setStateId("");
        setSelectedStateName("");
    }

    async function handleSubmit(e) {
        e.preventDefault();

        setErrorMsg("");
        setSuccessMsg("");

        api
            .addCity({ name, stateId })
            .then(() => {
                setSuccessMsg("City added successfully!");
                reload();
                setShowForm(false);
                resetForm();
            })
            .catch((err) => {
                setErrorMsg(err.message || "Failed to add city");
            });
    }

    // Auto-clear alert
    useEffect(() => {
        if (errorMsg || successMsg) {
            const t = setTimeout(() => {
                setErrorMsg("");
                setSuccessMsg("");
            }, 3000);
            return () => clearTimeout(t);
        }
    }, [errorMsg, successMsg]);

    return (
        <div>
            <Card className="w-full shadow-md border rounded-xl">
                <CardHeader className="flex justify-between items-center">
                    <CardTitle className="text-2xl">City Management</CardTitle>
                </CardHeader>

                <CardContent>
                    {errorMsg && <AppAlert type="error" message={errorMsg} />}
                    {successMsg && <AppAlert type="success" message={successMsg} />}

                    {/* TOP BAR */}
                    {!showForm && (
                        <div className="flex justify-between items-center mb-6">
                            <SearchBar value={search} onChange={setSearch} placeholder="Search city..." />

                            <div className="flex gap-3">
                                <Button
                                    onClick={() => exportToExcel(cities, "Cities")}
                                    className="bg-black hover:bg-gray-800 flex items-center gap-2"
                                >
                                    <FileSpreadsheet size={18} />
                                    Export Excel
                                </Button>

                                <Button
                                    onClick={() => setShowForm(true)}
                                    className="bg-blue-600 text-white flex items-center gap-2"
                                >
                                    <Plus size={18} />
                                    Add City
                                </Button>
                            </div>
                        </div>
                    )}

                    {/* FORM SECTION */}
                    {showForm && (
                        <div className="mb-8 bg-gray-50 p-6 rounded-lg border">
                            <h2 className="text-xl font-semibold mb-4">Add City</h2>

                            <form onSubmit={handleSubmit} className="space-y-4">
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">City Name</label>
                                    <Input value={name} onChange={(e) => setCityName(e.target.value)} required />
                                </div>

                                {/* Select State */}
                                <div className="space-y-2">
                                    <label className="text-sm font-medium">Select State</label>

                                    <div className="relative">
                                        <Input
                                            value={selectedStateName}
                                            readOnly
                                            className="cursor-pointer pr-10"
                                            placeholder="Select State"
                                            onClick={() => setStateModal(true)}
                                        />

                                        <Search
                                            className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-500 cursor-pointer hover:text-gray-800"
                                            size={18}
                                            onClick={() => setStateModal(true)}
                                        />
                                    </div>
                                </div>

                                <div className="flex justify-end gap-3">
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
                                        Save City
                                    </Button>
                                </div>
                            </form>
                        </div>
                    )}

                    {/* TABLE */}
                    {!showForm &&
                        (loading ? (
                            <TableSkeleton rows={5} />
                        ) : (
                            <div className="overflow-x-auto rounded-xl border shadow-sm bg-white">
                                <table className="min-w-[800px] w-full">
                                    <thead className="bg-gray-100">
                                        <tr>
                                            <th className="px-4 py-3 text-left">City</th>
                                            <th className="px-4 py-3 text-left">Status</th>
                                            <th className="px-4 py-3 text-right">Actions</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        {paginated.length === 0 && (
                                            <tr>
                                                <td colSpan={3} className="py-6 text-center text-gray-500 italic">
                                                    No cities found
                                                </td>
                                            </tr>
                                        )}

                                        {paginated.map((c) => (
                                            <tr key={c.id} className="border-t hover:bg-gray-50 transition">
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
                                                    <Button size="sm" className="bg-yellow-600 hover:bg-yellow-700">
                                                        Edit
                                                    </Button>

                                                    <Button
                                                        size="sm"
                                                        variant={c.isActive ? "destructive" : "default"}
                                                        className={c.isActive ? "" : "bg-green-600 hover:bg-green-700"}
                                                    >
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
                    <Pagination
                        page={page}
                        totalPages={totalPages}
                        onPrev={() => setPage(page - 1)}
                        onNext={() => setPage(page + 1)}
                    />
                )}
            </Card>
            {/* STATE SELECT MODAL */}
            <SelectDialog
                open={stateModal}
                onClose={() => setStateModal(false)}
                title="Select State"
                fetchData={api.GetAllState}
                searchKey="name"
                labelKey="name"
                valueKey="id"
                onSelect={(state) => {
                    setStateId(state.id);
                    setSelectedStateName(state.id);
                }}
            />

        </div>
    );
}