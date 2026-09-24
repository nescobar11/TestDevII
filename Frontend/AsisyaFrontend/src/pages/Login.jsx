import {useForm} from 'react-hook-form';
import {useNavigate} from 'react-router-dom';
import {api} from '../api';
import {saveAuth} from '../auth';
export default function Login(){
 const {register,handleSubmit,formState:{errors,isSubmitting}}=useForm({defaultValues:{username:'admin',password:'Admin123!'}});
 const nav=useNavigate();
 const onSubmit=async data=>{try{const r=await api.post('/Auth/login',data);saveAuth(r.data);nav('/products')}catch(e){alert(e.response?.data?.message||'No fue posible iniciar sesión')}};
 return <main className="center"><form className="card login" onSubmit={handleSubmit(onSubmit)}>
  <h1>Asisya</h1><p>Gestión de productos</p>
  <label>Usuario<input {...register('username',{required:'Requerido'})}/></label>{errors.username&&<small>{errors.username.message}</small>}
  <label>Contraseña<input type="password" {...register('password',{required:'Requerida'})}/></label>{errors.password&&<small>{errors.password.message}</small>}
  <button disabled={isSubmitting}>Ingresar</button><small>Demo: admin / Admin123!</small>
 </form></main>
}
