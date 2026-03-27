"use client";

import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { authService } from '@/services/auth-service';

const menuItems = [
  { name: 'Dashboard', href: '/admin/dashboard', icon: '📊' },
  { name: 'Products', href: '/admin/products', icon: '🍲' },
  { name: 'Menu', href: '/admin/menu', icon: '📜' },
  { name: 'Customers', href: '/admin/customers', icon: '👥' },
  { name: 'Orders', href: '/admin/orders', icon: '🛒' },
];

export default function AdminSidebar() {
  const pathname = usePathname();

  return (
    <aside className="admin-sidebar">
      <div style={{ marginBottom: '2.5rem' }}>
        <h2 style={{ fontSize: '1.1rem', fontWeight: 800, textTransform: 'uppercase', letterSpacing: '0.1em' }}>
           Admin<span style={{ color: 'var(--accent)' }}>Panel</span>
        </h2>
      </div>

      <nav style={{ flex: 1 }}>
        {menuItems.map((item) => {
          const isActive = pathname === item.href;
          return (
            <Link 
              key={item.href} 
              href={item.href} 
              className={`sidebar-link ${isActive ? 'active' : ''}`}
            >
              <span style={{ fontSize: '1.2rem' }}>{item.icon}</span>
              {item.name}
            </Link>
          );
        })}
      </nav>

      <div style={{ borderTop: '1px solid var(--border)', paddingTop: '1.5rem', marginTop: 'auto' }}>
        <button 
          onClick={() => authService.logout()} 
          className="sidebar-link" 
          style={{ width: '100%', border: 'none', background: 'transparent', cursor: 'pointer' }}
        >
          <span>🚪</span>
          Sign Out
        </button>
      </div>
    </aside>
  );
}
