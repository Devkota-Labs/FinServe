import { useEffect, useState } from "react";
import {api} from "@/lib/api";

export function useGetAllUsers() {
  const [users, setUsers] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  function load() {
    setLoading(true);
    api.getAllUsers()
      .then((res) => setUsers(res.data || []))
      .finally(() => setLoading(false));
  }
  useEffect(load, []);

  return { users, loading, reload: load };
}
