namespace Taskii;

public class Logger
{
    private static Logger _instance = null;

    private Logger()
    {
    }

    public static Logger GetInstnce()
    {
        if (_instance == null)
        {
            _instance = new Logger();
        }

        return _instance;
    }

    public void TaskNoExist(int id)
    {
        Console.WriteLine("не существует задачи с id = {0}", id);
    }

    public void SubTaskNoExist(int id)
    {
        Console.WriteLine("не существует подзадачи с id = {0}", id);
    }

    public void GroupNoExist(int id)
    {
        Console.WriteLine("не существует группы с id = {0}", id);
    }

    public void EnterSome(string a)
    {
        switch (a)
        {
            case "disc":
                Console.WriteLine("Введите описание");
                break;
            case "date":
                Console.WriteLine("Введите дату дедлайна (число, месяц, год); если его нет, введите \"-\"");
                break;
            case "date1":
                Console.WriteLine("Введите дату дедлайна (число, месяц, год)");
                break;
            case "task":
                Console.WriteLine("Введите id задачи");
                break;
            case "subtask":
                Console.WriteLine("Введите id подзадачи");
                break;
            case "name":
                Console.WriteLine("Введите название");
                break;
            case "group":
                Console.WriteLine("Введите id группы");
                break;
            case "path":
                Console.WriteLine("Введите путь к файлу, например {0}",
                    @"C:\Users\emink\Desktop\FFirsTT\Taskii\Taskii\bin\Debug\net6.0\mypath.json");
                break;
        }
    }


    public void SuccessfullyAdding(int id, string a)
    {
        if (a == "task")
            Console.WriteLine("Задача создана, task-id = {0}", id);
        if (a == "subtask")
            Console.WriteLine("Подадача создана, task-id = {0}", id);
        if (a == "group")
            Console.WriteLine("Группа создана, task-id = {0}", id);
    }

    public void SuccessfullyRemoving(int id, string a)
    {
        switch (a)
        {
            case "task":
                Console.WriteLine("Задача с id = {0} удалена", id);
                break;
            case "subtask":
                Console.WriteLine("Подзадача с id = {0} удалена", id);
                break;
            case "group":
                Console.WriteLine("Группа с id = {0} удалена", id);
                break;
        }
    }

    public void PrintTask(Task t1)
    {
        Console.WriteLine("[{0}] {1}task-id = {2}, {3}", t1.Complete ? "x" : " ",
            t1.Dl == DateTime.MaxValue ? "" : t1.Dl.ToShortDateString() + " ", t1.Id,
            t1.Description);
    }

    public void PrintSubtask(SubTask st1)
    {
        Console.WriteLine("  - [{0}] subtask-id = {1}, {2}", st1.Complete ? "x" : " ", st1.Id,
            st1.Description);
    }

    public void EmptyTaskList()
    {
        Console.WriteLine("Список задач пуст");
    }

    public void Deletelogs(string a)
    {
        switch (a)
        {
            case "repeat":
                Console.WriteLine("Повторите команду для удаления всех задач");
                break;
            case "suc":
                Console.WriteLine("Все задачи были удалены");
                break;
            case "cancel":
                Console.WriteLine("Удаление отменено");
                break;
        }
    }

    public void AlredyComp(string a)
    {
        switch (a)
        {
            case "task":
                Console.WriteLine("Задача уже выполнена");
                break;
            case "subtask":
                Console.WriteLine("Подзадача уже выполнена");
                break;
        }
    }

    public void NowComp(int id, string a)
    {
        switch (a)
        {
            case "task":
                Console.WriteLine("Задача с id = {0} теперь выполена", id);
                break;
            case "subtask":
                Console.WriteLine("Подзадача с id = {0} теперь выполена", id);
                break;
        }
    }

    public void FromToGroup(int TId, int GId, int a)
    {
        switch (a)
        {
            case 1:
                Console.WriteLine("Задача с task-id = {0} уже содержится в группе с group-id = {1}", TId, GId);
                break;
            case 2:
                Console.WriteLine("Задача с task-id = {0} теперь содержится в группе с group-id = {1}", TId, GId);
                break;
            case 3:
                Console.WriteLine("Задача с task-id = {0} и так не содержится в группе с group-id = {1}", TId, GId);
                break;
            case 4:
                Console.WriteLine("Задача с task-id = {0} теперь не содержится в группе с group-id = {1}", TId, GId);
                break;
        }
    }

    public void PrintGroup(Group g1)
    {
        Console.WriteLine("group-id = {0}, {1}", g1.Id, g1.Name);
    }

    public void TodayLogs(string a, int counter)
    {
        switch (a)
        {
            case "today":
                Console.WriteLine("Задачи на сегодня:");
                break;
            case "no":
                Console.WriteLine("Задач на сегодня нет");
                break;
            case "count":
                Console.WriteLine("Количесвто сегодняшних задач равно {0}", counter);
                break;
        }
    }

    public void WriteSpecial(int a)
    {
        switch (a)
        {
            case 0:
                Console.Write(" *");
                break;
            default:
                Console.Write("   {0})", a);
                break;
        }
    }

    public void SuccessSave(string path)
    {
        Console.WriteLine("Все задачи сохранены в файл {0}", path);
    }

    public void AvailableTasks()
    {
        Console.WriteLine("Есть ли сохранённые задачи? Y/N?");
    }

    public void HelpMessage()
    {
        Console.WriteLine("/help-t \t\t-\t cписок команд для задач");
        Console.WriteLine("/help-s \t\t-\t cписок команд для подзадач");
        Console.WriteLine("/help-g \t\t-\t cписок команд для групп");
        Console.WriteLine("/exit \t\t\t-\t выход");
    }

    public void HelpTM()
    {
        Console.WriteLine("/create-task \t\t-\t создаёт новой задачи");
        Console.WriteLine("/delete-task \t\t-\t удаляет задачу по id");
        Console.WriteLine("/perform-task \t\t-\t отмечает задачу по id");
        Console.WriteLine("/set-dl \t\t-\t устанвливает дедлайн на задачу");
        Console.WriteLine("/today \t\t\t-\t выводит список задач, с дедлайном сегодня");
        Console.WriteLine("/one-task \t\t-\t выводит задачу по id");
        Console.WriteLine("/all-tasks \t\t-\t выводит все задачи");
        Console.WriteLine("/complete-tasks \t-\t выводит все задачи");
        Console.WriteLine("/delete-all \t\t-\t удаляет все задачи");
    }

    public void HelpSM()
    {
        Console.WriteLine("/create-subtask \t-\t создаёт новой подзадачи");
        Console.WriteLine("/delete-subtask \t-\t удаляет подзадачу по id");
        Console.WriteLine("/perform-subtask \t-\t отмечает подзадачу по id");
    }

    public void HelpGM()
    {
        Console.WriteLine("/create-group \t\t-\t создаёт группу");
        Console.WriteLine("/delete-group \t\t-\t удаляет группу");
        Console.WriteLine("/add-to \t\t-\t добавляет задачу в группу");
        Console.WriteLine("/delete-from \t\t-\t удаляет задачу из группы");
        Console.WriteLine("/show-group \t\t-\t выводит группу");
    }

    public void UnknownCommand()
    {
        Console.WriteLine("Неизвестная команда");
    }

    public void FirstHelp()
    {
        Console.WriteLine("Используйте /help, чтобы увидеть список команд");
    }
}