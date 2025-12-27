import { useEffect, useState } from "react";
import {api} from "@/lib/api";

export function useRoles() {
  const [roles, setRoles] = useState([]);
  const [loading, setLoading] = useState(true);

  function load() {
    setLoading(true);
    api.GetAllState()
      .then((res) => setRoles(res.data || []))
      .finally(() => setLoading(false));
  }
  useEffect(load, []);
  return { roles, loading, reload: load };
}
