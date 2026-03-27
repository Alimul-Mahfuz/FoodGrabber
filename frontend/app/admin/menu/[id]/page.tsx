"use client";

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { menuService, Menu } from '@/services/menu-service';
import { productService, Product } from '@/services/product-service';

export default function MenuDetailsPage() {
  const params = useParams();
  const router = useRouter();
  const id = params?.id as string;

  const [menu, setMenu] = useState<Menu | null>(null);
  const [products, setProducts] = useState<Record<string, Product>>({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!id) return;
    
    async function fetchData() {
      try {
        // Fetch menu and all products (to map names)
        const [menuData, allProducts] = await Promise.all([
          menuService.getMenuById(id),
          productService.getProducts(1, 100)
        ]);

        const productMap = allProducts.items.reduce((acc: any, p: Product) => {
          acc[p.id] = p;
          return acc;
        }, {});

        setMenu(menuData);
        setProducts(productMap);
      } catch (err: any) {
        setError('Failed to fetch menu details.');
        console.error(err);
      } finally {
        setLoading(false);
      }
    }

    fetchData();
  }, [id]);

  const formatCurrency = (val: number) => `৳ ${val.toLocaleString()}`;

  if (loading) return <div style={{ padding: '80px', textAlign: 'center' }}>Loading menu details...</div>;

  if (error || !menu) {
    return (
      <div className="container" style={{ padding: '60px 0' }}>
         <div className="card" style={{ padding: '3rem', textAlign: 'center' }}>
           <p style={{ color: '#B91C1C' }}>{error || 'Menu not found.'}</p>
           <button onClick={() => router.back()} className="btn btn-secondary" style={{ marginTop: '2rem' }}>Go Back</button>
         </div>
      </div>
    );
  }

  return (
    <div className="animate-fade-up">
      <div style={{ marginBottom: '2.5rem' }}>
        <button 
          onClick={() => router.back()} 
          style={{ background: 'none', border: 'none', color: 'var(--muted)', cursor: 'pointer', fontSize: '0.85rem', marginBottom: '1rem' }}
        >
          ← Back to Menu Items
        </button>
        <h1 className="hero-title" style={{ fontSize: '2.5rem' }}>{menu.name}</h1>
        <p style={{ color: 'var(--muted)', marginTop: '0.4rem' }}>{menu.description}</p>
      </div>

      <div className="grid" style={{ display: 'grid', gridTemplateColumns: '1.2fr 0.8fr', gap: '30px' }}>
        
        {/* Composition List */}
        <div className="card" style={{ padding: '2.5rem' }}>
          <h3 style={sectionHeading}>Bundle Composition</h3>
          <p style={{ fontSize: '0.85rem', color: 'var(--muted)', marginBottom: '1.5rem' }}>This set menu includes the following products:</p>
          
          <div style={{ display: 'grid', gap: '1rem' }}>
            {menu.products.map((item, idx) => {
               const detail = products[item.productId];
               return (
                 <div key={idx} style={{ 
                    display: 'flex', 
                    justifyContent: 'space-between', 
                    alignItems: 'center', 
                    padding: '1.2rem', 
                    background: '#F9FAFB', 
                    borderRadius: '12px' 
                 }}>
                   <div>
                     <p style={{ fontWeight: 600, fontSize: '1rem' }}>{detail?.name || 'Unknown Product'}</p>
                     <p style={{ fontSize: '0.75rem', color: 'var(--muted)', marginTop: '4px' }}>ID: {item.productId}</p>
                   </div>
                   <div style={{ textAlign: 'right' }}>
                      <p style={{ fontSize: '1.1rem', fontWeight: 800 }}>x{item.quantity}</p>
                   </div>
                 </div>
               );
            })}
          </div>
        </div>

        {/* Pricing & Management */}
        <div style={{ display: 'grid', gap: '1.5rem', height: 'fit-content' }}>
          <div className="card" style={{ padding: '2.5rem' }}>
            <h3 style={sectionHeading}>Pricing</h3>
            <p style={{ fontSize: '2rem', fontWeight: 800, color: 'var(--accent)' }}>
              {formatCurrency(menu.sellingPrice)}
            </p>
            
            <div style={{ marginTop: '2rem', paddingTop: '2rem', borderTop: '1px solid var(--border)' }}>
               <label style={labelStyle}>Status</label>
               <span style={{
                  display: 'inline-block',
                  padding: '6px 12px',
                  borderRadius: '6px',
                  fontSize: '0.8rem',
                  fontWeight: 700,
                  marginTop: '0.5rem',
                  background: menu.isActive ? '#ECFDF5' : '#FEF2F2',
                  color: menu.isActive ? '#059669' : '#B91C1C'
               }}>
                  {menu.isActive ? 'AVAILABLE ONLINE' : 'INTERNAL/HIDDEN'}
               </span>
            </div>
          </div>

          <div className="card" style={{ padding: '2rem' }}>
             <h3 style={{ ...sectionHeading, fontSize: '0.9rem', marginBottom: '1rem' }}>Management</h3>
             <div style={{ display: 'grid', gap: '10px' }}>
                <button onClick={() => router.push(`/admin/menu/${id}/edit`)} className="btn btn-primary" style={{ width: '100%' }}>Edit Menu</button>
                <button className="btn btn-secondary" style={{ width: '100%', color: '#B91C1C', borderColor: '#FEE2E2' }}>Delete Item</button>
             </div>
          </div>

          <div style={{ padding: '0 1rem' }}>
             <p style={{ fontSize: '0.7rem', color: 'var(--muted)' }}>Created: {new Date((menu as any).createdAt).toLocaleDateString()}</p>
             <p style={{ fontSize: '0.7rem', color: 'var(--muted)' }}>Last Update: {new Date((menu as any).updatedAt).toLocaleDateString()}</p>
          </div>
        </div>

      </div>
    </div>
  );
}

const sectionHeading: React.CSSProperties = {
  fontSize: '1.1rem',
  fontWeight: 700,
  marginBottom: '1.5rem',
  color: 'var(--primary)'
};

const labelStyle: React.CSSProperties = {
  display: 'block',
  fontSize: '0.7rem',
  fontWeight: 800,
  color: 'var(--muted)',
  textTransform: 'uppercase',
  letterSpacing: '0.1em'
};
