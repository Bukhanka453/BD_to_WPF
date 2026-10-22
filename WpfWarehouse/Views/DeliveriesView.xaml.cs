using System;
using System.Windows;
using System.Windows.Controls;
using WpfWarehouse.Services;
using WpfWarehouse.Windows;

namespace WpfWarehouse.Views
{

    public partial class DeliveriesView : UserControl
    {
        private DatabaseService _db;
        private deliveries _selected;


        public DeliveriesView()
        {
            InitializeComponent();
            _db = new DatabaseService();
            LoadData();
        }

        private void LoadData()
        {
            DataGrid.ItemsSource = _db.GetDeliveries();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = SearchBox.Text.Trim();
            if (string.IsNullOrEmpty(text))
                LoadData();
            else
                DataGrid.ItemsSource = _db.SearchDeliveries(text);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new DeliveryDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _db.AddDelivery(dialog.Delivery);
                    LoadData();
                    MessageBox.Show("Поставка добавлена! Количество товара обновлено.", "Успех",
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
                MessageBox.Show("Выберите поставку!", "Предупреждение",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Сохраняем старые значения для обновления количества товара
            int oldQuantity = _selected.quantity;
            int oldProductId = _selected.product_id;

            var dialog = new DeliveryDialog(_selected);
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _db.UpdateDelivery(dialog.Delivery, oldQuantity, oldProductId);
                    LoadData();
                    MessageBox.Show("Поставка обновлена! Количество товара скорректировано.", "Успех",
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
                MessageBox.Show("Выберите поставку!", "Предупреждение",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить поставку товара '{_selected.products?.title}'?\n" +
                                        $"Количество товара будет уменьшено на {_selected.quantity}.",
                                        "Подтверждение удаления",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _db.DeleteDelivery(_selected.id, _selected.product_id, _selected.quantity);
                    LoadData();
                    _selected = null;
                    MessageBox.Show("Поставка удалена! Количество товара скорректировано.", "Успех",
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
            _selected = DataGrid.SelectedItem as deliveries;
        }
    }
}