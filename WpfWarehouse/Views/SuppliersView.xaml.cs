using System;
using System.Windows;
using System.Windows.Controls;
using WpfWarehouse.Services;
using WpfWarehouse.Windows;

namespace WpfWarehouse.Views
{
    public partial class SuppliersView : UserControl
    {
        private DatabaseService _db;
        private suppliers _selected;

        public SuppliersView()
        {
            InitializeComponent();
            _db = new DatabaseService();
            LoadData();
        }

        private void LoadData()
        {
            DataGrid.ItemsSource = _db.GetSuppliers();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = SearchBox.Text.Trim();
            if (string.IsNullOrEmpty(text))
                LoadData();
            else
                DataGrid.ItemsSource = _db.SearchSuppliers(text);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SupplierDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _db.AddSupplier(dialog.Supplier);
                    LoadData();
                    MessageBox.Show("Поставщик добавлен!", "Успех",
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
                MessageBox.Show("Выберите поставщика!", "Предупреждение",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new SupplierDialog(_selected);
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _db.UpdateSupplier(dialog.Supplier);
                    LoadData();
                    MessageBox.Show("Поставщик обновлен!", "Успех",
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
                MessageBox.Show("Выберите поставщика!", "Предупреждение",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить поставщика '{_selected.name}'?\n" +
                                        "Все связанные товары также будут удалены (каскадное удаление).",
                                        "Подтверждение удаления",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _db.DeleteSupplier(_selected.id);
                    LoadData();
                    _selected = null;
                    MessageBox.Show("Поставщик удален!", "Успех",
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
            _selected = DataGrid.SelectedItem as suppliers;
        }
    }
}