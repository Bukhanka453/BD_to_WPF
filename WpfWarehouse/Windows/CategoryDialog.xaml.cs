using System;
using System.Windows;

namespace WpfWarehouse.Windows
{
    public partial class CategoryDialog : Window
    {
        public categories Category { get; private set; }

        public CategoryDialog(categories category = null)
        {
            InitializeComponent();

            if (category != null)
            {
                Category = category;
                TitleBox.Text = category.title;
                Title = "Редактирование категории";
            }
            else
            {
                Category = new categories();
                Title = "Добавление категории";
            }
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                ErrorText.Text = "Название обязательно!";
                return false;
            }

            if (TitleBox.Text.Length > 60)
            {
                ErrorText.Text = "Максимум 60 символов!";
                return false;
            }

            ErrorText.Text = "";
            return true;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                MessageBox.Show("Исправьте ошибки!", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Category.title = TitleBox.Text.Trim();
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