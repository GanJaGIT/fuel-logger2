using System.Windows;

namespace FuelLogger.Desktop.Views
{
    public partial class CreateProjectDialog : Window
    {
        public string ProjectName { get; private set; }
        public string Author { get; private set; }

        public CreateProjectDialog()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProjectNameTextBox.Text))
            {
                MessageBox.Show("Введите имя проекта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ProjectName = ProjectNameTextBox.Text.Trim();
            Author = AuthorTextBox.Text?.Trim() ?? "Неизвестный";

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}