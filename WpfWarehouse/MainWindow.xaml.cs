using System.Windows;
using WpfWarehouse.Views;

namespace WpfWarehouse
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Categories_Click(object sender, RoutedEventArgs e)
        {
            var window = new Window
            {
                Title = "Категории",
                Content = new CategoriesView(),
                Width = 800,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            window.Owner = this;
            window.ShowDialog();
        }
        
        private void Suppliers_Click(object sender, RoutedEventArgs e)
        {
            var window = new Window
            {
                Title = "Поставщики",
                Content = new SuppliersView(),
                Width = 900,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            window.Owner = this;
            window.ShowDialog();
        }
        
        private void Products_Click(object sender, RoutedEventArgs e)
        {
            var window = new Window
            {
                Title = "Товары",
                Content = new ProductsView(),
                Width = 1000,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            window.Owner = this;
            window.ShowDialog();
        }
        
        private void Deliveries_Click(object sender, RoutedEventArgs e)
        {
            var window = new Window
            {
                Title = "Поставки",
                Content = new DeliveriesView(),
                Width = 1000,
                Height = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            window.Owner = this;
            window.ShowDialog();
        }
        
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}