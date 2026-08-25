using FuelLogger.Core.Models;
using FuelLogger.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace FuelLogger.Desktop.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        // --- Дополнительные свойства для привязок ---
        private string _vehicleStatusText = "Готово";
        public string VehicleStatusText
        {
            get => _vehicleStatusText;
            set { _vehicleStatusText = value; OnPropertyChanged(); }
        }

        private TemplateVehicle _selectedVehicle;
        public TemplateVehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set { _selectedVehicle = value; OnPropertyChanged(); }
        }

        private TaskType _selectedTaskType;
        public TaskType SelectedTaskType
        {
            get => _selectedTaskType;
            set { _selectedTaskType = value; OnPropertyChanged(); }
        }

        private string _taskStatusText = "Готово";
        public string TaskStatusText
        {
            get => _taskStatusText;
            set { _taskStatusText = value; OnPropertyChanged(); }
        }

        private string _reportStatusText = "Готово";
        public string ReportStatusText
        {
            get => _reportStatusText;
            set { _reportStatusText = value; OnPropertyChanged(); }
        }

        private Report _selectedReport;
        public Report SelectedReport
        {
            get => _selectedReport;
            set { _selectedReport = value; OnPropertyChanged(); }
        }

        // --- Команда для добавления подразделения (заглушка) ---
        public ICommand AddDepartmentCommand { get; }

        private void AddDepartment()
        {
            MessageBox.Show("Функция добавления подразделения будет реализована позже");
        }
        private AppDbContext _context;
        private string _dbPath;

        private ObservableCollection<Department> _departments = new ObservableCollection<Department>();
        public ObservableCollection<Department> Departments
        {
            get => _departments;
            set { _departments = value; OnPropertyChanged(); }
        }

        private ObservableCollection<FleetItem> _fleetItems = new ObservableCollection<FleetItem>();
        public ObservableCollection<FleetItem> FleetItems
        {
            get => _fleetItems;
            set { _fleetItems = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TemplateVehicle> _vehicles = new ObservableCollection<TemplateVehicle>();
        public ObservableCollection<TemplateVehicle> Vehicles
        {
            get => _vehicles;
            set { _vehicles = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TaskType> _taskTypes = new ObservableCollection<TaskType>();
        public ObservableCollection<TaskType> TaskTypes
        {
            get => _taskTypes;
            set { _taskTypes = value; OnPropertyChanged(); }
        }

        private ObservableCollection<FuelTask> _tasks = new ObservableCollection<FuelTask>();
        public ObservableCollection<FuelTask> Tasks
        {
            get => _tasks;
            set { _tasks = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Report> _reports = new ObservableCollection<Report>();
        public ObservableCollection<Report> Reports
        {
            get => _reports;
            set { _reports = value; OnPropertyChanged(); }
        }

        private string _statusText = "Готово";
        public string StatusText
        {
            get => _statusText;
            set { _statusText = value; OnPropertyChanged(); }
        }

        private string _statisticsText = "Нет данных";
        public string StatisticsText
        {
            get => _statisticsText;
            set { _statisticsText = value; OnPropertyChanged(); }
        }

        public ICommand LoadDataCommand { get; }
        public ICommand UpdateFleetCommand { get; }
        public ICommand LoadVehiclesCommand { get; }
        public ICommand AddVehicleCommand { get; }
        public ICommand EditVehicleCommand { get; }
        public ICommand DeleteVehicleCommand { get; }
        public ICommand LoadTasksCommand { get; }
        public ICommand CreateTaskCommand { get; }
        public ICommand CalculateTasksCommand { get; }
        public ICommand DeleteTasksCommand { get; }
        public ICommand LoadReportsCommand { get; }
        public ICommand OpenReportCommand { get; }
        public ICommand DeleteReportCommand { get; }
        public ICommand CreateProjectCommand { get; }
        public ICommand OpenProjectCommand { get; }
        public ICommand ExitCommand { get; }

        public MainWindowViewModel()
        {
            LoadDataCommand = new RelayCommand(() => LoadData());
            UpdateFleetCommand = new RelayCommand(() => UpdateFleet());
            LoadVehiclesCommand = new RelayCommand(() => LoadVehicles());
            AddVehicleCommand = new RelayCommand(() => AddVehicle());
            EditVehicleCommand = new RelayCommand(() => EditVehicle());
            DeleteVehicleCommand = new RelayCommand(() => DeleteVehicle());
            LoadTasksCommand = new RelayCommand(() => LoadTasks());
            CreateTaskCommand = new RelayCommand(() => CreateTask());
            CalculateTasksCommand = new RelayCommand(() => CalculateTasks());
            DeleteTasksCommand = new RelayCommand(() => DeleteTasks());
            LoadReportsCommand = new RelayCommand(() => LoadReports());
            OpenReportCommand = new RelayCommand(() => OpenReport());
            DeleteReportCommand = new RelayCommand(() => DeleteReport());
            CreateProjectCommand = new RelayCommand(() => CreateProject());
            OpenProjectCommand = new RelayCommand(() => OpenProject());
            ExitCommand = new RelayCommand(() => System.Windows.Application.Current.Shutdown());
            AddDepartmentCommand = new RelayCommand(() => AddDepartment());
        }

        public void InitializeProject(string projectPath)
        {
            try
            {
                _dbPath = System.IO.Path.Combine(projectPath, "fuel.db");
                _context = new AppDbContext(_dbPath);
                _context.Database.EnsureCreated();
                DatabaseInitializer.Initialize(_context);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в InitializeProject: {ex.Message}\n\n{ex.StackTrace}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private void LoadData()
        {
            try
            {
                StatusText = "Загрузка данных...";
                var depts = _context.Departments.ToList();
                Departments.Clear();
                var tree = BuildTree(depts);
                foreach (var item in tree)
                    Departments.Add(item);
                UpdateFleetStatistics();
                StatusText = $"Загружено {Departments.Count} подразделений";
            }
            catch (Exception ex)
            {
                StatusText = $"Ошибка: {ex.Message}";
            }
        }

        private List<Department> BuildTree(List<Department> flatList)
        {
            var lookup = flatList.ToLookup(d => d.ParentId);
            foreach (var dept in flatList)
            {
                dept.Children = lookup[dept.Id].ToList();
            }
            return lookup[null].ToList();
        }

        private void UpdateFleet()
        {
            try
            {
                var selectedIds = GetAllCheckedDepartments();
                if (selectedIds.Count == 0)
                {
                    FleetItems.Clear();
                    StatisticsText = "Выберите подразделения";
                    return;
                }

                var fleet = _context.FleetItems
                    .Where(f => selectedIds.Contains(f.DepartmentId))
                    .ToList();

                FleetItems.Clear();
                foreach (var item in fleet)
                    FleetItems.Add(item);

                UpdateFleetStatistics();
                StatusText = $"Загружено {FleetItems.Count} записей";
            }
            catch (Exception ex)
            {
                StatusText = $"Ошибка: {ex.Message}";
            }
        }

        private List<int> GetAllCheckedDepartments()
        {
            var result = new List<int>();
            foreach (var d in Departments)
            {
                if (d.Level == 1 && d.IsChecked)
                    result.Add(d.Id);
                if (d.Children != null)
                    result.AddRange(GetAllCheckedDepartments(d.Children));
            }
            return result;
        }

        private List<int> GetAllCheckedDepartments(List<Department> depts)
        {
            var result = new List<int>();
            foreach (var d in depts)
            {
                if (d.Level == 1 && d.IsChecked)
                    result.Add(d.Id);
                if (d.Children != null)
                    result.AddRange(GetAllCheckedDepartments(d.Children));
            }
            return result;
        }

        private void UpdateFleetStatistics()
        {
            if (FleetItems.Count == 0)
            {
                StatisticsText = "Нет данных";
                return;
            }

            int total = FleetItems.Sum(f => f.Quantity);
            int ab = FleetItems.Where(f => f.Vehicle != null && f.Vehicle.FuelType == "AB").Sum(f => f.Quantity);
            int dt = FleetItems.Where(f => f.Vehicle != null && f.Vehicle.FuelType == "DT").Sum(f => f.Quantity);
            double volume = FleetItems.Sum(f => f.TotalTankVolume);

            StatisticsText = $"Всего ТС: {total}  |  АБ: {ab}  |  ДТ: {dt}\nОбщий объём: {volume:F1} л";
        }

        // --- Заглушки (TODO) ---
        private void LoadVehicles() { }
        private void AddVehicle() { }
        private void EditVehicle() { }
        private void DeleteVehicle() { }
        private void LoadTasks() { }
        private void CreateTask() { }
        private void CalculateTasks() { }
        private void DeleteTasks() { }
        private void LoadReports() { }
        private void OpenReport() { }
        private void DeleteReport() { }
        private void CreateProject() { }
        private void OpenProject() { }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}