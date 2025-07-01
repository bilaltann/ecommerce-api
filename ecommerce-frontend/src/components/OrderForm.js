import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { v4 as uuidv4 } from 'uuid';
import './OrderForm.css';

const OrderForm = () => {
    const [products, setProducts] = useState([]);
    const [stocks, setStocks] = useState([]);
    const [mergedProducts, setMergedProducts] = useState([]);
    const [buyerEmail, setBuyerEmail] = useState('');
    const [cart, setCart] = useState([]);
    const [statusMessage, setStatusMessage] = useState({ type: '', message: '' });
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        // Ürünleri çek
        axios.get('https://localhost:7211/api/products')
            .then(res => setProducts(res.data))
            .catch(err => setStatusMessage({ type: 'error', message: 'Ürünler yüklenemedi!' }));
        // Stokları çek
        axios.get('https://localhost:7179/api/stocks')
            .then(res => setStocks(res.data))
            .catch(err => setStatusMessage({ type: 'error', message: 'Stoklar yüklenemedi!' }));
    }, []);

    useEffect(() => {
        // Ürün ve stokları productId ile birleştir
        const combined = products.map(product => {
            const stockObj = stocks.find(stock => stock.productId === product.productId);
            return {
                ...product,
                stock: stockObj ? stockObj.count : 0
            };
        });
        setMergedProducts(combined);
    }, [products, stocks]);

    const handleAddToCart = (product) => {
        setCart(prevCart => {
            const existingItem = prevCart.find(item => item.productId === product.productId);
            if (existingItem && existingItem.count >= product.stock) {
                setStatusMessage({ type: 'error', message: `${product.productName} için stok yetersiz!` });
                return prevCart;
            }
            if (existingItem) {
                return prevCart.map(item =>
                    item.productId === product.productId ? { ...item, count: item.count + 1 } : item
                );
            } else {
                // Sadece ihtiyaç olan alanları cart'a ekle
                return [...prevCart, { productId: product.productId, productName: product.productName, price: product.price, stock: product.stock, count: 1 }];
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
            orderItems: cart.map(item => ({
                productId: item.productId,
                count: item.count,
                price: item.price
            }))
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
                    {mergedProducts.map(product => {
                        const quantityInCart = cart.find(item => item.productId === product.productId)?.count || 0;
                        const isOutOfStock = quantityInCart >= product.stock;
                        return (
                            <div key={product.productId} className="product-item">
                                <span>
                                    {product.productName} - {product.price.toFixed(2)} TL
                                    {product.stock <= 15 && <span className="low-stock-warning"> Stok tükenmek üzere!</span>}
                                    <span> (Stok: {product.stock})</span>
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
                                    <span>{item.productName} x {item.count}</span>
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
