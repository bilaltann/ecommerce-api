-- ==========================
-- OrderAPIDB Oluşturuluyor
-- ==========================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'OrderAPIDB')
BEGIN
    CREATE DATABASE OrderAPIDB;
END
GO

USE OrderAPIDB;
GO

-- Orders tablosu
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Orders' AND xtype='U')
BEGIN
    CREATE TABLE Orders (
        OrderId UNIQUEIDENTIFIER PRIMARY KEY,
        BuyerId UNIQUEIDENTIFIER NOT NULL,
        BuyerEmail NVARCHAR(200) NOT NULL,
        TotalPrice DECIMAL(18,2) NOT NULL,
        OrderStatus INT NOT NULL,
        CreatedDate DATETIME NOT NULL
    );
END
GO

-- OrderItems tablosu
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='OrderItems' AND xtype='U')
BEGIN
    CREATE TABLE OrderItems (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        OrderId UNIQUEIDENTIFIER NOT NULL,
        ProductId NVARCHAR(36) NOT NULL, -- DEĞİŞTİRİLDİ
        Count INT NOT NULL,
        Price DECIMAL(18,2) NOT NULL,
        FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
    );
END
GO

-- ==========================
-- ProductAPIDB Oluşturuluyor
-- ==========================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ProductAPIDB')
BEGIN
    CREATE DATABASE ProductAPIDB;
END
GO

USE ProductAPIDB;
GO

-- Products tablosu
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
BEGIN
    CREATE TABLE Products (
        ProductId NVARCHAR(36) PRIMARY KEY, -- DEĞİŞTİRİLDİ
        ProductName NVARCHAR(255) NOT NULL,
        Price DECIMAL(18,2) NOT NULL
    );
END
GO

-- Veri ekleme
INSERT INTO Products (ProductId, ProductName, Price)
VALUES
('5f559722-7cd4-4c5a-bc6e-252d28bc5ec3', 'AirPods Pro 2', 7499.99),
('8e9f559d-b7fa-4a7d-a64d-d7ed8ab587f2', 'Logitech MX Master 3S', 2699.50),
('a2fccc9e-3ae2-4fd3-8008-0a27911d1cd9', 'Samsung T7 SSD 1TB', 2999.00),
('b4515011-b582-4f2c-a0a3-3756e5c61010', 'Dell 27" Monitör', 8999.00),
('14fb1064-3668-47d0-8f19-36c981fcdd1c', 'SteelSeries Klavye Apex Pro', 4999.99);

PRINT '🟢 ProductId artık string (NVARCHAR), tüm veriler başarıyla eklendi.';
