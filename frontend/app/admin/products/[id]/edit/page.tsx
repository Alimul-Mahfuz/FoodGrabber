"use client";

import { useEffect, useState } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { productService, Product } from '@/services/product-service';

export default function EditProductPage() {
  const params = useParams();
  const router = useRouter();
  const id = params?.id as string;

  const [formData, setFormData] = useState<Partial<Product>>({
    name: '',
    description: '',
    quantity: 0,
    basePrice: 0,
    sellingPrice: 0,
    image: '',
    tags: [],
    isActive: true
  });

  const [tagsInput, setTagsInput] = useState('');
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!id) return;
    
    async function fetchData() {
      try {
        const product = await productService.getProductById(id);
        const { id: excludedId, userId, createdAt, updatedAt, ...rest } = product;
        setFormData(rest);
        setTagsInput(rest.tags?.join(', ') || '');
      } catch (err: any) {
        setError('Failed to fetch product for editing.');
      } finally {
        setLoading(false);
      }
    }
    fetchData();
  }, [id]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    setFormData(prev => ({
       ...prev,
       [name]: type === 'number' ? parseFloat(value) : value
    }));
  };

  const handleToggle = () => {
    setFormData(prev => ({ ...prev, isActive: !prev.isActive }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitting(true);
    setError('');

    try {
      // Convert tags string to array before submitting
      const finalTags = tagsInput.split(',').map(tag => tag.trim()).filter(t => t !== '');
      await productService.updateProduct(id, { ...formData, tags: finalTags });
      router.replace(`/admin/products/${id}`); 
    } catch (err: any) {
      setError(err.message || 'Failed to update product. Please try again.');
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) return <div style={{ padding: '80px', textAlign: 'center' }}>Loading form...</div>;

  return (
    <div className="animate-fade-up">
      <div style={{ marginBottom: '2rem' }}>
        <button 
          onClick={() => router.back()} 
          style={{ background: 'none', border: 'none', color: 'var(--muted)', cursor: 'pointer', fontSize: '0.85rem', marginBottom: '1rem' }}
        >
          ← Cancel and Return
        </button>
        <h1 className="hero-title" style={{ fontSize: '2.5rem' }}>Edit Product.</h1>
      </div>

      <div className="card" style={{ padding: '3rem', maxWidth: '800px' }}>
        {error && (
          <div style={{ background: '#FEF2F2', color: '#B91C1C', padding: '1rem', borderRadius: '12px', fontSize: '0.85rem', marginBottom: '2rem' }}>
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} style={{ display: 'grid', gap: '2rem' }}>
          {/* Basic Fields */}
          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '2rem' }}>
            <div style={fieldGroup}>
              <label style={labelStyle}>Product Name</label>
              <input 
                name="name"
                value={formData.name}
                onChange={handleChange}
                required
                style={inputStyle}
              />
            </div>
            <div style={fieldGroup}>
              <label style={labelStyle}>Tags (comma separated)</label>
              <input 
                value={tagsInput}
                onChange={(e) => setTagsInput(e.target.value)}
                placeholder="e.g. burger, fast-food"
                style={inputStyle}
              />
            </div>
          </div>

          <div style={fieldGroup}>
            <label style={labelStyle}>Description</label>
            <textarea 
               name="description"
               value={formData.description}
               onChange={handleChange}
               rows={4}
               style={{ ...inputStyle, height: 'auto', padding: '1rem', resize: 'vertical' }}
            />
          </div>

          {/* Quantities & Prices */}
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: '1.5rem' }}>
            <div style={fieldGroup}>
              <label style={labelStyle}>Quantity</label>
              <input 
                type="number"
                name="quantity"
                value={formData.quantity}
                onChange={handleChange}
                style={inputStyle}
              />
            </div>
            <div style={fieldGroup}>
              <label style={labelStyle}>Base Price (৳)</label>
              <input 
                type="number"
                step="0.01"
                name="basePrice"
                value={formData.basePrice}
                onChange={handleChange}
                style={inputStyle}
              />
            </div>
            <div style={fieldGroup}>
              <label style={labelStyle}>Selling Price (৳)</label>
              <input 
                type="number"
                step="0.01"
                name="sellingPrice"
                value={formData.sellingPrice}
                onChange={handleChange}
                style={{ ...inputStyle, color: 'var(--accent)', fontWeight: 700 }}
              />
            </div>
          </div>

          {/* Status Toggle */}
          <div style={{ display: 'flex', alignItems: 'center', gap: '1rem', padding: '1.5rem', background: '#F9FAFB', borderRadius: '12px' }}>
             <button 
               type="button"
               onClick={handleToggle}
               style={{ 
                 width: '40px', 
                 height: '24px', 
                 borderRadius: '20px', 
                 background: formData.isActive ? '#059669' : '#D1D5DB',
                 position: 'relative',
                 border: 'none',
                 cursor: 'pointer',
                 transition: 'background 0.3s'
               }}
             >
               <div style={{ 
                 width: '18px', 
                 height: '18px', 
                 background: 'white', 
                 borderRadius: '50%', 
                 position: 'absolute', 
                 top: '3px',
                 left: formData.isActive ? '19px' : '3px',
                 transition: 'left 0.3s'
               }} />
             </button>
             <span style={{ fontSize: '0.9rem', fontWeight: 600 }}>Available for Online Sale</span>
          </div>

          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '1rem', marginTop: '1rem' }}>
            <button 
              type="submit" 
              className="btn btn-primary" 
              disabled={submitting}
              style={{ padding: '0 2.5rem' }}
            >
              {submitting ? 'Updating...' : 'Save Product'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

const fieldGroup: React.CSSProperties = {
  display: 'grid',
  gap: '0.6rem'
};

const labelStyle: React.CSSProperties = {
  fontSize: '0.85rem',
  fontWeight: 700,
  color: 'var(--muted)',
  textTransform: 'uppercase',
  letterSpacing: '0.05em'
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
