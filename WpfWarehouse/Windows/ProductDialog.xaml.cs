using System;
using System.Windows;
using WpfWarehouse.Services;

namespace WpfWarehouse.Windows
{
    public partial class ProductDialog : Window
    {
        public products Product { get; private set; }
        private DatabaseService _db;

        public ProductDialog(products product = null)
        {
            InitializeComponent();
            _db = new DatabaseService();
            LoadComboBoxes();

            if (product != null)
            {
                Product = product;
                TitleBox.Text = product.title;
                PriceBox.Text = product.price.ToString("0.00");
                QuantityBox.Text = product.quantity.ToString();
                CategoryComboBox.SelectedValue = product.category_id;
                SupplierComboBox.SelectedValue = product.supplier_id;
                Title = "Редактирование товара";
            }
            else
            {
                Product = new products();
                Title = "Добавление товара";
            }
        }

        private void LoadComboBoxes()
        {
            try
            {
                // Загружаем категории
                var categories = _db.GetCategoriesList();
                CategoryComboBox.ItemsSource = categories;
                CategoryComboBox.SelectedValuePath = "id";
                CategoryComboBox.DisplayMemberPath = "title";

                // Загружаем поставщиков
                var suppliers = _db.GetSuppliersList();
                SupplierComboBox.ItemsSource = suppliers;
                SupplierComboBox.SelectedValuePath = "id";
                SupplierComboBox.DisplayMemberPath = "name";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool Validate()
        {
            bool isValid = true;

            // Проверка названия
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                TitleError.Text = "Название обязательно!";
                isValid = false;
            }
            else if (TitleBox.Text.Length > 60)
            {
                TitleError.Text = "Максимум 60 символов!";
                isValid = false;
            }
            else
            {
                TitleError.Text = "";
            }

            // Проверка цены
            if (!decimal.TryParse(PriceBox.Text, out decimal price) || price <= 0)
            {
                PriceError.Text = "Введите положительное число!";
                isValid = false;
            }
            else
            {
                PriceError.Text = "";
            }

            // Проверка количества
            if (!int.TryParse(QuantityBox.Text, out int quantity) || quantity < 0)
            {
                QuantityError.Text = "Введите неотрицательное целое число!";
                isValid = false;
            }
            else
            {
                QuantityError.Text = "";
            }

            // Проверка категории
            if (CategoryComboBox.SelectedValue == null)
            {
                CategoryError.Text = "Выберите категорию!";
                isValid = false;
            }
            else
            {
                CategoryError.Text = "";
            }

            // Проверка поставщика
            if (SupplierComboBox.SelectedValue == null)
            {
                SupplierError.Text = "Выберите поставщика!";
                isValid = false;
            }
            else
            {
                SupplierError.Text = "";
            }

            return isValid;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                MessageBox.Show("Исправьте ошибки в форме!", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Product.title = TitleBox.Text.Trim();
            Product.price = decimal.Parse(PriceBox.Text);
            Product.quantity = int.Parse(QuantityBox.Text);
            Product.category_id = (int)CategoryComboBox.SelectedValue;
            Product.supplier_id = (int)SupplierComboBox.SelectedValue;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}