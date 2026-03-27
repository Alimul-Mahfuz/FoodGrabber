"use client";

export default function AdminDashboard() {
  return (
    <div>
      <span className="section-label">Overview</span>
      <h1 className="hero-title" style={{ fontSize: '2.5rem' }}>Dashboard.</h1>
      
      <div className="card-grid" style={{ marginTop: '30px' }}>
        <div className="card" style={{ padding: '1.5rem' }}>
          <p style={{ color: 'var(--muted)', fontSize: '0.85rem' }}>Total Revenue</p>
          <h2 style={{ fontSize: '1.75rem', marginTop: '0.5rem' }}>$12,450</h2>
        </div>
        <div className="card" style={{ padding: '1.5rem' }}>
          <p style={{ color: 'var(--muted)', fontSize: '0.85rem' }}>Active Orders</p>
          <h2 style={{ fontSize: '1.75rem', marginTop: '0.5rem' }}>24</h2>
        </div>
        <div className="card" style={{ padding: '1.5rem' }}>
          <p style={{ color: 'var(--muted)', fontSize: '0.85rem' }}>New Customers</p>
          <h2 style={{ fontSize: '1.75rem', marginTop: '0.5rem' }}>11</h2>
        </div>
      </div>

      <div className="card" style={{ marginTop: '2rem', padding: '1.5rem' }}>
         <h3>Recent Activity</h3>
         <p style={{ color: 'var(--muted)', marginTop: '1rem', fontSize: '0.9rem' }}>
           No recent activity to display.
         </p>
      </div>
    </div>
  );
}
