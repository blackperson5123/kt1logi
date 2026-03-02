using Serilog;
using System.Diagnostics;

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
        File.Delete("trace.log");
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                
                .WriteTo.File("trace.log",
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        


        Log.Information("Приложение запущено");
        Log.Debug("Приложение запущено");

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
                    
                    Log.Information("закрытие программы");
                    Log.Debug("Pользователь закрыл программу");
                    // 3. Завершаем работу логера
                    Log.CloseAndFlush();
                    Console.WriteLine("Нажмите любую клавишу для выхода...");
                    Console.ReadKey();
                    work = false;
                    string logPath = "trace.log";
                    Trace.Listeners.Add(new TextWriterTraceListener(logPath));
                    Trace.AutoFlush = true;
                    break;


                default:
                    Log.Information("Неизвестная команда");
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



















