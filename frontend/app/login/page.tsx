"use client";

import { useState, useEffect } from 'react';
import Link from 'next/link';
import { useRouter, useSearchParams } from 'next/navigation';
import { authService } from '@/services/auth-service';

export default function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const router = useRouter();
  const searchParams = useSearchParams();
  const returnUrl = searchParams.get('returnUrl') || '/';

  // Standard login check to prevent visiting when already logged in
  useEffect(() => {
    if (authService.isLoggedIn()) {
      const user = authService.getUser();
      if (user?.roles.includes('Admin')) {
        router.replace(returnUrl !== '/' ? returnUrl : '/admin/dashboard');
      } else {
        router.replace(returnUrl);
      }
    }
  }, [router, returnUrl]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    
    try {
      const data = await authService.login({ email, password });
      
      // Redirect based on role
      // use replace to prevent backward redirection to login page
      if (data.roles.includes('Admin')) {
        router.replace(returnUrl !== '/' ? returnUrl : '/admin/dashboard');
      } else {
        router.replace(returnUrl);
      }
    } catch (err: any) {
      setError(err.message || 'Failed to sign in. Please check your credentials.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ 
      minHeight: 'calc(100vh - 72px)', 
      display: 'flex', 
      alignItems: 'center', 
      justifyContent: 'center',
      padding: '2rem 1rem'
    }}>
      <div className="animate-fade-up" style={{ 
        width: '100%', 
        maxWidth: '420px', 
        background: 'white', 
        padding: '3rem 2.5rem', 
        borderRadius: '24px',
        border: '1px solid var(--border)',
        boxShadow: '0 10px 40px rgba(0,0,0,0.03)'
      }}>
        <div style={{ textAlign: 'center', marginBottom: '2.5rem' }}>
          <h1 style={{ fontSize: '1.75rem', marginBottom: '0.75rem' }}>Welcome Back.</h1>
          <p style={{ color: 'var(--muted)', fontSize: '0.9rem' }}>
            Enter your credentials to access your account.
          </p>
        </div>

        {error && (
          <div style={{ 
            background: '#FEF2F2', 
            color: '#B91C1C', 
            padding: '1rem', 
            borderRadius: '12px', 
            fontSize: '0.85rem', 
            marginBottom: '1.5rem',
            border: '1px solid #FEE2E2'
          }}>
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} style={{ display: 'grid', gap: '1.5rem' }}>
          <div style={{ display: 'grid', gap: '0.6rem' }}>
            <label style={{ fontSize: '0.85rem', fontWeight: 600, color: 'var(--primary)' }}>Email Address</label>
            <input 
              type="email" 
              placeholder="name@example.com"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              style={{ 
                height: '48px', 
                padding: '0 1rem', 
                borderRadius: '12px', 
                border: '1px solid var(--border)',
                outline: 'none',
                fontSize: '0.95rem',
                transition: 'border-color 0.2s'
              }}
              onFocus={(e) => e.target.style.borderColor = 'var(--primary)'}
              onBlur={(e) => e.target.style.borderColor = 'var(--border)'}
            />
          </div>

          <div style={{ display: 'grid', gap: '0.6rem' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <label style={{ fontSize: '0.85rem', fontWeight: 600, color: 'var(--primary)' }}>Password</label>
              <Link href="/forgot-password" style={{ fontSize: '0.8rem', color: 'var(--muted)', textDecoration: 'none' }}>
                Forgot?
              </Link>
            </div>
            <input 
              type="password" 
              placeholder="••••••••"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              style={{ 
                height: '48px', 
                padding: '0 1rem', 
                borderRadius: '12px', 
                border: '1px solid var(--border)',
                outline: 'none',
                fontSize: '0.95rem',
                transition: 'border-color 0.2s'
              }}
              onFocus={(e) => e.target.style.borderColor = 'var(--primary)'}
              onBlur={(e) => e.target.style.borderColor = 'var(--border)'}
            />
          </div>

          <button 
            type="submit" 
            className="btn btn-primary" 
            disabled={loading}
            style={{ width: '100%', height: '52px', marginTop: '0.5rem' }}
          >
            {loading ? 'Signing in...' : 'Sign In'}
          </button>
        </form>

        <div style={{ marginTop: '2.5rem', textAlign: 'center', borderTop: '1px solid var(--border)', paddingTop: '1.5rem' }}>
          <p style={{ fontSize: '0.9rem', color: 'var(--muted)' }}>
            Don't have an account? {' '}
            <Link href="/register" style={{ color: 'var(--primary)', fontWeight: 700, textDecoration: 'none' }}>
              Create one
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
}
