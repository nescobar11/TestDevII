export const isAuthenticated = () => !!localStorage.getItem('token');
export const saveAuth = data => { localStorage.setItem('token', data.token); localStorage.setItem('username', data.username); };
export const logout = () => { localStorage.removeItem('token'); localStorage.removeItem('username'); };
