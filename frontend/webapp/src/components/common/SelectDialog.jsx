"use client";
import { useEffect, useState } from "react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";

export default function SelectDialog({
  open,
  title = "Select Item",
  fetchData,       // API function
  searchKey = "name", // what property to search on
  labelKey = "name",  // what to show as label
  valueKey = "id",    // what to return
  onClose,
  onSelect
}) {
  const [items, setItems] = useState([]);
  const [search, setSearch] = useState("");
  useEffect(() => {
    if (open) {
      fetchData().then((res) => setItems(res.data || []));
    }
  }, [open]);
  
  if (!open) return null;

  const filtered = items.filter((item) =>
    item[searchKey]?.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className="fixed inset-0 bg-black bg-opacity-40 flex justify-center items-center">
      <div className="bg-white w-96 max-h-[400px] rounded-xl shadow-lg p-6 space-y-4 overflow-y-auto">
        <h2 className="text-lg font-semibold">{title}</h2>
        <Input
          placeholder={`Search ${title}...`}
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
        <div className="space-y-2 mt-2">
          {filtered.map((item) => (
            <div
              key={item[valueKey]}
              className="p-3 border rounded-lg hover:bg-gray-100 cursor-pointer flex justify-between"
              onClick={() => {
                onSelect(item);
                onClose();
              }}
            >
              <span className="font-medium">{item[labelKey]}</span>
              <span className="text-sm text-gray-500">{item[valueKey]}</span>
            </div>
          ))}
        </div>
        <div className="flex justify-end">
          <Button variant="outline" onClick={onClose}>Close</Button>
        </div>
      </div>
    </div>
  );
}
