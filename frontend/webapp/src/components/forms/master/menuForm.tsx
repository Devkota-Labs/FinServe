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
  const [editing, setEditing] = useState<any>(null);
  const [addMode, setAddMode] = useState<any>(null);
  const [modules, setModules] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [deleteTarget, setDeleteTarget] = useState<any>(null);

  // FETCH ALL MODULES + ACTIVITIES
  async function loadModules() {
    setLoading(true);
    const res = await api.getMenus();
    setModules(res.data || []);
    setLoading(false);
  }

  useEffect(() => {
    loadModules();
  }, []);

  if (loading) return <TableSkeleton rows={4} />;

  if (!modules.length) {
    return (
      <p className="p-6 bg-white rounded-xl shadow border text-gray-600">
        No modules found
      </p>
    );
  }

  return (
    <div className="p-6 w-full">
      <div className="bg-white rounded-xl shadow-lg border p-6">
        <h2 className="text-2xl font-semibold mb-6">Menu Management</h2>

        <Accordion type="single" collapsible className="w-full space-y-4">
          {modules.map((module) => (
            <AccordionItem
              key={module.id}
              value={String(module.id)}
              className="border rounded-lg px-4"
            >
              <AccordionTrigger className="py-4 text-lg font-medium">
                {module.name}
              </AccordionTrigger>

              <AccordionContent>
                <div className="space-y-4 pl-4">
                  {/* ACTIVITY LIST */}
                  {module.activities.map((act: any) => (
                    <ActivityRow
                      key={act.id}
                      activity={act}
                      editing={editing}
                      setEditing={setEditing}
                      refresh={loadModules}
                      onDelete={() => setDeleteTarget(act)}
                    />
                  ))}

                  {/* ADD NEW ACTIVITY ROW */}
                  {addMode === module.id ? (
                    <AddActivityRow
                      moduleId={module.id}
                      refresh={loadModules}
                      setAddMode={setAddMode}
                    />
                  ) : (
                    <Button
                      variant="outline"
                      className="mt-3 flex items-center gap-2"
                      onClick={() => setAddMode(module.id)}
                    >
                      <Plus size={16} /> Add Activity
                    </Button>
                  )}
                </div>
              </AccordionContent>
            </AccordionItem>
          ))}
        </Accordion>
      </div>

      {/* DELETE CONFIRMATION */}
      {deleteTarget && (
        <ConfirmDialog
          open={true}
          title="Delete Activity?"
          description={`Are you sure you want to delete "${deleteTarget.name}"?`}
          onCancel={() => setDeleteTarget(null)}
          onConfirm={async () => {
            //await api.deleteActivity(deleteTarget.id);
            setDeleteTarget(null);
            loadModules();
          }}
        />
      )}
    </div>
  );
}

/* ------------------------------------------------
   ACTIVITY ROW (VIEW + EDIT MODE)
------------------------------------------------ */
function ActivityRow({ activity, editing, setEditing, refresh, onDelete }) {
  const [form, setForm] = useState(activity);

  const isEditing = editing === activity.id;

  // keep form synced when activity updates
  useEffect(() => {
    setForm(activity);
  }, [activity]);

  async function save() {
    //await api.updateActivity(form.id, form);
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
            placeholder="Activity Name"
          />

          <Input
            value={form.route}
            onChange={(e) => setForm({ ...form, route: e.target.value })}
            placeholder="Route (/admin/...)"
          />

          <div className="flex items-center gap-3">
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
          {/* Name + route */}
          <div>
            <p className="font-medium">{activity.name}</p>
            <p className="text-sm text-gray-500">{activity.route}</p>
          </div>

          {/* Actions */}
          <div className="flex items-center gap-3">
            <Switch
              checked={activity.isActive}
              onCheckedChange={async () => {
                // await api.updateActivity(activity.id, {
                //   ...activity,
                //   isActive: !activity.isActive,
                // });
                refresh();
              }}
            />

            <Button
              size="sm"
              variant="outline"
              onClick={() => setEditing(activity.id)}
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

/* ------------------------------------------------
   ADD NEW ACTIVITY ROW
------------------------------------------------ */
function AddActivityRow({ moduleId, setAddMode, refresh }) {
  const [form, setForm] = useState({ name: "", route: "" });

  async function save() {
    //await api.createActivity(moduleId, form);
    setAddMode(null);
    refresh();
  }

  return (
    <div className="border p-4 rounded-lg bg-white shadow-sm space-y-3">
      <Input
        placeholder="Activity Name"
        value={form.name}
        onChange={(e) => setForm({ ...form, name: e.target.value })}
      />

      <Input
        placeholder="Route (/admin/... )"
        value={form.route}
        onChange={(e) => setForm({ ...form, route: e.target.value })}
      />

      <div className="flex gap-3">
        <Button variant="outline" onClick={() => setAddMode(null)}>
          <X size={16} /> Cancel
        </Button>

        <Button className="bg-blue-600 text-white" onClick={save}>
          <Save size={16} /> Save
        </Button>
      </div>
    </div>
  );
}
