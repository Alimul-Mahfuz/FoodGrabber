import { apiClient } from '@/lib/api-client';

export type LoginResponse = {
  token: string;
  expiresAtUtc: string;
  email: string;
  roles: string[];
};

export const authService = {
  async login(credentials: any): Promise<LoginResponse> {
    const data = await apiClient.post<LoginResponse>('/auth/login', credentials);
    if (data.token) {
      localStorage.setItem('token', data.token);
      localStorage.setItem('user', JSON.stringify({
        email: data.email,
        roles: data.roles
      }));
    }
    return data;
  },

  async register(userData: any) {
    return apiClient.post('/auth/register', userData);
  },

  isLoggedIn(): boolean {
    if (typeof window === 'undefined') return false;
    return !!localStorage.getItem('token');
  },

  getUser(): { email: string; roles: string[] } | null {
    if (typeof window === 'undefined') return null;
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  },

  isAdmin(): boolean {
    const user = this.getUser();
    return user?.roles.includes('Admin') || false;
  },

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    window.location.href = '/login';
  },
};
