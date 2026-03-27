import { apiClient } from '@/lib/api-client';

export type MenuProduct = {
  id?: string;
  productId: string;
  quantity: number;
};

export type Menu = {
  id?: string;
  name: string;
  description: string;
  sellingPrice: number;
  isActive: boolean;
  products: MenuProduct[];
};

export const menuService = {
  async getMenus(page = 1, pageSize = 10): Promise<any> {
    return apiClient.get(`/menus?Page=${page}&PageSize=${pageSize}`);
  },

  async createMenu(menuData: Menu): Promise<Menu> {
    return apiClient.post<Menu>('/menus', menuData);
  },

  async getMenuById(id: string): Promise<Menu> {
    return apiClient.get<Menu>(`/menus/${id}`);
  },
  
  async updateMenu(id: string, menuData: Partial<Menu>): Promise<Menu> {
    return apiClient.put<Menu>(`/menus/${id}`, menuData);
  }
};
