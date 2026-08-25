using System;
using System.IO;
using System.Windows;
using FuelLogger.Desktop.ViewModels;

namespace FuelLogger.Desktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в конструкторе: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        public void InitializeProject(string projectPath)
        {
            try
            {
                var viewModel = DataContext as MainWindowViewModel;
                if (viewModel == null)
                {
                    MessageBox.Show("DataContext is null!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                viewModel.InitializeProject(projectPath);
                Title = $"Fuel Logger - {projectPath}";
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}\n\n{ex.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
    }
}