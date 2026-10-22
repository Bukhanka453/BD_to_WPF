using System.Windows;

namespace WpfWarehouse
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                using (var db = new Services.DatabaseService())
                {
                    var test = db.GetCategories();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    "Ошибка подключения к базе данных!\n\n" +
                    $"Сообщение: {ex.Message}\n\n" +
                    "Проверьте:\n" +
                    "1. Запущен ли SQL Server\n" +
                    "2. Правильная ли строка подключения в App.config\n" +
                    "3. Существует ли база данных",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Current.Shutdown();
            }
        }
    }
}