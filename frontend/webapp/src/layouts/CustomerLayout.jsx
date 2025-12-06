import { Outlet, Link } from 'react-router-dom';
export default function CustomerLayout(){ return (<div><h1>Customer</h1><nav><Link to='/customer'>Dashboard</Link></nav><Outlet/></div>); }