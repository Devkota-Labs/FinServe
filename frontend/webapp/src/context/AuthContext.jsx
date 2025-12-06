import { createContext, useState } from 'react';
import jwtDecode from 'jwt-decode';
export const AuthContext = createContext();
export function AuthProvider({children}){
  const [token, setToken] = useState(localStorage.getItem('token'));
  const [user, setUser] = useState(token ? jwtDecode(token) : null);

  const login = (t) => { localStorage.setItem('token', t); setToken(t); setUser(jwtDecode(t)); };
  const logout = () => { localStorage.removeItem('token'); setToken(null); setUser(null); };

  return <AuthContext.Provider value={{token, user, login, logout}}>{children}</AuthContext.Provider>
}
