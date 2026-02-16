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
            Trace.WriteLine($"[{DateTime.Now:HH:mm}] [WARNING] Попытка добавления задачи с пустым названием");
            Console.WriteLine("[WARNING] Название задачи не может быть пустым");
            return;
        }

        Trace.WriteLine($"[{DateTime.Now:HH:mm}] [TRACE] Начало добавления задачи: {title}");

        var newTask = new TaskItem(title);
        tasks.Add(newTask);

        Console.WriteLine($"[INFO] Задача '{title}' добавлена");
        Trace.WriteLine($"[{DateTime.Now:HH:mm}] [INFORMATION] Задача '{title}' успешно добавлена");
    }

    public static void RemoveTask(string title)
    {
        Trace.WriteLine($"[{DateTime.Now:HH:mm}] [TRACE] Начало удаления задачи: {title}");

        var taskToRemove = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (taskToRemove == null)
        {
            Trace.WriteLine($"[{DateTime.Now:HH:mm}] [ERROR] Задача '{title}' не найдена для удаления");
            Console.WriteLine($"[INFO] Задача '{title}' не найдена");
            return;
        }

        tasks.Remove(taskToRemove);
        Console.WriteLine($"[INFO] Задача '{title}' удалена");
        Trace.WriteLine($"[{DateTime.Now:HH:mm}] [INFORMATION] Задача '{title}' успешно удалена");
    }

    public static void ListTasks()
    {
        Trace.WriteLine($"[{DateTime.Now:HH:mm}] [TRACE] Начало вывода списка задач");

        if (tasks.Count == 0)
        {
            Console.WriteLine("[INFO] Задачи отсутствуют");
            Trace.WriteLine($"[{DateTime.Now:HH:mm}] [INFORMATION] Список задач пуст");
            return;
        }

        Console.WriteLine("_____________________________");
        Console.WriteLine($"[INFO] Список задач (всего: {tasks.Count}):");

        foreach (var task in tasks)
        {
            Console.WriteLine($" - {task.Title}");
        }

        Console.WriteLine("_____________________________");
        Trace.WriteLine($"[{DateTime.Now:HH:mm}] [INFORMATION] Выведено {tasks.Count} задач");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("[INFO] Приложение запущено");
        Trace.WriteLine($"[{DateTime.Now:HH:mm}] [TRACE] Приложение запущено");

        bool work = true;
        bool istrace = false;
        while (work)
        {
            if (istrace == true)
            {
                Trace.Listeners.Add(new ConsoleTraceListener());
            }
            ShowMenu();
            Trace.WriteLine($"[{DateTime.Now:HH:mm}] [TRACE] Ожидание команды пользователя");

            string ans = Console.ReadLine();
            Trace.WriteLine($"[{DateTime.Now:HH:mm}] [TRACE] Получена команда {ans}");

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
                    work = false;
                    Console.WriteLine("[INFO] Закрытие программы");
                    Trace.WriteLine($"[{DateTime.Now:HH:mm}] [TRACE] Пользователь закрыл программу");
                    break;
                case "toggle":
                    istrace = true;
                    Console.WriteLine("[INFO] Включение трассировки");
                    break;

                default:
                    Console.WriteLine("[INFO] Неизвестная команда");
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
        Console.WriteLine("toggle - включить трассировку");
        Console.WriteLine("exit - закрыть программу");
        Console.WriteLine("___________________________");
    }

    static void HandleAddCommand()
    {
        Console.WriteLine("Введите название задачи:");
        Trace.WriteLine($"[{DateTime.Now:HH:mm}] [TRACE] Ожидание ввода названия задачи");

        string title = Console.ReadLine();
        Trace.WriteLine($"[{DateTime.Now:HH:mm}] [TRACE] Получено название задачи: {title}");

        TaskManager.AddTask(title);
    }

    static void HandleRemoveCommand()
    {
        Console.WriteLine("Введите название задачи для удаления:");
        Trace.WriteLine($"[{DateTime.Now:HH:mm}] [TRACE] Ожидание ввода названия задачи для удаления");

        string title = Console.ReadLine();
        Trace.WriteLine($"[{DateTime.Now:HH:mm}] [TRACE] Получено название задачи для удаления: {title}");

        TaskManager.RemoveTask(title);
    }
}