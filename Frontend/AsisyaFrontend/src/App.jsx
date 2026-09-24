import {Routes, Route, Navigate} from 'react-router-dom';
import Login from './pages/Login';
import Products from './pages/Products';
import ProductForm from './pages/ProductForm';
import AuthGuard from './components/AuthGuard';
export default function App(){
 return <Routes>
  <Route path="/login" element={<Login/>}/>
  <Route element={<AuthGuard/>}>
   <Route path="/" element={<Navigate to="/products" replace/>}/>
   <Route path="/products" element={<Products/>}/>
   <Route path="/products/new" element={<ProductForm/>}/>
   <Route path="/products/:id/edit" element={<ProductForm/>}/>
  </Route>
  <Route path="*" element={<Navigate to="/products" replace/>}/>
 </Routes>
}
