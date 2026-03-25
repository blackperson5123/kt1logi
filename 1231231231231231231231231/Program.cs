using Serilog;
using Serilog.Events;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class TaskItem
{
    public string Title { get; set; }

    public TaskItem(string title)
    {
        Title = title;
    }
}

public static class TaskManager
{
    private static List<TaskItem> tasks = new List<TaskItem>();

    public static void AddTask(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            Log.Warning("Попытка добавления задачи с пустым названием");
            Log.Error("Название задачи не может быть пустым");
            return;
        }

        Log.Debug($"Начало добавления задачи: {title}");

        var newTask = new TaskItem(title);
        tasks.Add(newTask);

        Log.Debug($"Задача '{title}' добавлена");
        Log.Information($" Задача '{title}' успешно добавлена");
    }

    public static void RemoveTask(string title)
    {
        Log.Debug($"Начало удаления задачи: {title}");

        var taskToRemove = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (taskToRemove == null)
        {
            Log.Warning($"Задача '{title}' не найдена для удаления");
            Log.Error($"адача '{title}' не найдена");
            return;
        }

        tasks.Remove(taskToRemove);
        Log.Debug($"Задача '{title}' удалена");
        Log.Information($"Задача '{title}' успешно удалена");
    }

    public static void ListTasks()
    {
        Log.Debug("Начало вывода списка задач");

        if (tasks.Count == 0)
        {
            Log.Debug("Задачи отсутствуют");
            Log.Information("Список задач пуст");
            return;
        }

        Console.WriteLine("_____________________________");
        Log.Information($"писок задач (всего: {tasks.Count}):");

        foreach (var task in tasks)
        {
            Console.WriteLine($" - {task.Title}");
        }

        Console.WriteLine("_____________________________");
        Log.Information($" {tasks.Count} задач");
    }
}

class Program
{
    static void Main()
    {
        //File.Delete("log/debug.log");
        //File.Delete("log/error.log");
        //File.Delete("log/info.log");
        //File.Delete("log/warn.log");
        var date1 = DateTime.Now;
        string date = date1.ToString().Replace(':', '.');

        Directory.CreateDirectory($"{date}");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                .WriteTo.File($"{date}/ debug.log",
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                .WriteTo.File($"{date}/info.log",
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                .WriteTo.File($"{date}/warn.log",
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                .WriteTo.File($"{date}/error.log",
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
            .CreateLogger();

        var time = DateTime.Now;
        Log.Information($"{time}: Запуск");
        Log.Warning($"{time}: Запуск");
        Log.Debug($"{time}: Запуск");
        Log.Error($"{time}: Запуск");

        bool work = true;
        while (work)
        {
            ShowMenu();
            string ans = Console.ReadLine();
            Log.Debug("Получена команда {ans}");

            switch (ans?.ToLower())
            {
                case "add":
                    HandleAddCommand();
                    break;

                case "remove":
                    HandleRemoveCommand();
                    break;

                case "list":
                    TaskManager.ListTasks();
                    break;

                case "exit":

                    Log.Information("Закрытие программы");
                    Log.Debug("Пользователь закрыл программу");

                    Log.CloseAndFlush();

                    Console.WriteLine("Нажмите любую клавишу для выхода...");
                    Console.ReadKey();
                    work = false;
                    break;


                default:
                    Log.Error("Неизвестная команда");
                    break;


            }
            
        }
    }





    static void ShowMenu()
    {
        Console.WriteLine("___________________________");
        Console.WriteLine("add - добавление задачи");
        Console.WriteLine("remove - удаление задачи");
        Console.WriteLine("list - вывод списка задач");
        Console.WriteLine("exit - закрыть программу");
        Console.WriteLine("___________________________");
    }

    static void HandleAddCommand()
    {
        Console.WriteLine("Введите название задачи:");
        Log.Debug("Ожидание ввода названия задачи");

        string title = Console.ReadLine();
        Log.Debug($"Получено название задачи: {title}");

        TaskManager.AddTask(title);
    }

    static void HandleRemoveCommand()
    {
        Console.WriteLine("Введите название задачи для удаления:");
        Log.Debug(" Ожидание ввода названия задачи для удаления");

        string title = Console.ReadLine();
        Log.Debug($"Получено название задачи для удаления: {title}");

        TaskManager.RemoveTask(title);
    }
}
