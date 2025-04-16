using System.Collections.ObjectModel;
using CalculatorMAUI.Entities;
using CalculatorMAUI.Services;

namespace CalculatorMAUI;

public partial class SQLiteDemo : ContentPage
{
    private IDbService _dbService;

    public SQLiteDemo(IDbService dbService)
	{
		InitializeComponent();
        _dbService = dbService;
        InitializeDatabase();
	}

    private async void InitializeDatabase()
    {
        try
        {
            await _dbService.InitAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось инициализировать базу данных: {ex.Message}", "OK");
        }
    }

    private async void OnProjectSelected(object sender, EventArgs e)
    {
        if (projectsPicker.SelectedItem is Project selectedProject)
        {
            try
            {
                var tasks = await _dbService.GetProjectTasksAsync(selectedProject.Id);

                tasksCollection.ItemsSource = new ObservableCollection<Entities.Task>(tasks);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось загрузить задачи: {ex.Message}", "OK");
            }
        }
    }

    private async void SQLiteDemo_Loaded(object sender, EventArgs e)
    {
        try
        {
            var projects = await _dbService.GetAllProjectsAsync();
            projectsPicker.ItemsSource = new ObservableCollection<Project>(projects);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось загрузить проекты: {ex.Message}", "OK");
        }
    }
}