"use client";

import { useState, useEffect } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { menuService, MenuProduct, Menu } from '@/services/menu-service';
import { productService, Product } from '@/services/product-service';

export default function EditMenuPage() {
  const params = useParams();
  const router = useRouter();
  const id = params?.id as string;
  
  // State for products list to select from
  const [availableProducts, setAvailableProducts] = useState<Product[]>([]);
  
  // Form State
  const [formData, setFormData] = useState<Partial<Menu>>({
    name: '',
    description: '',
    sellingPrice: 0,
    isActive: true,
    products: []
  });

  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!id) return;

    async function fetchData() {
      try {
        const [menuData, allProducts] = await Promise.all([
          menuService.getMenuById(id),
          productService.getProducts(1, 100)
        ]);
        
        // Clean the menu data to match the PUT structure (omit non-updatable fields if needed)
        const { id: excludedId, createdAt, updatedAt, ...rest } = menuData;
        setFormData(rest);
        setAvailableProducts(allProducts.items);
      } catch (err: any) {
        setError('Failed to load menu details for editing.');
      } finally {
        setLoading(false);
      }
    }
    fetchData();
  }, [id]);

  const addProductRow = () => {
    setFormData(prev => ({
      ...prev,
      products: [...(prev.products || []), { productId: '', quantity: 1 }]
    }));
  };

  const removeProductRow = (index: number) => {
    setFormData(prev => ({
      ...prev,
      products: prev.products?.filter((_, i) => i !== index)
    }));
  };

  const updateProductRow = (index: number, field: keyof MenuProduct, value: string | number) => {
    const updated = [...(formData.products || [])];
    updated[index] = { ...updated[index], [field]: value };
    setFormData(prev => ({ ...prev, products: updated }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.products || formData.products.length === 0) {
      setError('Please add at least one product to this menu.');
      return;
    }
    
    setSubmitting(true);
    setError('');

    try {
      // The API expects productId and quantity. 
      // Filter out any 'id' (the relation ID) that might have come from the GET response,
      // though the PUT might accept it, it's safer to send exactly what's requested.
      const cleanedProducts = formData.products.map(p => ({
        productId: p.productId,
        quantity: p.quantity
      }));

      await menuService.updateMenu(id, { ...formData, products: cleanedProducts });
      router.replace(`/admin/menu/${id}`);
    } catch (err: any) {
      setError(err.message || 'Failed to update menu item.');
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) return <div style={{ padding: '80px', textAlign: 'center' }}>Loading menu form...</div>;

  return (
    <div className="animate-fade-up">
      <div style={{ marginBottom: '2.5rem' }}>
        <button 
          onClick={() => router.back()} 
          style={{ background: 'none', border: 'none', color: 'var(--muted)', cursor: 'pointer', fontSize: '0.85rem', marginBottom: '1rem' }}
        >
          ← Cancel Changes
        </button>
        <h1 className="hero-title" style={{ fontSize: '2.5rem' }}>Edit Menu Item.</h1>
      </div>

      <form onSubmit={handleSubmit}>
        <div style={{ display: 'grid', gridTemplateColumns: '1.2fr 0.8fr', gap: '2rem' }}>
          
          {/* Left Side: General Info & Products */}
          <div className="grid" style={{ display: 'grid', gap: '2rem' }}>
            <div className="card" style={{ padding: '2.5rem' }}>
              <h3 style={sectionHeading}>General Details</h3>
              
              <div style={fieldGroup}>
                <label style={labelStyle}>Menu Name</label>
                <input 
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  required
                  style={inputStyle}
                />
              </div>

              <div style={fieldGroup}>
                <label style={labelStyle}>Description</label>
                <textarea 
                   value={formData.description}
                   onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                   rows={3}
                   style={{ ...inputStyle, height: 'auto', padding: '1rem', resize: 'vertical' }}
                />
              </div>
            </div>

            <div className="card" style={{ padding: '2.5rem' }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1.5rem' }}>
                 <h3 style={{ ...sectionHeading, marginBottom: 0 }}>Included Products</h3>
                 <button type="button" onClick={addProductRow} className="btn btn-secondary" style={{ padding: '0 1rem', height: '36px', fontSize: '0.85rem' }}>
                    + Add Item
                 </button>
              </div>

              <div style={{ display: 'grid', gap: '1rem' }}>
                 {formData.products?.map((p, idx) => (
                   <div key={idx} style={{ display: 'grid', gridTemplateColumns: '1fr 100px 48px', gap: '12px', alignItems: 'center' }}>
                      <select 
                        value={p.productId} 
                        onChange={(e) => updateProductRow(idx, 'productId', e.target.value)}
                        required
                        style={inputStyle}
                      >
                         <option value="">Select a product...</option>
                         {availableProducts.map((avail) => (
                           <option key={avail.id} value={avail.id}>{avail.name}</option>
                         ))}
                      </select>
                      <input 
                        type="number"
                        min="1"
                        value={p.quantity}
                        onChange={(e) => updateProductRow(idx, 'quantity', parseInt(e.target.value))}
                        style={inputStyle}
                      />
                      <button type="button" onClick={() => removeProductRow(idx)} style={{ height: '48px', borderRadius: '12px', background: '#FEF2F2', border: 'none', color: '#B91C1C', cursor: 'pointer' }}>
                        ✕
                      </button>
                   </div>
                 ))}
              </div>
            </div>
          </div>

          {/* Right Side: Pricing & Visibility */}
          <div className="grid" style={{ display: 'grid', gap: '2rem', height: 'fit-content' }}>
            <div className="card" style={{ padding: '2.5rem' }}>
                <h3 style={sectionHeading}>Pricing & Visibility</h3>
                
                <div style={fieldGroup}>
                  <label style={labelStyle}>Selling Price (৳)</label>
                  <input 
                    type="number"
                    step="0.01"
                    value={formData.sellingPrice}
                    onChange={(e) => setFormData({ ...formData, sellingPrice: parseFloat(e.target.value) })}
                    required
                    style={{ ...inputStyle, fontSize: '1.4rem', fontWeight: 800, color: 'var(--accent)' }}
                  />
                </div>

                <div style={{ display: 'flex', alignItems: 'center', gap: '1rem', padding: '1.5rem', background: '#F9FAFB', borderRadius: '12px', marginTop: '1rem' }}>
                   <button 
                     type="button"
                     onClick={() => setFormData({ ...formData, isActive: !formData.isActive })}
                     style={{ 
                       width: '40px', height: '24px', borderRadius: '20px', 
                       background: formData.isActive ? '#059669' : '#D1D5DB',
                       position: 'relative', border: 'none', cursor: 'pointer'
                     }}
                   >
                     <div style={{ 
                       width: '18px', height: '18px', background: 'white', borderRadius: '50%', 
                       position: 'absolute', top: '3px', left: formData.isActive ? '19px' : '3px',
                       transition: 'all 0.3s'
                     }} />
                   </button>
                   <span style={{ fontSize: '0.9rem', fontWeight: 600 }}>Active - Show in App</span>
                </div>

                <button 
                  type="submit" 
                  disabled={submitting}
                  className="btn btn-primary" 
                  style={{ width: '100%', marginTop: '2.5rem', height: '56px', fontSize: '1.1rem' }}
                >
                  {submitting ? 'Updating...' : 'Save Menu Item'}
                </button>

                {error && (
                  <p style={{ marginTop: '1.5rem', color: '#B91C1C', fontSize: '0.85rem' }}>{error}</p>
                )}
            </div>
          </div>

        </div>
      </form>
    </div>
  );
}

const sectionHeading: React.CSSProperties = {
  fontSize: '1.1rem',
  fontWeight: 700,
  marginBottom: '1.5rem',
  color: 'var(--primary)'
};

const fieldGroup: React.CSSProperties = {
  display: 'grid',
  gap: '0.6rem',
  marginBottom: '1.8rem'
};

const labelStyle: React.CSSProperties = {
  fontSize: '0.75rem',
  fontWeight: 700,
  color: 'var(--muted)',
  textTransform: 'uppercase',
  letterSpacing: '0.1em'
};

const inputStyle: React.CSSProperties = {
  height: '48px',
  padding: '0 1rem',
  borderRadius: '12px',
  border: '1px solid var(--border)',
  outline: 'none',
  fontSize: '0.95rem',
  width: '100%',
  transition: 'border-color 0.2s',
};
