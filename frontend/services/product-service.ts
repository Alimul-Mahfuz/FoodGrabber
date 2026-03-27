import { apiClient } from '@/lib/api-client';

export type Product = {
  id: string;
  name: string;
  description: string;
  quantity: number;
  basePrice: number;
  sellingPrice: number;
  image: string | null;
  tags: string[];
  isActive: boolean;
  userId: string;
  createdAt: string;
  updatedAt: string;
};

export type ProductResponse = {
  items: Product[];
  totalCount: number;
  page: number;
  pageSize: number;
};

export const productService = {
  async getProducts(page = 1, pageSize = 10): Promise<ProductResponse> {
    return apiClient.get<ProductResponse>(`/products?Page=${page}&PageSize=${pageSize}`);
  },
  
  async addProduct(productData: Partial<Product>): Promise<Product> {
    return apiClient.post<Product>('/products', productData);
  },

  async getProductById(id: string): Promise<Product> {
    return apiClient.get<Product>(`/products/${id}`);
  },

  async updateProduct(id: string, productData: Partial<Product>): Promise<Product> {
    return apiClient.put<Product>(`/products/${id}`, productData);
  },

  async toggleActive(id: string) {
    // Assuming there's a toggle or update endpoint
    return apiClient.patch(`/products/${id}/toggle-active`, {});
  }
};
