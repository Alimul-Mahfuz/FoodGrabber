import { apiClient } from '@/lib/api-client';

export type Product = {
  id: string;
  name: string;
  description: string;
  currentStock: number;
  stockUnit: string;
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
  
  async addProduct(productData: {
    name: string;
    description: string;
    initialStock: number;
    stockUnit: string;
    basePrice: number;
    sellingPrice: number;
    image: File;
    tags?: string[];
    isActive?: boolean;
  }): Promise<Product> {
    const formData = new FormData();
    formData.append('name', productData.name);
    formData.append('description', productData.description);
    formData.append('initialStock', String(productData.initialStock));
    formData.append('stockUnit', productData.stockUnit);
    formData.append('basePrice', String(productData.basePrice));
    formData.append('sellingPrice', String(productData.sellingPrice));
    formData.append('image', productData.image);
    formData.append('isActive', String(productData.isActive ?? true));

    for (const tag of productData.tags ?? []) {
      formData.append('tags', tag);
    }

    return apiClient.post<Product>('/products', formData);
  },

  async getProductById(id: string): Promise<Product> {
    return apiClient.get<Product>(`/products/${id}`);
  },

  async updateProduct(id: string, formData: FormData): Promise<Product> {
    return apiClient.put<Product>(`/products/${id}`, formData);
  },

  async toggleActive(id: string) {
    // Assuming there's a toggle or update endpoint
    return apiClient.patch(`/products/${id}/toggle-active`, {});
  }
};
