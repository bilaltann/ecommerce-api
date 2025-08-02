# 🛒 E-Ticaret Mikroservis Projesi

Bu proje, mikroservis mimarisiyle tasarlanmış bir e-ticaret sistemidir. Servisler birbirleriyle RabbitMQ üzerinden mesajlaşır. MSSQL ve MongoDB gibi farklı veritabanları kullanılır.

## 🔧 Kullanılan Servisler

* **ProductAPI**: Ürün bilgilerini sağlar (MSSQL)
* **OrderAPI**: Sipariş oluşturma ve durumu yönetme (MSSQL)
* **StockAPI**: Ürün stoğunu kontrol eder ve günceller (MongoDB)
* **PaymentAPI**: Ödeme işlemlerini simüle eder
* **MailAPI**: Sipariş durumu hakkında bilgilendirici e-posta gönderir
* **RabbitMQ**: Servisler arası iletişimi sağlar (event-based)
* **Frontend**: React tabanlı kullanıcı arayüzü

## 🧩 Proje Akışı (Senaryo)

1. Kullanıcı bir ürün satın almak ister.

2. OrderAPI, siparişi oluşturur ve bir "OrderCreated" eventi gönderir.

3. StockAPI bu eventi alır, stok kontrolü yapar.
   * Stok yeterliyse, "StockReserved" eventi fırlatır.
   * Yetersizse, "StockNotReserved" eventi ile siparişi başarısız yapar.

4. Ödeme işlemi yapılır:
   * Başarılıysa "PaymentCompleted" eventi gönderilir, MailAPI bilgilendirici e-posta yollar.
   * Başarısızsa "PaymentFailed" eventi tetiklenir, stok geri alınır ve sipariş iptal edilir ve başarısız sipariş e-posta ile bilgilendirilir.

## 🚀 Projeyi Çalıştırma

> Projeyi çalıştırmak için 
  - Docker 
  - Docker Compose 
  - Git
yüklü olmalıdır.

### 1. Projeyi Klonla

git clone https://github.com/bilaltann/ecommerce-api

cd ecommerce-api


### 2. Compose ile Başlat

docker compose up --build

Bu komut tüm servisleri ayağa kaldırır. `init.sql` dosyası sayesinde MSSQL içindeki veritabanı ve örnek ürün verileri otomatik oluşturulur.

## 📌 Servis Erişim Noktaları

| Servis      | Adres                                                                    
| ----------- | ----------------------------------------------------
| ProductAPI  | [http://localhost:5005/api/products]   
| OrderAPI    | [http://localhost:5001]     
| StockAPI    | [http://localhost:5002/api/stocks]                           
| PaymentAPI  | [http://localhost:5003]                           
| MailAPI     | [http://localhost:5004]                           
| Frontend    | [http://localhost:3000]                         
| RabbitMQ UI | [http://localhost:15672] (guest/guest)           


## 🗃 Veritabanı Bilgileri

* **MSSQL**:

  * Sunucu: `mssql`
  * Kullanıcı: `sa`
  * Şifre: `StrongPassword123!`
  * Veritabanları: `ProductAPIDB`, `OrderAPIDB`

* **MongoDB**:

  * Port: `27018`
  * Veritabanı: `StockDb` (varsayılan olarak)



## 📬 Geri Bildirim

Herhangi bir sorun ya da öneriniz varsa, lütfen issue açmaktan çekinmeyin.

