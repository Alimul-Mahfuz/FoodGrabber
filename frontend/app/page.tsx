"use client";

import Link from 'next/link';
import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { productService } from '@/services/product-service';
import { authService } from '@/services/auth-service';

export default function HomePage() {
  const router = useRouter();
  const [products, setProducts] = useState<Array<{
    id: string;
    name: string;
    description: string;
    sellingPrice: number;
    image: string | null;
    tags: string[];
  }>>([]);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  const handleOrderNow = () => {
    if (authService.isLoggedIn()) {
      router.push('/admin/menu');
      return;
    }

    router.push('/login?returnUrl=/admin/menu');
  };

  useEffect(() => {
    let isMounted = true;

    const loadProducts = async () => {
      try {
        const response = await productService.getProducts(1, 10, { auth: false });

        if (!isMounted) {
          return;
        }

        setProducts(response.items);
        setError(null);
      } catch (err) {
        if (!isMounted) {
          return;
        }

        const message = err instanceof Error ? err.message : 'Failed to load products';
        setError(message);
      } finally {
        if (isMounted) {
          setIsLoading(false);
        }
      }
    };

    loadProducts();

    return () => {
      isMounted = false;
    };
  }, []);

  return (
    <div className="container">
      {/* Hero */}
      <section className="hero">
        <div className="animate-slide-left">
          <span className="section-label">Now serving daily</span>
          <h1 className="hero-title">Experience <br />Honest Flavors.</h1>
          <p className="hero-description">
            Thoughtfully sourced ingredients, prepared with care, 
            delivered with ease. Join us on a journey of refined culinary simplicity.
          </p>
          <div style={{ display: 'flex', gap: '12px', marginTop: '12px' }}>
            <Link href="/menu" className="btn btn-primary">Browse Menu</Link>
            <Link href="/about" className="btn btn-secondary">Our Story</Link>
          </div>
        </div>
        <div className="animate-float" style={{ background: '#F4F4F5', borderRadius: 'var(--radius)', width: '100%', height: '100%', minHeight: '400px', display: 'flex', alignItems: 'center', justifyContent: 'center', color: '#A1A1AA' }}>
          [ Featured Photo Placement ]
        </div>
      </section>

      {/* Product List */}
      <section className="home-products-section" style={{ padding: '80px 0' }}>
        <span className="section-label">A Glimpse of our menu</span>
        <h2 className="section-title">The Seasonal Classics.</h2>
        
        {error && (
          <div style={{ padding: '20px', backgroundColor: '#fee2e2', borderRadius: 'var(--radius)', marginBottom: '20px', color: '#991b1b' }}>
            Error loading products: {error}
          </div>
        )}

        <div className="card-grid home-products-grid" style={{ padding: '0' }}>
          {isLoading ? (
            <p style={{ textAlign: 'center', color: 'var(--muted)', padding: '40px 0' }}>
              Loading products...
            </p>
          ) : products.length > 0 ? (
            products.map((product, idx) => (
              <div key={product.id} className={`card home-card animate-fade-up stagger-${idx + 1}`}>
                {product.image ? (
                  <div className="card-img" style={{ backgroundImage: `url(${product.image})`, backgroundSize: 'cover', backgroundPosition: 'center' }} />
                ) : (
                  <div className="card-img" style={{ background: '#F4F4F5', display: 'flex', alignItems: 'center', justifyContent: 'center', color: '#A1A1AA' }}>
                    No Image
                  </div>
                )}
                <div className="card-content">
                  <span className="card-tag">{product.tags?.[0] || 'Food'}</span>
                  <h3 style={{ fontSize: '1.05rem', marginBottom: '6px' }}>{product.name}</h3>
                  <p style={{ color: 'var(--muted)', fontSize: '0.8rem', lineHeight: '1.45' }}>
                    {product.description}
                  </p>
                  <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginTop: '1rem', gap: '12px' }}>
                    <span className="card-price">${product.sellingPrice.toFixed(2)}</span>
                    <button onClick={handleOrderNow} className="btn btn-primary" style={{ padding: '0 14px', height: '32px', fontSize: '0.75rem' }}>Order Now</button>
                  </div>
                </div>
              </div>
            ))
          ) : (
            <p style={{ textAlign: 'center', color: 'var(--muted)', padding: '40px 0' }}>
              {error ? `Unable to load products. ${error}` : 'No products available'}
            </p>
          )}
        </div>
      </section>

      {/* Philosophy Section */}
      <section style={{ padding: '100px 0', borderTop: '1px solid var(--border)', textAlign: 'center' }}>
        <div style={{ maxWidth: '700px', margin: '0 auto' }}>
          <h2 style={{ fontSize: '2.4rem', marginBottom: '1.5rem' }}>Minimalist kitchen. <br />Bold expression.</h2>
          <p style={{ color: 'var(--muted)', fontSize: '1.125rem', marginBottom: '3rem' }}>
            We believe that great food doesn't need to be complicated. We focus on the quality of every single ingredient to provide a transparent dining experience that celebrates pure taste.
          </p>
          <Link href="/menu" style={{ textDecoration: 'underline', color: 'var(--primary)', fontWeight: 600 }}>Explore our sourcing partners</Link>
        </div>
      </section>

      {/* Footer */}
      <footer>
        <div className="footer-content">
          <Link href="/" className="logo">FoodGrabber.</Link>
          <div className="nav-links">
            <Link href="/terms" className="nav-link" style={{ fontSize: '0.75rem' }}>Terms</Link>
            <Link href="/privacy" className="nav-link" style={{ fontSize: '0.75rem' }}>Privacy</Link>
          </div>
        </div>
        <p style={{ marginTop: '20px', color: '#A1A1AA', fontSize: '0.75rem' }}>
           &copy; 2026 FoodGrabber. Crafted with precision for the modern diner.
        </p>
      </footer>
    </div>
  );
}
