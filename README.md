# eticaret-api

Bu proje, mikroservis mimarisiyle geliştirilmiş bir e-ticaret API sistemidir. Sipariş oluşturma, stok kontrolü, ödeme işlemleri ve e-posta bildirimlerini yöneten dört temel servisten oluşur.

## Servisler

| Servis Adı       | Açıklama                                                                 |
|------------------|--------------------------------------------------------------------------|
| Order Service    | Sipariş oluşturur, stok kontrolü yapar, ödeme sürecini başlatır.         |
| Stock Service    | MongoDB üzerinde ürün stoklarını kontrol eder ve günceller.              |
| Payment Service  | Ödeme işlemlerini simüle eder.                                           |
| Mail Service     | Sipariş başarıyla tamamlandığında kullanıcıya e-posta gönderir.          |

## 🛠️ Kullanılan Teknolojiler

- ASP.NET Core Web API
- Entity Framework Core (Order Service için MSSQL)
- MongoDB (Stock Service için)
- RabbitMQ (Servisler arası mesajlaşma)
