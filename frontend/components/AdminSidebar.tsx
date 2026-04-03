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
    <aside className="w-72 h-screen sticky top-0 bg-white dark:bg-zinc-900 border-r border-slate-200 dark:border-zinc-800 flex flex-col p-8">
      <div className="mb-10">
        <h2 className="text-xl font-black tracking-tighter uppercase">
           Admin<span className="text-primary italic">Panel</span>
        </h2>
      </div>

      <nav className="flex-1 flex flex-col gap-1">
        {menuItems.map((item) => {
          const isActive = pathname === item.href;
          return (
            <Link 
              key={item.href} 
              href={item.href} 
              className={`flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 font-semibold ${
                isActive 
                  ? 'bg-primary text-primary-foreground shadow-lg shadow-primary/20 translate-x-1' 
                  : 'text-muted-foreground hover:bg-slate-100 dark:hover:bg-zinc-800 hover:text-foreground'
              }`}
            >
              <span className="text-xl leading-none">{item.icon}</span>
              {item.name}
            </Link>
          );
        })}
      </nav>

      <div className="mt-auto pt-6 border-t border-slate-200 dark:border-zinc-800">
        <button 
          onClick={() => authService.logout()} 
          className="w-full flex items-center gap-3 px-4 py-3 rounded-xl text-muted-foreground font-semibold hover:bg-slate-100 dark:hover:bg-zinc-800 hover:text-destructive transition-all duration-200"
        >
          <span className="text-xl leading-none">🚪</span>
          Sign Out
        </button>
      </div>
    </aside>
  );
}
