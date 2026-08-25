using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using FuelLogger.Core.Models;
using Newtonsoft.Json;

namespace FuelLogger.Desktop.Views
{
    public partial class ProjectSelectorWindow : Window
    {
        private List<Project> _recentProjects = new List<Project>();
        private Project _selectedProject;

        public Project SelectedProject { get; private set; }

        public ProjectSelectorWindow()
        {
            InitializeComponent();
            LoadRecentProjects();
        }

        private void LoadRecentProjects()
        {
            var projectsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FuelLogger", "recent.json");
            if (File.Exists(projectsFile))
            {
                try
                {
                    var json = File.ReadAllText(projectsFile);
                    _recentProjects = JsonConvert.DeserializeObject<List<Project>>(json) ?? new List<Project>();
                }
                catch { _recentProjects = new List<Project>(); }
            }

            RecentProjectsList.ItemsSource = _recentProjects;
            StatusTextBlock.Text = _recentProjects.Any() ? $"Доступно {_recentProjects.Count} проектов" : "Нет сохранённых проектов";
        }

        private void SaveRecentProjects()
        {
            var projectsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FuelLogger", "recent.json");
            Directory.CreateDirectory(Path.GetDirectoryName(projectsFile) ?? string.Empty);
            File.WriteAllText(projectsFile, JsonConvert.SerializeObject(_recentProjects));
        }

        private void AddToRecent(Project project)
        {
            _recentProjects.RemoveAll(p => p.FolderPath == project.FolderPath);
            _recentProjects.Insert(0, project);
            if (_recentProjects.Count > 10)
                _recentProjects = _recentProjects.Take(10).ToList();
            SaveRecentProjects();
            RecentProjectsList.ItemsSource = null;
            RecentProjectsList.ItemsSource = _recentProjects;
        }

        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreateProjectDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    StatusTextBlock.Text = "Создание проекта...";
                    var projectFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FuelLoggerProjects", dialog.ProjectName);
                    Directory.CreateDirectory(projectFolder);

                    var project = new Project
                    {
                        Name = dialog.ProjectName,
                        Author = dialog.Author ?? "Неизвестный",
                        FolderPath = projectFolder,
                        DensityGasoline = 0.75,
                        DensityDiesel = 0.85,
                        CreatedAt = DateTime.Now
                    };

                    var projectFile = Path.Combine(projectFolder, "project.json");
                    File.WriteAllText(projectFile, JsonConvert.SerializeObject(project));

                    var dbPath = Path.Combine(projectFolder, "fuel.db");
                    File.WriteAllBytes(dbPath, new byte[0]);

                    AddToRecent(project);
                    SelectedProject = project;
                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка создания проекта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    StatusTextBlock.Text = "Ошибка создания";
                }
            }
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFolderDialog();
                dialog.Title = "Выберите папку проекта Fuel Logger";

                if (dialog.ShowDialog() == true)
                {
                    var projectFile = Path.Combine(dialog.FolderName, "project.json");
                    if (!File.Exists(projectFile))
                    {
                        MessageBox.Show("В выбранной папке нет файла проекта (project.json)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var json = File.ReadAllText(projectFile);
                    var project = JsonConvert.DeserializeObject<Project>(json);
                    if (project != null)
                    {
                        project.FolderPath = dialog.FolderName;
                        AddToRecent(project);
                        SelectedProject = project;
                        DialogResult = true;
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия проекта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RecentProjectsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectedProject = RecentProjectsList.SelectedItem as Project;
            OpenSelectedButton.IsEnabled = _selectedProject != null;
        }

        private void OpenSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProject != null)
            {
                if (!Directory.Exists(_selectedProject.FolderPath) || !File.Exists(Path.Combine(_selectedProject.FolderPath, "fuel.db")))
                {
                    MessageBox.Show("Проект больше не существует. Он будет удален из списка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _recentProjects.Remove(_selectedProject);
                    SaveRecentProjects();
                    LoadRecentProjects();
                    return;
                }

                SelectedProject = _selectedProject;
                DialogResult = true;
                Close();
            }
        }
    }
}