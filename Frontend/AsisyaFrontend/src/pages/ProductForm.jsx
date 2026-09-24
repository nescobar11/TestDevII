import {useEffect} from 'react';
import {useForm} from 'react-hook-form';
import {useNavigate,useParams} from 'react-router-dom';
import {api} from '../api';
export default function ProductForm(){
 const {id}=useParams(); const nav=useNavigate(); const {register,handleSubmit,reset,formState:{errors,isSubmitting}}=useForm({defaultValues:{categoryId:1,unitPrice:0,unitsInStock:0,unitsOnOrder:0,reorderLevel:0,discontinued:false}});
 useEffect(()=>{if(id)api.get(`/Product/${id}`).then(r=>reset(r.data))},[id]);
 const submit=async d=>{try{d.categoryId=Number(d.categoryId);d.unitPrice=Number(d.unitPrice);d.unitsInStock=Number(d.unitsInStock);d.unitsOnOrder=Number(d.unitsOnOrder);d.reorderLevel=Number(d.reorderLevel);if(id)await api.put(`/Product/${id}`,d);else await api.post('/Product',d);nav('/products')}catch(e){alert(e.response?.data?.message||'Error al guardar')}};
 return <main className="page narrow"><h1>{id?'Editar':'Nuevo'} producto</h1><form className="card form" onSubmit={handleSubmit(submit)}>
 <label>Nombre<input {...register('productName',{required:'Requerido',maxLength:200})}/></label>{errors.productName&&<small>{errors.productName.message}</small>}
 <label>Categoría ID<input type="number" {...register('categoryId',{required:true,min:1})}/></label>
 <label>Cantidad por unidad<input {...register('quantityPerUnit')}/></label>
 <label>Precio<input type="number" step="0.01" {...register('unitPrice',{min:0})}/></label>
 <label>Stock<input type="number" {...register('unitsInStock',{min:0})}/></label>
 <label>En pedido<input type="number" {...register('unitsOnOrder',{min:0})}/></label>
 <label>Nivel de reposición<input type="number" {...register('reorderLevel',{min:0})}/></label>
 <label className="check"><input type="checkbox" {...register('discontinued')}/> Descontinuado</label>
 <div><button disabled={isSubmitting}>Guardar</button> <button type="button" onClick={()=>nav('/products')}>Cancelar</button></div>
 </form></main>
}
