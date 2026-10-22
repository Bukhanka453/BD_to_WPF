using System;
using System.Windows;
using System.Windows.Controls;
using WpfWarehouse.Services;
using WpfWarehouse.Windows;

namespace WpfWarehouse.Views
{
    public partial class ProductsView : UserControl
    {
        private DatabaseService _db;
        private products _selected;

        public ProductsView()
        {
            InitializeComponent();
            _db = new DatabaseService();
            LoadData();
        }

        private void LoadData()
        {
            DataGrid.ItemsSource = _db.GetProducts();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = SearchBox.Text.Trim();
            if (string.IsNullOrEmpty(text))
                LoadData();
            else
                DataGrid.ItemsSource = _db.SearchProducts(text);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProductDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _db.AddProduct(dialog.Product);
                    LoadData();
                    MessageBox.Show("Товар добавлен!", "Успех",
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
                MessageBox.Show("Выберите товар!", "Предупреждение",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new ProductDialog(_selected);
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _db.UpdateProduct(dialog.Product);
                    LoadData();
                    MessageBox.Show("Товар обновлен!", "Успех",
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
                MessageBox.Show("Выберите товар!", "Предупреждение",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить товар '{_selected.title}'?\n" +
                                        "Все связанные поставки также будут удалены (каскадное удаление).",
                                        "Подтверждение удаления",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _db.DeleteProduct(_selected.id);
                    LoadData();
                    _selected = null;
                    MessageBox.Show("Товар удален!", "Успех",
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
            _selected = DataGrid.SelectedItem as products;
        }
    }
}