import React, { useState } from 'react';
import axios from 'axios';
import { v4 as uuidv4 } from 'uuid';
import './OrderForm.css';

const availableProducts = [
    { productId: "5f559722-7cd4-4c5a-bc6e-252d28bc5ec3", name: "Akıllı Saat", price: 1500.50 , stock:1530},
    { productId: "8e9f559d-b7fa-4a7d-a64d-d7ed8ab587f2", name: "Bluetooth Kulaklık", price: 899.99, stock:701 },
    { productId: "a2fccc9e-3ae2-4fd3-8008-0a27911d1cd9", name: "Mekanik Klavye", price: 1200.00, stock: 1981 },
    { productId: "b4515011-b582-4f2c-a0a3-3756e5c61010", name: "Gaming Mouse", price: 750.75, stock:4855 },
    { productId: "14fb1064-3668-47d0-8f19-36c981fcdd1c", name: "Webcam", price: 450.00, stock: 246 }
];

const OrderForm = () => {
    const [buyerEmail, setBuyerEmail] = useState('');
    const [cart, setCart] = useState([]);
    const [statusMessage, setStatusMessage] = useState({ type: '', message: '' });
    const [isLoading, setIsLoading] = useState(false);

   

    const handleAddToCart = (product) => {
        setCart(prevCart => {
            const existingItem = prevCart.find(item => item.productId === product.productId);
            if (existingItem && existingItem.count >= product.stock) {
                setStatusMessage({ type: 'error', message: `${product.name} için stok yetersiz!` });
                return prevCart;
            }
            if (existingItem) {
                return prevCart.map(item =>
                    item.productId === product.productId ? { ...item, count: item.count + 1 } : item
                );
            } else {
                return [...prevCart, { ...product, count: 1 }];
            }
        });
    };

    const handleRemoveFromCart = (productId) => {
        setCart(prevCart => prevCart.filter(item => item.productId !== productId));
    };

    const calculateTotal = () => {
        return cart.reduce((total, item) => total + item.price * item.count, 0).toFixed(2);
    };

    const handleSubmitOrder = async () => {
        if (!buyerEmail || cart.length === 0) {
            setStatusMessage({ type: 'error', message: 'Lütfen e-posta adresinizi girin ve sepete ürün ekleyin.' });
            return;
        }
        setIsLoading(true);
        setStatusMessage({ type: '', message: '' });

        const orderData = {
            buyerId: uuidv4(),
            buyerEmail: buyerEmail,
            orderItems: cart.map(item => ({ productId: item.productId, count: item.count, price: item.price }))
        };

        try {
            const API_URL = 'http://localhost:5195/api/orders';
            const response = await axios.post(API_URL, orderData);
            if (response.status === 200) {
                setStatusMessage({ type: 'success', message: 'Siparişiniz başarıyla oluşturuldu!' });
                setCart([]);
                setBuyerEmail('');
            }
        } catch (error) {
            console.error("Sipariş gönderme hatası:", error);
            setStatusMessage({ type: 'error', message: "Sipariş oluşturulurken bir hata oluştu." });
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="order-container">
            <header>
                <h1>E-Ticaret Sipariş Arayüzü</h1>
            </header>

            <main className="main-content">
                <div className="product-list">
                    <h2>Ürünler</h2>
                    {availableProducts.map(product => {
                        const quantityInCart = cart.find(item => item.productId === product.productId)?.count || 0;
                        const isOutOfStock = quantityInCart >= product.stock;
                        return (
                            <div key={product.productId} className="product-item">
                                <span>
                                    {product.name} - {product.price.toFixed(2)} TL
                                    {product.stock <= 15 && <span className="low-stock-warning"> Stok tükenmek üzere!</span>}
                                </span>
                                <button onClick={() => handleAddToCart(product)} disabled={isOutOfStock}>
                                    {isOutOfStock ? 'Stok Tükendi' : 'Sepete Ekle'}
                                </button>
                            </div>
                        );
                    })}
                </div>

                <div className="order-summary">
                    <h2>Sipariş Özeti</h2>
                    <div className="buyer-info">
                        <label htmlFor="email">Alıcı E-posta:</label>
                        <input
                            type="email"
                            id="email"
                            value={buyerEmail}
                            onChange={(e) => setBuyerEmail(e.target.value)}
                            placeholder="ornek@mail.com"
                        />
                    </div>

                    <div className="cart">
                        <h3>Sepetiniz</h3>
                        {cart.length === 0 ? (
                            <p>Sepetiniz boş.</p>
                        ) : (
                            cart.map(item => (
                                <div key={item.productId} className="cart-item">
                                    <span>{item.name} x {item.count}</span>
                                    <span>{(item.price * item.count).toFixed(2)} TL</span>
                                    <button className="remove-btn" onClick={() => handleRemoveFromCart(item.productId)}>Kaldır</button>
                                </div>
                            ))
                        )}
                    </div>

                    <div className="total-price">
                        <strong>Toplam Tutar: {calculateTotal()} TL</strong>
                    </div>

                    <button className="submit-order-btn" onClick={handleSubmitOrder} disabled={isLoading || cart.length === 0}>
                        {isLoading ? 'Sipariş Gönderiliyor...' : 'Siparişi Oluştur'}
                    </button>

                    {statusMessage.message && (
                        <div className={`status-message ${statusMessage.type}`} style={{ marginTop: '10px' }}>
                            {statusMessage.message}
                        </div>
                    )}
                </div>
            </main>
        </div>
    );
};

export default OrderForm;
