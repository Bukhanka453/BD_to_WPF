-- ============================================
-- Создание базы данных для WPF Warehouse
-- ============================================

CREATE DATABASE WpfWarehouseDb;
GO

USE WpfWarehouseDb;
GO

-- ============================================
-- Таблица категорий
-- ============================================
CREATE TABLE categories (
    id INT IDENTITY(1,1) PRIMARY KEY,
    title VARCHAR(60) NOT NULL UNIQUE
);
GO

-- ============================================
-- Таблица поставщиков
-- ============================================
CREATE TABLE suppliers (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    contact_person VARCHAR(60),
    phone VARCHAR(20),
    email VARCHAR(100) UNIQUE
);
GO

-- ============================================
-- Таблица товаров
-- ============================================
CREATE TABLE products (
    id INT IDENTITY(1,1) PRIMARY KEY,
    title VARCHAR(60) NOT NULL,
    price DECIMAL(10, 2) NOT NULL CHECK (price > 0),
    quantity INT NOT NULL CHECK (quantity >= 0),
    category_id INT NOT NULL REFERENCES categories(id) ON DELETE CASCADE,
    supplier_id INT NOT NULL REFERENCES suppliers(id) ON DELETE CASCADE,
    UNIQUE(title, price, quantity, category_id, supplier_id)
);
GO

-- ============================================
-- Таблица поставок
-- ============================================
CREATE TABLE deliveries (
    id INT IDENTITY(1,1) PRIMARY KEY,
    product_id INT NOT NULL REFERENCES products(id) ON DELETE CASCADE,
    delivery_date DATETIME NOT NULL DEFAULT GETDATE(),
    quantity INT NOT NULL CHECK (quantity > 0),
    cost DECIMAL(10, 2) NOT NULL CHECK (cost > 0)
);
GO

-- ============================================
-- Тестовые данные: категории
-- ============================================
INSERT INTO categories (title) VALUES
('Электроника'),
('Офисные принадлежности'),
('Компьютерная техника'),
('Мебель');
GO

-- ============================================
-- Тестовые данные: поставщики
-- ============================================
INSERT INTO suppliers (name, contact_person, phone, email) VALUES
('ООО "ТехноПоставка"', 'Иванов Иван', '+7(495)123-45-67', 'info@tehno.ru'),
('ИП Петров', 'Петров Петр', '+7(916)789-12-34', 'petrov@mail.ru'),
('ЗАО "ОфисМаркет"', 'Сидорова Анна', '+7(812)345-67-89', 'anna@officemarket.ru');
GO

-- ============================================
-- Тестовые данные: товары
-- ============================================
INSERT INTO products (title, price, quantity, category_id, supplier_id) VALUES
('Ноутбук Lenovo', 45000.00, 10, 1, 1),
('Мышь компьютерная', 1200.00, 50, 3, 1),
('Бумага А4', 350.00, 200, 2, 3),
('Стул офисный', 4500.00, 25, 4, 2),
('Клавиатура Logitech', 2500.00, 30, 3, 1);
GO

-- ============================================
-- Тестовые данные: поставки
-- (безопасный формат даты YYYYMMDD HH:MM:SS)
-- ============================================
INSERT INTO deliveries (product_id, delivery_date, quantity, cost) VALUES
(1, '20240115 10:30:00', 5, 40000.00),
(2, '20240120 14:15:00', 20, 1000.00),
(3, '20240125 09:00:00', 100, 300.00),
(4, '20240201 11:45:00', 10, 4000.00),
(5, '20240205 16:20:00', 15, 2200.00);
GO
