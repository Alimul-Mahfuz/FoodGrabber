"use client";

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { productService, Product } from '@/services/product-service';

export default function AdminProducts() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    async function fetchData() {
      try {
        const data = await productService.getProducts(1, 20);
        setProducts(data.items);
      } catch (err: any) {
        setError('Failed to load products. Make sure your API is running.');
        console.error(err);
      } finally {
        setLoading(false);
      }
    }
    fetchData();
  }, []);

  const formatCurrency = (val: number) => `৳ ${val.toLocaleString()}`;

  return (
    <div className="animate-fade-up">
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '2rem' }}>
        <div>
          <h1 className="hero-title" style={{ fontSize: '2.2rem' }}>Products.</h1>
          <p style={{ color: 'var(--muted)', marginTop: '0.4rem' }}>Manage your inventory and pricing.</p>
        </div>
        <Link href="/admin/products/new">
          <button className="btn btn-primary" style={{ height: '44px', padding: '0 1.5rem' }}>+ Add Product</button>
        </Link>
      </div>

      {loading ? (
        <div style={{ padding: '4rem', textAlign: 'center', color: 'var(--muted)' }}>
          <p>Loading products...</p>
        </div>
      ) : error ? (
        <div className="card" style={{ padding: '2rem', textAlign: 'center', color: '#B91C1C', borderColor: '#FEE2E2', background: '#FEF2F2' }}>
          {error}
        </div>
      ) : (
        <div className="card" style={{ overflow: 'hidden' }}>
          <table style={{ width: '100%', borderCollapse: 'collapse', textAlign: 'left' }}>
            <thead style={{ background: '#FAFAFA', borderBottom: '1px solid var(--border)' }}>
              <tr>
                <th style={thStyle}>Product</th>
                <th style={thStyle}>In Stock</th>
                <th style={thStyle}>Base Price</th>
                <th style={thStyle}>Selling Price</th>
                <th style={thStyle}>Status</th>
                <th style={{ ...thStyle, textAlign: 'right' }}>Actions</th>
              </tr>
            </thead>
            <tbody>
              {products.map((product) => (
                <tr key={product.id} className="table-row" style={{ borderBottom: '1px solid #F4F4F5' }}>
                  <td style={tdStyle}>
                    <Link href={`/admin/products/${product.id}`} style={{ textDecoration: 'none', color: 'inherit' }}>
                      <div style={{ fontWeight: 600, color: 'var(--primary)' }}>{product.name}</div>
                      <div style={{ fontSize: '0.8rem', color: 'var(--muted)', marginTop: '4px' }}>
                        {product.tags.join(', ')}
                      </div>
                    </Link>
                  </td>
                  <td style={tdStyle}>{product.currentStock} {product.stockUnit}</td>
                  <td style={tdStyle}>{formatCurrency(product.basePrice)}</td>
                  <td style={{ ...tdStyle, fontWeight: 700, color: 'var(--accent)' }}>
                    {formatCurrency(product.sellingPrice)}
                  </td>
                  <td style={tdStyle}>
                    <span style={{
                      padding: '4px 8px',
                      borderRadius: '4px',
                      fontSize: '0.75rem',
                      fontWeight: 600,
                      background: product.isActive ? '#ECFDF5' : '#FEF2F2',
                      color: product.isActive ? '#059669' : '#B91C1C'
                    }}>
                      {product.isActive ? 'Active' : 'Inactive'}
                    </span>
                  </td>
                  <td style={{ ...tdStyle, textAlign: 'right' }}>
                    <Link href={`/admin/products/${product.id}/edit`}>
                      <button className="btn btn-secondary" style={{ padding: '4px 12px', height: '32px', fontSize: '0.8rem' }}>Edit</button>
                    </Link>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Row Styling for extra polish */}
      <style jsx>{`
        .table-row:last-child { border-bottom: none; }
        .table-row:hover { background: #FAFAFB; }
      `}</style>
    </div>
  );
}

const thStyle: React.CSSProperties = {
  padding: '1rem 1.5rem',
  fontSize: '0.85rem',
  fontWeight: 600,
  color: 'var(--muted)',
  textTransform: 'uppercase',
  letterSpacing: '0.05em'
};

const tdStyle: React.CSSProperties = {
  padding: '1.25rem 1.5rem',
  fontSize: '0.9rem'
};
