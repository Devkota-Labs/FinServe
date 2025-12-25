"use client";

import { useState, useEffect } from "react";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import AppAlert from "@/components/common/AppAlert";
import SelectDialog from "@/components/common/SelectDialog";
import { api } from "@/lib/api";
import { Search } from "lucide-react";
import Link from "next/link";

type Role = {
    id: string;
    name: string;
};

export default function RoleAssignmentPage() {

    /* ===================== STATES ===================== */

    // User
    const [userModal, setUserModal] = useState(false);
    const [selectedUserId, setSelectedUserId] = useState("");
    const [selectedUserName, setSelectedUserName] = useState("");

    // Roles (MULTI)
    const [roleModal, setRoleModal] = useState(false);
    const [selectedRoles, setSelectedRoles] = useState<Role[]>([]);

    // Alerts
    const [errorMsg, setErrorMsg] = useState("");
    const [successMsg, setSuccessMsg] = useState("");

    /* ===================== HANDLERS ===================== */

    function handleRoleSelect(role: any) {
        const exists = selectedRoles.some(r => r.id === role.id);
        if (exists) return;

        setSelectedRoles(prev => [
            ...prev,
            { id: role.id, name: role.name }
        ]);
    }

    function removeRole(roleId: string) {
        setSelectedRoles(prev =>
            prev.filter(r => r.id !== roleId)
        );
    }

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        if (!selectedUserId || selectedRoles.length === 0) {
            setErrorMsg("Please select a user and at least one role");
            return;
        }
        setErrorMsg("");
        setSuccessMsg("");
        api.assingRoles(
            selectedUserId,
            selectedRoles.map(r => r.id),
        )
        .then(() => {
            setSuccessMsg("Roles assigned successfully!");
            setSelectedUserId("");
            setSelectedUserName("");
            setSelectedRoles([]);
        })
        .catch(err => {
                setErrorMsg(err?.message || "Failed to assign roles");
        });
     }
    /* ===================== AUTO CLEAR ALERT ===================== */
    useEffect(() => {
        if (errorMsg || successMsg) {
            const timer = setTimeout(() => {
                setErrorMsg("");
                setSuccessMsg("");
            }, 3000);
            return () => clearTimeout(timer);
        }
    }, [errorMsg, successMsg]);

    /* ===================== UI ===================== */

    return (
        <div>
            <Card className="w-full shadow-md border rounded-xl">
                <CardHeader>
                    <CardTitle className="text-2xl">
                        Role Assignment
                    </CardTitle>
                </CardHeader>

                <CardContent>

                    {errorMsg && <AppAlert type="error" message={errorMsg} />}
                    {successMsg && <AppAlert type="success" message={successMsg} />}

                    <div className="bg-gray-50 p-6 rounded-lg border">
                        <h2 className="text-xl font-semibold mb-4">
                            Assign Roles to User
                        </h2>

                        <form onSubmit={handleSubmit} className="space-y-5">

                            {/* ---------- SELECT USER ---------- */}
                            <div className="space-y-2">
                                <label className="text-sm font-medium">
                                    Select User
                                </label>

                                <div className="relative">
                                    <Input
                                        value={selectedUserName}
                                        readOnly
                                        placeholder="Select User"
                                        className="cursor-pointer pr-10"
                                        onClick={() => setUserModal(true)}
                                    />

                                    <Search
                                        className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-500 cursor-pointer"
                                        size={18}
                                        onClick={() => setUserModal(true)}
                                    />
                                </div>
                            </div>

                            {/* ---------- SELECT ROLES ---------- */}
                            <div className="space-y-2">
                                <label className="text-sm font-medium">
                                    Select Roles
                                </label>

                                <Button
                                    type="button"
                                    variant="outline"
                                    onClick={() => setRoleModal(true)}
                                >
                                    + Add Role
                                </Button>

                                {/* SELECTED ROLES CHIPS */}
                                {selectedRoles.length > 0 && (
                                    <div className="flex flex-wrap gap-2 mt-2">
                                        {selectedRoles.map(role => (
                                            <div
                                                key={role.id}
                                                className="flex items-center gap-2 bg-blue-100 text-blue-700 px-3 py-1 rounded-full text-sm"
                                            >
                                                {role.name}
                                                <button
                                                    type="button"
                                                    onClick={() => removeRole(role.id)}
                                                    className="text-blue-700 hover:text-red-600"
                                                >
                                                    ✕
                                                </button>
                                            </div>
                                        ))}
                                    </div>
                                )}
                            </div>

                            {/* ---------- ACTION BUTTONS ---------- */}
                            <div className="flex justify-end gap-3 pt-4">
                                <Link href="/admin/dashboard">
                                    <Button variant="outline">
                                        Cancel
                                    </Button>
                                </Link>

                                <Button
                                    type="submit"
                                    className="bg-blue-600 text-white"
                                >
                                    Assign Roles
                                </Button>
                            </div>
                        </form>
                    </div>
                </CardContent>
            </Card>

            {/* ===================== MODALS ===================== */}

            {/* USER SELECT MODAL */}
            <SelectDialog
                open={userModal}
                onClose={() => setUserModal(false)}
                title="Select User"
                fetchData={api.getAllUsers}
                searchKey="firstName"
                labelKey="firstName"
                valueKey="id"
                onSelect={(user) => {
                    setSelectedUserId(user.firstName);
                    setSelectedUserName(user.firstName);
                }}
            />

            {/* ROLE SELECT MODAL */}
            <SelectDialog
                open={roleModal}
                onClose={() => setRoleModal(false)}
                title="Select Role"
                fetchData={api.getRoles}
                searchKey="name"
                labelKey="name"
                valueKey="id"
                onSelect={(role) => {
                    handleRoleSelect(role);
                    setRoleModal(false);
                }}
            />
        </div>
    );
}
