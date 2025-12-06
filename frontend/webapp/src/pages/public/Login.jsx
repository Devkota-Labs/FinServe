import { useState, useContext } from 'react';
import api from '../../services/api';
import { AuthContext } from '../../context/AuthContext';
export default function Login(){
  const { login } = useContext(AuthContext);
  const [email,setEmail]=useState('admin@finserve.local'); const [password,setPassword]=useState('password');
  const submit = async e => {
    e.preventDefault();
    const res = await api.post('/api/auth/login', { email, password });
    login(res.data.token);
    if(res.data.token){
      const roles = JSON.parse(atob(res.data.token.split('.')[1])).roles || [];
      if(roles.includes('Admin')) window.location.href = '/admin';
      else window.location.href = '/customer';
    }
  };
  return (<form onSubmit={submit} style={{maxWidth:400, margin:'2rem auto'}}><h2>Login</h2><input value={email} onChange={e=>setEmail(e.target.value)}/><input type='password' value={password} onChange={e=>setPassword(e.target.value)}/><button>Login</button></form>);
}
