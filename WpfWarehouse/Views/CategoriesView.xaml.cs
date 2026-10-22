using System;
using System.Windows;
using System.Windows.Controls;
using WpfWarehouse.Services;
using WpfWarehouse.Windows;

namespace WpfWarehouse.Views
{
    public partial class CategoriesView : UserControl
    {
        private DatabaseService _db;
        private categories _selected;

        public CategoriesView()
        {
            InitializeComponent();
            _db = new DatabaseService();
            LoadData();
        }

        private void LoadData()
        {
            // Используем DataGrid - ЭТО НЕ СТАТИЧЕСКИЙ МЕТОД
            DataGrid.ItemsSource = _db.GetCategories();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = SearchBox.Text.Trim(); // SearchBox - это поле класса, не статическое
            if (string.IsNullOrEmpty(text))
                LoadData();
            else
                DataGrid.ItemsSource = _db.SearchCategories(text);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CategoryDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _db.AddCategory(dialog.Category);
                    LoadData();
                    MessageBox.Show("Категория добавлена!", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (_selected == null)
            {
                MessageBox.Show("Выберите запись!", "Предупреждение",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new CategoryDialog(_selected);
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _db.UpdateCategory(dialog.Category);
                    LoadData();
                    MessageBox.Show("Категория обновлена!", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (_selected == null)
            {
                MessageBox.Show("Выберите запись!", "Предупреждение",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить '{_selected.title}'?",
                                        "Подтверждение",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _db.DeleteCategory(_selected.id);
                    LoadData();
                    _selected = null;
                    MessageBox.Show("Категория удалена!", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            LoadData();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selected = DataGrid.SelectedItem as categories;
        }
    }
}