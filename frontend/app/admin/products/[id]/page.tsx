"use client";

import { useEffect, useState, use } from 'react';
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

  return (
    <div className="animate-fade-up">
      <div style={{ marginBottom: '2.5rem' }}>
        <button 
          onClick={() => router.back()} 
          style={{ 
            background: 'none', 
            border: 'none', 
            color: 'var(--muted)', 
            cursor: 'pointer', 
            fontSize: '0.85rem', 
            marginBottom: '1rem',
            display: 'flex',
            alignItems: 'center',
            gap: '8px'
          }}
        >
          ← Back to Products
        </button>
        <h1 className="hero-title" style={{ fontSize: '2.5rem' }}>{product.name}</h1>
      </div>

      <div className="grid" style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '30px' }}>
        {/* Basic Information */}
        <div className="card" style={{ padding: '2.5rem' }}>
          <h3 style={{ marginBottom: '2rem', borderBottom: '1px solid var(--border)', paddingBottom: '0.8rem' }}>General Information</h3>
          
          <div style={fieldGroup}>
            <label style={labelStyle}>Description</label>
            <p style={valueStyle}>{product.description}</p>
          </div>

          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '20px' }}>
            <div style={fieldGroup}>
              <label style={labelStyle}>Category Tags</label>
              <div style={{ display: 'flex', flexWrap: 'wrap', gap: '8px' }}>
                {product.tags.map(tag => (
                   <span key={tag} className="card-tag" style={{ margin: 0 }}>{tag}</span>
                ))}
              </div>
            </div>
            <div style={fieldGroup}>
              <label style={labelStyle}>Status</label>
              <p style={{ ...valueStyle, color: product.isActive ? '#059669' : '#B91C1C', fontWeight: 700 }}>
                {product.isActive ? 'Active' : 'Inactive'}
              </p>
            </div>
          </div>
        </div>

        {/* Inventory & Pricing */}
        <div className="card" style={{ padding: '2.5rem' }}>
          <h3 style={{ marginBottom: '2rem', borderBottom: '1px solid var(--border)', paddingBottom: '0.8rem' }}>Inventory & Pricing</h3>
          
          <div style={fieldGroup}>
            <label style={labelStyle}>Quantity in Stock</label>
            <p style={{ ...valueStyle, fontSize: '1.2rem', fontWeight: 600 }}>{product.quantity} units</p>
          </div>

          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '20px' }}>
            <div style={fieldGroup}>
              <label style={labelStyle}>Base Price</label>
              <p style={valueStyle}>{formatCurrency(product.basePrice)}</p>
            </div>
            <div style={fieldGroup}>
              <label style={labelStyle}>Selling Price</label>
              <p style={{ ...valueStyle, color: 'var(--accent)', fontWeight: 800, fontSize: '1.5rem' }}>
                {formatCurrency(product.sellingPrice)}
              </p>
            </div>
          </div>

          <div style={{ marginTop: '2rem', padding: '1.5rem', background: '#F9FAFB', borderRadius: '12px' }}>
             <p style={{ fontSize: '0.8rem', color: 'var(--muted)' }}>
               Profit per unit: <span style={{ fontWeight: 700, color: '#059669' }}>{formatCurrency(product.sellingPrice - product.basePrice)}</span>
             </p>
          </div>
        </div>
      </div>

      {/* Meta Information */}
      <div className="card" style={{ marginTop: '30px', padding: '1.5rem', display: 'flex', justifyContent: 'space-between' }}>
        <p style={{ fontSize: '0.75rem', color: 'var(--muted)' }}>Product ID: {product.id}</p>
        <p style={{ fontSize: '0.75rem', color: 'var(--muted)' }}>Last Updated: {new Date(product.updatedAt).toLocaleDateString()} at {new Date(product.updatedAt).toLocaleTimeString()}</p>
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
