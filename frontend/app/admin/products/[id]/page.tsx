"use client";

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { productService, Product } from '@/services/product-service';

export default function ProductDetails() {
  const params = useParams();
  const router = useRouter();
  const [product, setProduct] = useState<Product | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const id = params?.id as string;

  useEffect(() => {
    if (!id) return;
    
    async function fetchData() {
      try {
        const data = await productService.getProductById(id);
        setProduct(data);
      } catch (err: any) {
        setError('Failed to fetch product details.');
        console.error(err);
      } finally {
        setLoading(false);
      }
    }

    fetchData();
  }, [id]);

  const formatCurrency = (val: number) => `৳ ${val.toLocaleString()}`;

  if (loading) {
    return <div style={{ padding: '80px', textAlign: 'center' }}>Loading details...</div>;
  }

  if (error || !product) {
    return (
      <div className="container" style={{ padding: '60px 0' }}>
         <div className="card" style={{ padding: '3rem', textAlign: 'center' }}>
           <p style={{ color: '#B91C1C' }}>{error || 'Product not found.'}</p>
           <button onClick={() => router.back()} className="btn btn-secondary" style={{ marginTop: '2rem' }}>Go Back</button>
         </div>
      </div>
    );
  }

  const profit = product.sellingPrice - product.basePrice;

  return (
    <div className="max-w-5xl mx-auto animate-fade-up">
      <div className="mb-10 flex justify-between items-end">
        <div>
          <button 
            onClick={() => router.back()} 
            className="group flex items-center gap-2 text-sm font-bold text-muted-foreground hover:text-primary transition-colors mb-4"
          >
            <span className="group-hover:-translate-x-1 transition-transform">←</span>
            Back to Products
          </button>
          <div className="flex items-center gap-4">
            <h1 className="text-4xl font-black tracking-tighter">{product.name}</h1>
            <span className={`px-4 py-1.5 rounded-full text-[10px] font-black uppercase tracking-widest ${
              product.isActive 
                ? 'bg-emerald-500/10 text-emerald-600 border border-emerald-500/20' 
                : 'bg-rose-500/10 text-rose-600 border border-rose-500/20'
            }`}>
              {product.isActive ? 'Active Listing' : 'Inactive'}
            </span>
          </div>
        </div>
        
        <button 
          onClick={() => router.push(`/admin/products/${id}/edit`)}
          className="btn btn-primary px-8 shadow-xl shadow-primary/20 hover:scale-105 active:scale-95 transition-all"
        >
          Edit Product
        </button>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        {/* Left Column: Image and Description */}
        <div className="lg:col-span-2 space-y-8">
          {product.image && (
            <div className="bg-white dark:bg-zinc-900 rounded-[2.5rem] overflow-hidden border border-slate-200 dark:border-zinc-800 shadow-2xl shadow-black/5 aspect-video relative group">
              <img 
                src={product.image} 
                alt={product.name}
                className="w-full h-full object-cover transition-transform duration-700 group-hover:scale-110"
              />
              <div className="absolute inset-0 bg-linear-to-t from-black/40 to-transparent flex items-bottom p-8">
                 <p className="mt-auto text-white/80 text-sm font-bold backdrop-blur-sm bg-black/10 px-4 py-2 rounded-full border border-white/10 uppercase tracking-widest">Product Showcase</p>
              </div>
            </div>
          )}

          <div className="bg-white dark:bg-zinc-900 rounded-[2.5rem] p-10 border border-slate-200 dark:border-zinc-800 shadow-2xl shadow-black/5">
             <h3 className="text-xs font-black uppercase tracking-widest text-primary/40 mb-8 border-b border-slate-50 dark:border-zinc-800 pb-4">Product Overview</h3>
             <div className="space-y-6">
                <div className="space-y-2">
                   <label className="text-[10px] font-black uppercase tracking-widest text-muted-foreground/60">Description</label>
                   <p className="text-lg text-slate-700 dark:text-zinc-300 leading-relaxed font-medium">
                     {product.description || 'No description provided for this product.'}
                   </p>
                </div>

                <div className="pt-6">
                  <label className="text-[10px] font-black uppercase tracking-widest text-muted-foreground/60 block mb-4">Discovery Tags</label>
                  <div className="flex flex-wrap gap-2">
                    {product.tags.length > 0 ? product.tags.map(tag => (
                       <span key={tag} className="px-4 py-2 rounded-xl bg-slate-100 dark:bg-zinc-800 text-sm font-bold text-slate-600 dark:text-zinc-400 border border-slate-200/50 dark:border-zinc-700/50">
                         {tag}
                       </span>
                    )) : <span className="text-sm italic text-muted-foreground">No tags attached.</span>}
                  </div>
                </div>
             </div>
          </div>

          <div className="bg-white dark:bg-zinc-900 rounded-[2rem] p-8 border border-slate-200 dark:border-zinc-800 flex items-center justify-between">
             <div className="flex flex-col gap-1">
               <span className="text-[10px] font-black uppercase tracking-widest text-muted-foreground/60">System ID</span>
               <span className="text-xs font-mono bg-slate-50 dark:bg-zinc-800 px-3 py-1 rounded-full border border-slate-100 dark:border-zinc-700">{product.id}</span>
             </div>
             <div className="flex flex-col items-end gap-1">
               <span className="text-[10px] font-black uppercase tracking-widest text-muted-foreground/60">Last Modification</span>
               <span className="text-sm font-bold">{new Date(product.updatedAt).toLocaleDateString()} — {new Date(product.updatedAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</span>
             </div>
          </div>
        </div>

        {/* Right Column: Key Metrics */}
        <div className="space-y-8">
           <div className="bg-primary text-primary-foreground rounded-[2.5rem] p-10 shadow-2xl shadow-primary/30 relative overflow-hidden group">
              <div className="absolute -right-8 -top-8 w-32 h-32 bg-white/10 rounded-full blur-3xl group-hover:scale-150 transition-transform duration-700" />
              <h3 className="text-[10px] font-black uppercase tracking-widest opacity-40 mb-8 border-b border-white/10 pb-4">Financials</h3>
              
              <div className="space-y-8">
                <div>
                  <label className="text-[10px] font-black uppercase tracking-widest opacity-60 block mb-1">Selling Value</label>
                  <span className="text-4xl font-black tracking-tighter">{formatCurrency(product.sellingPrice)}</span>
                </div>
                
                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <label className="text-[10px] font-black uppercase tracking-widest opacity-60 block mb-1">Base Cost</label>
                    <span className="text-lg font-bold">{formatCurrency(product.basePrice)}</span>
                  </div>
                  <div>
                    <label className="text-[10px] font-black uppercase tracking-widest opacity-60 block mb-1">Margin</label>
                    <span className="text-xl font-black text-emerald-300">+{formatCurrency(profit)}</span>
                  </div>
                </div>
              </div>
           </div>

           <div className="bg-white dark:bg-zinc-900 rounded-[2.5rem] p-10 border border-slate-200 dark:border-zinc-800 shadow-xl shadow-black/5">
              <h3 className="text-xs font-black uppercase tracking-widest text-primary/40 mb-8 border-b border-slate-50 dark:border-zinc-800 pb-4">Availability</h3>
              
              <div className="space-y-8">
                <div className="flex items-end gap-3 translate-y-1">
                  <span className="text-5xl font-black tracking-tighter text-slate-800 dark:text-zinc-100">{product.currentStock}</span>
                  <span className="text-sm font-black uppercase py-2 tracking-widest text-muted-foreground">{product.stockUnit}s remaining</span>
                </div>

                <div className="h-2 w-full bg-slate-100 dark:bg-zinc-800 rounded-full overflow-hidden">
                  <div className={`h-full rounded-full transition-all duration-1000 ${product.currentStock > 10 ? 'bg-emerald-500' : 'bg-rose-500'}`} style={{ width: `${Math.min((product.currentStock / 100) * 100, 100)}%` }} />
                </div>
                
                <p className="text-[10px] font-black uppercase tracking-wider text-muted-foreground/60 text-center">
                  Inventory level is currently <span className="text-slate-800 dark:text-zinc-100 font-black">{product.currentStock > 10 ? 'Healthy' : 'Critical'}</span>
                </p>
              </div>
           </div>
        </div>
      </div>
    </div>
  );
}

const fieldGroup: React.CSSProperties = {
  marginBottom: '1.8rem'
};

const labelStyle: React.CSSProperties = {
  display: 'block',
  fontSize: '0.75rem',
  fontWeight: 700,
  textTransform: 'uppercase',
  color: 'var(--muted)',
  letterSpacing: '0.1em',
  marginBottom: '0.5rem'
};

const valueStyle: React.CSSProperties = {
  fontSize: '1rem',
  color: 'var(--primary)'
};
