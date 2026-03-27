"use client";

import { useState } from 'react';
import Link from 'next/link';
import { authService } from '@/services/auth-service';

export default function RegisterPage() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    
    try {
       await authService.register({ name, email, password });
    } catch (err: any) {
      setError(err.message || 'Error occurred during registration.');
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
        border: '1px solid var(--border)'
      }}>
        <div style={{ textAlign: 'center', marginBottom: '2.5rem' }}>
          <h1 style={{ fontSize: '1.75rem', marginBottom: '0.75rem' }}>Create Account.</h1>
          <p style={{ color: 'var(--muted)', fontSize: '0.9rem' }}>
             Join us for a fresh culinary experience.
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

        <form onSubmit={handleSubmit} style={{ display: 'grid', gap: '1.2rem' }}>
          <div style={{ display: 'grid', gap: '0.6rem' }}>
            <label style={{ fontSize: '0.85rem', fontWeight: 600 }}>Full Name</label>
            <input 
              type="text" 
              placeholder="Your name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
              style={{ 
                height: '48px', 
                padding: '0 1rem', 
                borderRadius: '12px', 
                border: '1px solid var(--border)',
                outline: 'none',
              }}
            />
          </div>

          <div style={{ display: 'grid', gap: '0.6rem' }}>
            <label style={{ fontSize: '0.85rem', fontWeight: 600 }}>Email</label>
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
              }}
            />
          </div>

          <div style={{ display: 'grid', gap: '0.6rem' }}>
            <label style={{ fontSize: '0.85rem', fontWeight: 600 }}>Password</label>
            <input 
              type="password" 
              placeholder="Create a password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              style={{ 
                height: '48px', 
                padding: '0 1rem', 
                borderRadius: '12px', 
                border: '1px solid var(--border)',
                outline: 'none',
              }}
            />
          </div>

          <button 
            type="submit" 
            className="btn btn-primary" 
            disabled={loading}
            style={{ width: '100%', height: '52px', marginTop: '0.5rem' }}
          >
            {loading ? 'Registering...' : 'Register'}
          </button>
        </form>

        <p style={{ marginTop: '2.5rem', textAlign: 'center', fontSize: '0.9rem', color: 'var(--muted)' }}>
          Already have an account? {' '}
          <Link href="/login" style={{ color: 'var(--primary)', fontWeight: 700, textDecoration: 'none' }}>
            Sign In
          </Link>
        </p>
      </div>
    </div>
  );
}
