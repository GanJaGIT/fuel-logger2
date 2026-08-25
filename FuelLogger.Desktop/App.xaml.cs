using System;
using System.IO;
using System.Windows;
using FuelLogger.Desktop.Views;

namespace FuelLogger.Desktop
{
    public partial class App : Application
    {
        private string _logFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FuelLogger", "app.log");

        private void Log(string message)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_logFile));
                File.AppendAllText(_logFile, $"{DateTime.Now:HH:mm:ss} - {message}\n");
            }
            catch { }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                Log("=== СТАРТ ===");

                var selector = new ProjectSelectorWindow();
                Log("Окно выбора создано");

                var result = selector.ShowDialog();
                Log($"Результат выбора: {result}");

                if (result == true && selector.SelectedProject != null)
                {
                    Log($"Выбран проект: {selector.SelectedProject.Name}, путь: {selector.SelectedProject.FolderPath}");

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.InitializeProject(selector.SelectedProject.FolderPath);
                    mainWindow.Show();

                    // --- ЭТО ГЛАВНОЕ! ---
                    Application.Current.MainWindow = mainWindow;
                    Log("Главное окно создано");

                    mainWindow.InitializeProject(selector.SelectedProject.FolderPath);
                    Log("InitializeProject вызван");

                    mainWindow.Show();
                    Log("Главное окно показано");

                    // Добавляем ожидание, чтобы окно не закрылось
                    System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    Log("Проект не выбран, завершение");
                    Shutdown();
                }
            }
            catch (Exception ex)
            {
                Log($"ОШИБКА: {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show($"Ошибка запуска: {ex.Message}\n\nПодробности в {_logFile}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}