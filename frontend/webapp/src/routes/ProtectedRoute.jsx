import { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
export default function ProtectedRoute({children, roles}){
  const { token, user } = useContext(AuthContext);
  if(!token) return <Navigate to="/login" />;
  if(roles && !roles.some(r => user?.roles?.includes(r))) return <Navigate to="/login" />;
  return children;
}
