import {useEffect,useState} from 'react';
import {Link,useNavigate} from 'react-router-dom';
import {api} from '../api';
import {logout} from '../auth';
export default function Products(){
 const [data,setData]=useState({items:[],totalCount:0,pageNumber:1,pageSize:10});
 const [q,setQ]=useState({search:'',categoryId:'',minPrice:'',maxPrice:''});
 const nav=useNavigate();
 const load=async(page=1)=>{const p={pageNumber:page,pageSize:10};Object.entries(q).forEach(([k,v])=>{if(v!=='')p[k]=v});const r=await api.get('/Product',{params:p});setData(r.data)};
 useEffect(()=>{load(1)},[]);
 const remove=async id=>{if(!confirm('¿Eliminar producto?'))return;await api.delete(`/Product/${id}`);load(data.pageNumber)};
 return <main className="page"><header><div><h1>Productos</h1><span>{data.totalCount} registros</span></div><nav><Link className="button" to="/products/new">Nuevo</Link><button onClick={()=>{logout();nav('/login')}}>Salir</button></nav></header>
 <section className="card filters"><input placeholder="Buscar..." value={q.search} onChange={e=>setQ({...q,search:e.target.value})}/><input placeholder="Categoría ID" value={q.categoryId} onChange={e=>setQ({...q,categoryId:e.target.value})}/><input type="number" placeholder="Precio mín." value={q.minPrice} onChange={e=>setQ({...q,minPrice:e.target.value})}/><input type="number" placeholder="Precio máx." value={q.maxPrice} onChange={e=>setQ({...q,maxPrice:e.target.value})}/><button onClick={()=>load(1)}>Filtrar</button></section>
 <section className="card tableWrap"><table><thead><tr><th>ID</th><th>Producto</th><th>Categoría</th><th>Precio</th><th>Stock</th><th></th></tr></thead><tbody>{data.items.map(p=><tr key={p.productId}><td>{p.productId}</td><td>{p.productName}</td><td>{p.categoryName}</td><td>${Number(p.unitPrice).toFixed(2)}</td><td>{p.unitsInStock}</td><td><Link to={`/products/${p.productId}/edit`}>Editar</Link> <button className="danger" onClick={()=>remove(p.productId)}>Eliminar</button></td></tr>)}</tbody></table></section>
 <div className="pagination"><button disabled={data.pageNumber<=1} onClick={()=>load(data.pageNumber-1)}>Anterior</button><span>Página {data.pageNumber} / {data.totalPages}</span><button disabled={data.pageNumber>=data.totalPages} onClick={()=>load(data.pageNumber+1)}>Siguiente</button></div>
 </main>
}
