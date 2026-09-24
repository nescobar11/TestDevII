import {Navigate, Outlet} from 'react-router-dom';
import {isAuthenticated} from '../auth';
export default function AuthGuard(){ return isAuthenticated() ? <Outlet/> : <Navigate to="/login" replace/>; }
