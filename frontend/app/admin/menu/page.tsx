"use client";

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { menuService, Menu } from '@/services/menu-service';

export default function AdminMenu() {
  const [menus, setMenus] = useState<Menu[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    async function fetchData() {
      try {
        const data = await menuService.getMenus(1, 20);
        setMenus(data.items);
      } catch (err: any) {
        setError('Failed to load menu list.');
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
          <h1 className="hero-title" style={{ fontSize: '2.2rem' }}>Menu Items.</h1>
          <p style={{ color: 'var(--muted)', marginTop: '0.4rem' }}>Bundle your products into delicious set meals.</p>
        </div>
        <Link href="/admin/menu/new">
          <button className="btn btn-primary" style={{ height: '44px', padding: '0 1.5rem' }}>+ Create Menu</button>
        </Link>
      </div>

      {loading ? (
        <div style={{ padding: '4rem', textAlign: 'center', color: 'var(--muted)' }}>
          <p>Loading menu items...</p>
        </div>
      ) : error ? (
        <div className="card" style={{ padding: '2rem', textAlign: 'center', color: '#B91C1C', borderColor: '#FEE2E2', background: '#FEF2F2' }}>
          {error}
        </div>
      ) : menus.length === 0 ? (
        <div className="card" style={{ padding: '3rem', textAlign: 'center', color: 'var(--muted)' }}>
          <p>No menus created yet. Start by bundling your products.</p>
        </div>
      ) : (
        <div className="card" style={{ overflow: 'hidden' }}>
          <table style={{ width: '100%', borderCollapse: 'collapse', textAlign: 'left' }}>
            <thead style={{ background: '#FAFAFA', borderBottom: '1px solid var(--border)' }}>
              <tr>
                <th style={thStyle}>Menu Item</th>
                <th style={thStyle}>Composition</th>
                <th style={thStyle}>Selling Price</th>
                <th style={thStyle}>Status</th>
                <th style={{ ...thStyle, textAlign: 'right' }}>Actions</th>
              </tr>
            </thead>
            <tbody>
              {menus.map((menu) => (
                <tr key={menu.id} className="table-row" style={{ borderBottom: '1px solid #F4F4F5' }}>
                  <td style={tdStyle}>
                    <div>
                      <div style={{ fontWeight: 600 }}>{menu.name}</div>
                      <div style={{ fontSize: '0.8rem', color: 'var(--muted)', marginTop: '4px' }}>
                        {menu.description}
                      </div>
                    </div>
                  </td>
                  <td style={tdStyle}>
                     <span style={{ fontSize: '0.85rem' }}>{menu.products.length} Products Included</span>
                  </td>
                  <td style={{ ...tdStyle, fontWeight: 700, color: 'var(--accent)', fontSize: '1.1rem' }}>
                    {formatCurrency(menu.sellingPrice)}
                  </td>
                  <td style={tdStyle}>
                    <span style={{
                      padding: '4px 8px',
                      borderRadius: '4px',
                      fontSize: '0.75rem',
                      fontWeight: 600,
                      background: menu.isActive ? '#ECFDF5' : '#FEF2F2',
                      color: menu.isActive ? '#059669' : '#B91C1C'
                    }}>
                      {menu.isActive ? 'Active' : 'Inactive'}
                    </span>
                  </td>
                  <td style={{ ...tdStyle, textAlign: 'right' }}>
                     <div style={{ display: 'flex', gap: '8px', justifyContent: 'flex-end' }}>
                        <Link href={`/admin/menu/${menu.id}`}>
                           <button className="btn btn-secondary" style={{ padding: '4px 12px', height: '32px', fontSize: '0.8rem' }}>View</button>
                        </Link>
                        <Link href={`/admin/menu/${menu.id}/edit`}>
                           <button className="btn btn-secondary" style={{ padding: '4px 12px', height: '32px', fontSize: '0.8rem' }}>Edit</button>
                        </Link>
                     </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

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
