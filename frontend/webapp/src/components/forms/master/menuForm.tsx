"use client";

import { useEffect, useState } from "react";
import {
  Accordion,
  AccordionItem,
  AccordionTrigger,
  AccordionContent,
} from "@/components/ui/accordion";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Switch } from "@/components/ui/switch";
import { Edit, Save, X, Plus, Trash } from "lucide-react";
import { api } from "@/lib/api";
import TableSkeleton from "@/components/common/TableSkeleton";
import ConfirmDialog from "@/components/common/ConfirmDialog";

export default function MenuManagement() {
  const [editing, setEditing] = useState<number | null>(null);
  const [menus, setMenus] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [deleteTarget, setDeleteTarget] = useState<any>(null);

  async function loadMenus() {
    setLoading(true);
    const res = await api.getMenus();
    const menuList =
    res?.data||[];
    setMenus(menuList);
    setLoading(false);
  }

  useEffect(() => {
    loadMenus();
  }, []);

  if (loading) return <TableSkeleton rows={6} />;

  if (!menus.length) {
    return (
      <p className="p-6 bg-white rounded-xl shadow border text-gray-600">
        No menus found
      </p>
    );
  }

  return (
    <div className="p-6 w-full">
      <div className="bg-white rounded-xl shadow-lg border p-6">
        <h2 className="text-2xl font-semibold mb-6">Menu Management</h2>

        <Accordion type="single" collapsible className="w-full space-y-4">
          {menus
            .sort((a, b) => a.order - b.order)
            .map((menu) => (
              <AccordionItem
                key={menu.menuId}
                value={String(menu.menuId)}
                className="border rounded-lg px-4"
              >
                <AccordionTrigger className="py-4 text-lg font-medium">
                  {menu.name}
                </AccordionTrigger>

                <AccordionContent>
                  <MenuRow
                    menu={menu}
                    editing={editing}
                    setEditing={setEditing}
                    refresh={loadMenus}
                    onDelete={() => setDeleteTarget(menu)}
                  />
                </AccordionContent>
              </AccordionItem>
            ))}
        </Accordion>
      </div>

      {deleteTarget && (
        <ConfirmDialog
          open
          title="Delete Menu?"
          description={`Are you sure you want to delete "${deleteTarget.name}"?`}
          onCancel={() => setDeleteTarget(null)}
          onConfirm={async () => {
            // await api.deleteMenu(deleteTarget.menuId);
            setDeleteTarget(null);
            loadMenus();
          }}
        />
      )}
    </div>
  );
}
function MenuRow({ menu, editing, setEditing, refresh, onDelete }) {
  const [form, setForm] = useState(menu);
  const isEditing = editing === menu.menuId;

  useEffect(() => {
    setForm(menu);
  }, [menu]);

  async function save() {
    // await api.updateMenu(form.menuId, form);
    setEditing(null);
    refresh();
  }

  return (
    <div className="border p-4 rounded-lg bg-gray-50">
      {isEditing ? (
        <div className="space-y-3">
          <Input
            value={form.name}
            onChange={(e) => setForm({ ...form, name: e.target.value })}
            placeholder="Menu Name"
          />

          <Input
            value={form.icon || ""}
            onChange={(e) => setForm({ ...form, icon: e.target.value })}
            placeholder="Route / Icon Path"
          />

          <div className="flex gap-3">
            <Button variant="outline" onClick={() => setEditing(null)}>
              <X size={16} /> Cancel
            </Button>
            <Button className="bg-blue-600 text-white" onClick={save}>
              <Save size={16} /> Save
            </Button>
          </div>
        </div>
      ) : (
        <div className="flex justify-between items-center">
          <div>
            <p className="font-medium">{menu.name}</p>
            <p className="text-sm text-gray-500">{menu.icon}</p>
          </div>

          <div className="flex items-center gap-3">
            <Button
              size="sm"
              variant="outline"
              onClick={() => setEditing(menu.menuId)}
            >
              <Edit size={16} />
            </Button>

            <Button size="sm" variant="destructive" onClick={onDelete}>
              <Trash size={16} />
            </Button>
          </div>
        </div>
      )}
    </div>
  );
}
