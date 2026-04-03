"use client";

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { productService, Product } from '@/services/product-service';

export default function EditProductPage() {
  const params = useParams();
  const router = useRouter();
  const id = params?.id as string;

  const [formData, setFormData] = useState<Partial<Product>>({
    name: '',
    description: '',
    stockUnit: 'piece',
    basePrice: 0,
    sellingPrice: 0,
    image: '',
    tags: [],
    isActive: true
  });

  const [tagsInput, setTagsInput] = useState('');
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!id) return;
    
    async function fetchData() {
      try {
        const product = await productService.getProductById(id);
        const { id: excludedId, userId, createdAt, updatedAt, ...rest } = product;
        setFormData(rest);
        setTagsInput(rest.tags?.join(', ') || '');
      } catch (err: any) {
        setError('Failed to fetch product for editing.');
      } finally {
        setLoading(false);
      }
    }
    fetchData();
  }, [id]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value, type } = e.target;
    setFormData(prev => ({
       ...prev,
       [name]: type === 'number' ? parseFloat(value) : value
    }));
  };

  const handleToggle = () => {
    setFormData(prev => ({ ...prev, isActive: !prev.isActive }));
  };

  const [imageFile, setImageFile] = useState<File | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitting(true);
    setError('');

    try {
      const finalTags = tagsInput.split(',').map(tag => tag.trim()).filter(t => t !== '');
      
      const updateFormData = new FormData();
      updateFormData.append('name', formData.name || '');
      updateFormData.append('description', formData.description || '');
      updateFormData.append('stockUnit', formData.stockUnit || 'piece');
      updateFormData.append('basePrice', String(formData.basePrice || 0));
      updateFormData.append('sellingPrice', String(formData.sellingPrice || 0));
      updateFormData.append('isActive', String(formData.isActive ?? true));

      for (const tag of finalTags) {
        updateFormData.append('tags', tag);
      }

      if (imageFile) {
        updateFormData.append('image', imageFile);
      }

      await productService.updateProduct(id, updateFormData);
      router.replace(`/admin/products/${id}`); 
    } catch (err: any) {
      setError(err.message || 'Failed to update product. Please try again.');
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) return <div style={{ padding: '80px', textAlign: 'center' }}>Loading form...</div>;

  return (
    <div className="max-w-4xl mx-auto animate-fade-up">
      <div className="mb-10">
        <button 
          onClick={() => router.back()} 
          className="group flex items-center gap-2 text-sm font-bold text-muted-foreground hover:text-primary transition-colors mb-4"
        >
          <span className="group-hover:-translate-x-1 transition-transform">←</span>
          Cancel and Return
        </button>
        <h1 className="text-4xl font-black tracking-tighter">Edit Product.</h1>
        <p className="text-muted-foreground mt-2">Modify the details of your menu item here.</p>
      </div>

      <div className="bg-white dark:bg-zinc-900 rounded-[2rem] border border-slate-200 dark:border-zinc-800 shadow-2xl shadow-black/5 overflow-hidden">
        {error && (
          <div className="m-8 p-4 bg-destructive/10 border border-destructive/20 text-destructive text-sm font-bold rounded-2xl flex items-center gap-3">
             <span className="flex-shrink-0 w-6 h-6 rounded-full bg-destructive/20 flex items-center justify-center">!</span>
             {error}
          </div>
        )}

        <form onSubmit={handleSubmit} className="p-8 md:p-12 space-y-10">
          <section className="space-y-6">
            <h3 className="text-sm font-black uppercase tracking-widest text-primary/40">General Information</h3>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
              <div className="space-y-2">
                <label className="text-xs font-black uppercase tracking-widest text-muted-foreground/60">Product Name</label>
                <input 
                  name="name"
                  value={formData.name}
                  onChange={handleChange}
                  required
                  className="w-full h-14 px-6 rounded-2xl border border-slate-200 dark:border-zinc-800 bg-slate-50/50 dark:bg-zinc-800/50 focus:bg-white dark:focus:bg-zinc-900 focus:ring-4 focus:ring-primary/5 focus:border-primary outline-none transition-all font-medium"
                />
              </div>
              <div className="space-y-2">
                <label className="text-xs font-black uppercase tracking-widest text-muted-foreground/60">Tags</label>
                <input 
                  value={tagsInput}
                  onChange={(e) => setTagsInput(e.target.value)}
                  placeholder="e.g. Pasta, Spicy, Chef's Choice"
                  className="w-full h-14 px-6 rounded-2xl border border-slate-200 dark:border-zinc-800 bg-slate-50/50 dark:bg-zinc-800/50 focus:bg-white dark:focus:bg-zinc-900 focus:ring-4 focus:ring-primary/5 focus:border-primary outline-none transition-all font-medium"
                />
              </div>
              <div className="md:col-span-2 space-y-2">
                <label className="text-xs font-black uppercase tracking-widest text-muted-foreground/60">Description</label>
                <textarea 
                   name="description"
                   value={formData.description}
                   onChange={handleChange}
                   rows={4}
                   className="w-full p-6 rounded-2xl border border-slate-200 dark:border-zinc-800 bg-slate-50/50 dark:bg-zinc-800/50 focus:bg-white dark:focus:bg-zinc-900 focus:ring-4 focus:ring-primary/5 focus:border-primary outline-none transition-all font-medium resize-none"
                />
              </div>
            </div>
          </section>

          <section className="space-y-6">
            <h3 className="text-sm font-black uppercase tracking-widest text-primary/40">Inventory & Pricing</h3>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
              <div className="space-y-2">
                <label className="text-xs font-black uppercase tracking-widest text-muted-foreground/60">Stock Unit</label>
                <select
                  name="stockUnit"
                  value={formData.stockUnit}
                  onChange={handleChange}
                  className="w-full h-14 px-6 rounded-2xl border border-slate-200 dark:border-zinc-800 bg-slate-50/50 dark:bg-zinc-800/50 focus:bg-white dark:focus:bg-zinc-900 focus:ring-4 focus:ring-primary/5 focus:border-primary outline-none transition-all font-bold appearance-none cursor-pointer"
                >
                  <option value="piece">Piece</option>
                  <option value="kg">Kg</option>
                  <option value="gram">Gram</option>
                  <option value="liter">Liter</option>
                  <option value="ml">ML</option>
                  <option value="pack">Pack</option>
                </select>
              </div>
              <div className="space-y-2">
                <label className="text-xs font-black uppercase tracking-widest text-muted-foreground/60">Base Price</label>
                <div className="relative">
                  <span className="absolute left-6 top-1/2 -translate-y-1/2 font-bold text-muted-foreground">৳</span>
                  <input 
                    type="number"
                    step="0.01"
                    name="basePrice"
                    value={formData.basePrice}
                    onChange={handleChange}
                    className="w-full h-14 pl-12 pr-6 rounded-2xl border border-slate-200 dark:border-zinc-800 bg-slate-50/50 dark:bg-zinc-800/50 focus:bg-white dark:focus:bg-zinc-900 focus:ring-4 focus:ring-primary/5 focus:border-primary outline-none transition-all font-bold"
                  />
                </div>
              </div>
              <div className="space-y-2">
                <label className="text-xs font-black uppercase tracking-widest text-muted-foreground/60">Selling Price</label>
                <div className="relative">
                  <span className="absolute left-6 top-1/2 -translate-y-1/2 font-bold text-primary">৳</span>
                  <input 
                    type="number"
                    step="0.01"
                    name="sellingPrice"
                    value={formData.sellingPrice}
                    onChange={handleChange}
                    className="w-full h-14 pl-12 pr-6 rounded-2xl border border-slate-200 dark:border-zinc-800 bg-slate-50/50 dark:bg-zinc-800/50 focus:bg-white dark:focus:bg-zinc-900 focus:ring-4 focus:ring-primary/5 focus:border-primary outline-none transition-all font-black text-primary text-lg"
                  />
                </div>
              </div>
            </div>
          </section>

          <section className="space-y-6">
            <h3 className="text-sm font-black uppercase tracking-widest text-primary/40">Visual Representation</h3>
            <div className="space-y-4">
                <label className="text-xs font-black uppercase tracking-widest text-muted-foreground/60">Product Image</label>
                
                {/* Existing Image Preview */}
                {formData.image && (
                  <div className="relative w-40 h-40 rounded-2xl overflow-hidden border border-slate-200 dark:border-zinc-800 shadow-lg">
                    <img src={formData.image} alt="Current product" className="w-full h-full object-cover" />
                    <div className="absolute inset-0 bg-black/40 flex items-center justify-center opacity-0 hover:opacity-100 transition-opacity">
                      <span className="text-white text-[10px] font-black uppercase tracking-widest">Current</span>
                    </div>
                  </div>
                )}

                <div className="relative group/file">
                  <input
                    type="file"
                    accept="image/*"
                    onChange={(e) => setImageFile(e.target.files?.[0] ?? null)}
                    className="absolute inset-0 w-full h-full opacity-0 cursor-pointer z-10"
                  />
                  <div className="w-full h-20 px-6 rounded-2xl border-2 border-dashed border-slate-200 dark:border-zinc-800 group-hover/file:border-primary transition-colors flex items-center gap-4 bg-slate-50/30 dark:bg-zinc-800/30">
                    <div className="w-10 h-10 rounded-full bg-slate-100 dark:bg-zinc-800 flex items-center justify-center text-xl">📸</div>
                    <div className="flex flex-col">
                      <span className="text-sm font-bold text-slate-700 dark:text-zinc-200">
                        {imageFile ? imageFile.name : 'Choose a new photo to replace existing...'}
                      </span>
                      <span className="text-xs text-muted-foreground uppercase font-bold tracking-wider">Leave empty to keep current</span>
                    </div>
                  </div>
                </div>
              </div>
          </section>

          <div className="pt-8 flex flex-col md:flex-row md:items-center justify-between gap-6 border-t border-slate-100 dark:border-zinc-800">
            <div 
              onClick={handleToggle}
              className="group flex items-center gap-4 cursor-pointer"
            >
               <div className={`w-14 h-8 rounded-full relative transition-colors duration-300 shadow-inner ${formData.isActive ? 'bg-primary' : 'bg-slate-200 dark:bg-zinc-800'}`}>
                 <div className={`w-6 h-6 rounded-full bg-white absolute top-1 transition-all duration-300 shadow-sm ${formData.isActive ? 'left-7' : 'left-1'}`} />
               </div>
               <div className="flex flex-col">
                 <span className="text-sm font-bold">Visible in Menu</span>
                 <span className="text-xs text-muted-foreground uppercase tracking-wider font-bold">{(formData.isActive ? 'Active' : 'Hidden')}</span>
               </div>
            </div>

            <button 
              type="submit" 
              disabled={submitting}
              className="btn btn-primary h-14 px-10 shadow-xl shadow-primary/20 hover:scale-[1.02] active:scale-95 disabled:opacity-50 disabled:scale-100 transition-all font-black text-sm uppercase tracking-widest"
            >
              {submitting ? 'Applying Mastery...' : 'Save Product Changes'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
