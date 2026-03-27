"use client";

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { authService } from '@/services/auth-service';
import AdminSidebar from '@/components/AdminSidebar';

export default function AdminLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const router = useRouter();
  const [authorized, setAuthorized] = useState(false);

  useEffect(() => {
    // Check if the user is an admin
    if (!authService.isLoggedIn() || !authService.isAdmin()) {
      router.replace('/login');
    } else {
      setAuthorized(true);
    }
  }, [router]);

  if (!authorized) {
    return (
      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: '100vh' }}>
        <p>Verifying access...</p>
      </div>
    );
  }

  return (
    <div className="admin-layout">
      {/* Sidebar hidden on small screens can be toggled later, 
          for now using basic desktop layout */}
      <AdminSidebar />
      <main className="admin-content">
        {children}
      </main>
      
      {/* Hide the global Navbar on admin pages */}
      <style jsx global>{`
        header.header {
          display: none !important;
        }
      `}</style>
    </div>
  );
}
