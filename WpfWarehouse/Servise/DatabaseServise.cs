using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfWarehouse.Services
{
    public class DatabaseService : IDisposable
    {
        private readonly WpfWarehouseDbEntities _context;

        public DatabaseService()
        {
            _context = new WpfWarehouseDbEntities();
        }

        // ========== КАТЕГОРИИ ==========
        public List<categories> GetCategories()
        {
            try
            {
                return _context.categories.OrderBy(x => x.id).ToList();
            }
            catch (Exception ex)
            {
                ShowError("загрузки категорий", ex);
                return new List<categories>();
            }
        }

        public void AddCategory(categories category)
        {
            try
            {
                _context.categories.Add(category);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления категории: {ex.Message}");
            }
        }

        public void UpdateCategory(categories category)
        {
            try
            {
                var item = _context.categories.Find(category.id);
                if (item != null)
                {
                    item.title = category.title;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления категории: {ex.Message}");
            }
        }

        public void DeleteCategory(int id)
        {
            try
            {
                var item = _context.categories.Find(id);
                if (item != null)
                {
                    _context.categories.Remove(item);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления категории: {ex.Message}");
            }
        }

        public List<categories> SearchCategories(string text)
        {
            try
            {
                return _context.categories
                    .Where(x => x.title.Contains(text))
                    .OrderBy(x => x.id)
                    .ToList();
            }
            catch (Exception ex)
            {
                ShowError("поиска категорий", ex);
                return new List<categories>();
            }
        }

        // ========== ПОСТАВЩИКИ ==========
        public List<suppliers> GetSuppliers()
        {
            try
            {
                return _context.suppliers.OrderBy(x => x.id).ToList();
            }
            catch (Exception ex)
            {
                ShowError("загрузки поставщиков", ex);
                return new List<suppliers>();
            }
        }

        public void AddSupplier(suppliers supplier)
        {
            try
            {
                _context.suppliers.Add(supplier);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления поставщика: {ex.Message}");
            }
        }

        public void UpdateSupplier(suppliers supplier)
        {
            try
            {
                var item = _context.suppliers.Find(supplier.id);
                if (item != null)
                {
                    item.name = supplier.name;
                    item.contact_person = supplier.contact_person;
                    item.phone = supplier.phone;
                    item.email = supplier.email;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления поставщика: {ex.Message}");
            }
        }

        public void DeleteSupplier(int id)
        {
            try
            {
                var item = _context.suppliers.Find(id);
                if (item != null)
                {
                    _context.suppliers.Remove(item);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления поставщика: {ex.Message}");
            }
        }

        public List<suppliers> SearchSuppliers(string text)
        {
            try
            {
                return _context.suppliers
                    .Where(x => x.name.Contains(text) ||
                                (x.contact_person != null && x.contact_person.Contains(text)))
                    .OrderBy(x => x.id)
                    .ToList();
            }
            catch (Exception ex)
            {
                ShowError("поиска поставщиков", ex);
                return new List<suppliers>();
            }
        }

        // ========== ТОВАРЫ ==========
        public List<products> GetProducts()
        {
            try
            {
                return _context.products
                    .Include("categories")
                    .Include("suppliers")
                    .OrderBy(x => x.id)
                    .ToList();
            }
            catch (Exception ex)
            {
                ShowError("загрузки товаров", ex);
                return new List<products>();
            }
        }

        public void AddProduct(products product)
        {
            try
            {
                _context.products.Add(product);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления товара: {ex.Message}");
            }
        }

        public void UpdateProduct(products product)
        {
            try
            {
                var item = _context.products.Find(product.id);
                if (item != null)
                {
                    item.title = product.title;
                    item.price = product.price;
                    item.quantity = product.quantity;
                    item.category_id = product.category_id;
                    item.supplier_id = product.supplier_id;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления товара: {ex.Message}");
            }
        }

        public void DeleteProduct(int id)
        {
            try
            {
                var item = _context.products.Find(id);
                if (item != null)
                {
                    _context.products.Remove(item);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления товара: {ex.Message}");
            }
        }

        public List<products> SearchProducts(string text)
        {
            try
            {
                return _context.products
                    .Include("categories")
                    .Include("suppliers")
                    .Where(x => x.title.Contains(text) ||
                                x.categories.title.Contains(text) ||
                                x.suppliers.name.Contains(text))
                    .OrderBy(x => x.id)
                    .ToList();
            }
            catch (Exception ex)
            {
                ShowError("поиска товаров", ex);
                return new List<products>();
            }
        }

        // ========== ПОСТАВКИ ==========
        public List<deliveries> GetDeliveries()
        {
            try
            {
                return _context.deliveries
                    .Include("products")
                    .OrderBy(x => x.id)
                    .ToList();
            }
            catch (Exception ex)
            {
                ShowError("загрузки поставок", ex);
                return new List<deliveries>();
            }
        }

        public void AddDelivery(deliveries delivery)
        {
            try
            {
                _context.deliveries.Add(delivery);
                _context.SaveChanges();

                var product = _context.products.Find(delivery.product_id);
                if (product != null)
                {
                    product.quantity += delivery.quantity;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления поставки: {ex.Message}");
            }
        }

        public void UpdateDelivery(deliveries delivery, int oldQuantity, int oldProductId)
        {
            try
            {
                var oldProduct = _context.products.Find(oldProductId);
                if (oldProduct != null)
                    oldProduct.quantity -= oldQuantity;

                var item = _context.deliveries.Find(delivery.id);
                if (item != null)
                {
                    item.product_id = delivery.product_id;
                    item.delivery_date = delivery.delivery_date;
                    item.quantity = delivery.quantity;
                    item.cost = delivery.cost;
                    _context.SaveChanges();
                }

                var newProduct = _context.products.Find(delivery.product_id);
                if (newProduct != null)
                    newProduct.quantity += delivery.quantity;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления поставки: {ex.Message}");
            }
        }

        public void DeleteDelivery(int id, int productId, int quantity)
        {
            try
            {
                var product = _context.products.Find(productId);
                if (product != null)
                    product.quantity -= quantity;

                var item = _context.deliveries.Find(id);
                if (item != null)
                {
                    _context.deliveries.Remove(item);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления поставки: {ex.Message}");
            }
        }

        public List<deliveries> SearchDeliveries(string text)
        {
            try
            {
                return _context.deliveries
                    .Include("products")
                    .Where(x => x.products.title.Contains(text))
                    .OrderBy(x => x.id)
                    .ToList();
            }
            catch (Exception ex)
            {
                ShowError("поиска поставок", ex);
                return new List<deliveries>();
            }
        }

        // ========== ВСПОМОГАТЕЛЬНЫЕ ==========
        public List<categories> GetCategoriesList() => _context.categories.OrderBy(x => x.title).ToList();
        public List<suppliers> GetSuppliersList() => _context.suppliers.OrderBy(x => x.name).ToList();
        public List<products> GetProductsList() => _context.products.OrderBy(x => x.title).ToList();

        private void ShowError(string action, Exception ex)
        {
            MessageBox.Show($"Ошибка {action}: {ex.Message}", "Ошибка",
                          MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void Dispose() => _context?.Dispose();
    }
}