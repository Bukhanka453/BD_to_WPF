using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace WpfWarehouse.Windows
{
    public partial class SupplierDialog : Window
    {
        public suppliers Supplier { get; private set; }

        public SupplierDialog(suppliers supplier = null)
        {
            InitializeComponent();

            if (supplier != null)
            {
                Supplier = supplier;
                NameBox.Text = supplier.name;
                ContactBox.Text = supplier.contact_person;
                PhoneBox.Text = supplier.phone;
                EmailBox.Text = supplier.email;
                Title = "Редактирование поставщика";
            }
            else
            {
                Supplier = new suppliers();
                Title = "Добавление поставщика";
            }
        }

        private bool Validate()
        {
            bool isValid = true;

            // Проверка названия
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                NameError.Text = "Название обязательно!";
                isValid = false;
            }
            else if (NameBox.Text.Length > 100)
            {
                NameError.Text = "Максимум 100 символов!";
                isValid = false;
            }
            else
            {
                NameError.Text = "";
            }

            // Проверка контактного лица (необязательно)
            if (ContactBox.Text.Length > 60)
            {
                ContactError.Text = "Максимум 60 символов!";
                isValid = false;
            }
            else
            {
                ContactError.Text = "";
            }

            // Проверка телефона (необязательно)
            if (!string.IsNullOrEmpty(PhoneBox.Text) && PhoneBox.Text.Length > 20)
            {
                PhoneError.Text = "Максимум 20 символов!";
                isValid = false;
            }
            else
            {
                PhoneError.Text = "";
            }

            // Проверка email (необязательно)
            if (!string.IsNullOrEmpty(EmailBox.Text))
            {
                if (EmailBox.Text.Length > 100)
                {
                    EmailError.Text = "Максимум 100 символов!";
                    isValid = false;
                }
                else if (!Regex.IsMatch(EmailBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    EmailError.Text = "Некорректный формат email!";
                    isValid = false;
                }
                else
                {
                    EmailError.Text = "";
                }
            }
            else
            {
                EmailError.Text = "";
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

            Supplier.name = NameBox.Text.Trim();
            Supplier.contact_person = string.IsNullOrWhiteSpace(ContactBox.Text) ? null : ContactBox.Text.Trim();
            Supplier.phone = string.IsNullOrWhiteSpace(PhoneBox.Text) ? null : PhoneBox.Text.Trim();
            Supplier.email = string.IsNullOrWhiteSpace(EmailBox.Text) ? null : EmailBox.Text.Trim();

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