import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Login from './pages/public/Login';
import ProtectedRoute from './routes/ProtectedRoute';
import AdminLayout from './layouts/AdminLayout';
import CustomerLayout from './layouts/CustomerLayout';
import AdminDashboard from './pages/admin/Dashboard';
import Users from './pages/admin/Users';
import CustomerDashboard from './pages/customer/Dashboard';

export default function App(){
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login/>} />
        <Route path="/admin/*" element={
          <ProtectedRoute roles={['Admin']}>
            <AdminLayout />
          </ProtectedRoute>
        }>
          <Route index element={<AdminDashboard/>} />
          <Route path="users" element={<Users/>} />
        </Route>

        <Route path="/customer/*" element={
          <ProtectedRoute roles={['Customer','Admin']}>
            <CustomerLayout />
          </ProtectedRoute>
        }>
          <Route index element={<CustomerDashboard/>} />
        </Route>

        <Route path="*" element={<Login/>} />
      </Routes>
    </BrowserRouter>
  );
}
