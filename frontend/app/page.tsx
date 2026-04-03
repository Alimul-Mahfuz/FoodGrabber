import Link from 'next/link';

export default function HomePage() {
  const menuItems = [
    { id: 1, name: 'Truffle Tagliatelle', price: '$22.00', category: 'Pasta' },
    { id: 2, name: 'Citrus Glazed Salmon', price: '$28.00', category: 'Seafood' },
    { id: 3, name: 'Burrata & Tomato Salad', price: '$16.00', category: 'Salads' },
  ];

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
          <div style={{ display: 'flex', gap: '12px' }}>
            <Link href="/menu" className="btn btn-primary">Browse Menu</Link>
            <Link href="/about" className="btn btn-secondary">Our Story</Link>
          </div>
        </div>
        <div className="animate-float" style={{ background: '#F4F4F5', borderRadius: 'var(--radius)', width: '100%', height: '100%', minHeight: '400px', display: 'flex', alignItems: 'center', justifyContent: 'center', color: '#A1A1AA' }}>
          [ Featured Photo Placement ]
        </div>
      </section>

      {/* Product List */}
      <section style={{ padding: '80px 0' }}>
        <span className="section-label">A Glimpse of our menu</span>
        <h2 className="section-title">The Seasonal Classics.</h2>
        
        <div className="card-grid" style={{ padding: '0' }}>
          {menuItems.map((item, idx) => (
            <div key={item.id} className={`card animate-fade-up stagger-${idx + 1}`}>
              <div className="card-img">
                Dish {item.id} Preview
              </div>
              <div className="card-content">
                <span className="card-tag">{item.category}</span>
                <h3 style={{ fontSize: '1.2rem', marginBottom: '8px' }}>{item.name}</h3>
                <p style={{ color: 'var(--muted)', fontSize: '0.85rem' }}>
                  A harmonious blend of textures and seasonal aromas crafted for your palate.
                </p>
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginTop: '1.5rem' }}>
                   <span className="card-price">{item.price}</span>
                   <button className="btn btn-primary" style={{ padding: '0 16px', height: '36px', fontSize: '0.8rem' }}>Order Now</button>
                </div>
              </div>
            </div>
          ))}
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
