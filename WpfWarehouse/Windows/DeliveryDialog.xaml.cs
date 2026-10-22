using System;
using System.Linq;
using System.Windows;
using WpfWarehouse.Services;

namespace WpfWarehouse.Windows
{
    public partial class DeliveryDialog : Window
    {
        public deliveries Delivery { get; private set; }
        private DatabaseService _db;
        private bool _isEditMode;

        public DeliveryDialog(deliveries delivery = null)
        {
            InitializeComponent();
            _db = new DatabaseService();
            LoadProducts();

            if (delivery != null)
            {
                _isEditMode = true;
                Delivery = delivery;

                // Заполняем поля
                ProductComboBox.SelectedValue = delivery.product_id;
                DatePicker.SelectedDate = delivery.delivery_date;
                QuantityBox.Text = delivery.quantity.ToString();
                CostBox.Text = delivery.cost.ToString("0.00");

                Title = "Редактирование поставки";
                CalculateTotal();
            }
            else
            {
                _isEditMode = false;
                Delivery = new deliveries();
                DatePicker.SelectedDate = DateTime.Now;
                Title = "Добавление поставки";
            }

            // Подписываемся на события для подсчета итога
            QuantityBox.TextChanged += (s, e) => CalculateTotal();
            CostBox.TextChanged += (s, e) => CalculateTotal();
        }

        private void LoadProducts()
        {
            try
            {
                var products = _db.GetProductsList();
                ProductComboBox.ItemsSource = products;
                ProductComboBox.SelectedValuePath = "id";
                ProductComboBox.DisplayMemberPath = "title";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateTotal()
        {
            try
            {
                if (decimal.TryParse(QuantityBox.Text, out decimal quantity) &&
                    decimal.TryParse(CostBox.Text, out decimal cost))
                {
                    TotalText.Text = (quantity * cost).ToString("0.00");
                }
                else
                {
                    TotalText.Text = "0";
                }
            }
            catch
            {
                TotalText.Text = "0";
            }
        }

        private bool Validate()
        {
            bool isValid = true;

            // Проверка товара
            if (ProductComboBox.SelectedValue == null)
            {
                ProductError.Text = "Выберите товар!";
                isValid = false;
            }
            else
            {
                ProductError.Text = "";
            }

            // Проверка даты
            if (DatePicker.SelectedDate == null)
            {
                DateError.Text = "Выберите дату!";
                isValid = false;
            }
            else
            {
                DateError.Text = "";
            }

            // Проверка количества
            if (!int.TryParse(QuantityBox.Text, out int quantity) || quantity <= 0)
            {
                QuantityError.Text = "Введите положительное целое число!";
                isValid = false;
            }
            else
            {
                QuantityError.Text = "";
            }

            // Проверка цены
            if (!decimal.TryParse(CostBox.Text, out decimal cost) || cost <= 0)
            {
                CostError.Text = "Введите положительное число!";
                isValid = false;
            }
            else
            {
                CostError.Text = "";
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

            Delivery.product_id = (int)ProductComboBox.SelectedValue;
            Delivery.delivery_date = DatePicker.SelectedDate.Value;
            Delivery.quantity = int.Parse(QuantityBox.Text);
            Delivery.cost = decimal.Parse(CostBox.Text);

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