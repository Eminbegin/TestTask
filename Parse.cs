namespace Taskii;

using Newtonsoft.Json;

public class Parser
{
    private static Logger log = Logger.GetInstnce();
    private ListOfTasks allTasks = new ListOfTasks();
    private string path;

    public void Parse(string input)
    {
        int id, id1;
        switch (input)
        {
            case "/help":
                log.HelpMessage();
                break;
            case "/help-t":
                log.HelpTM();
                break;
            case "/help-s":
                log.HelpSM();
                break;
            case "/help-g":
                log.HelpGM();
                break;
            case "/create-task":
                Task temp = new Task();
                log.EnterSome("disc");
                temp.Description = Console.ReadLine() ?? throw new InvalidDataException();
                log.EnterSome("date");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Environment.Exit(0);
                }

                if (input != "-")
                {
                    string[] numsStrings = input.Split(", ");
                    temp.Dl = new DateTime(Convert.ToInt32(numsStrings[2]), Convert.ToInt32(numsStrings[1]),
                        Convert.ToInt32(numsStrings[0]));
                }

                allTasks.AddTask(temp);
                break;
            case "/all-tasks":
                allTasks.WriteAll();
                break;
            case "/one-task":
                log.EnterSome("task");
                id = int.Parse(Console.ReadLine() ?? throw new InvalidDataException());
                if (allTasks.TaskContainsIn(id))
                {
                    allTasks.WriteOne(id);
                }

                break;
            case "/delete-task":
                log.EnterSome("task");
                id = int.Parse(Console.ReadLine() ?? throw new InvalidDataException());
                if (allTasks.TaskContainsIn(id))
                {
                    allTasks.DeleteOne(id);
                }

                break;
            case "/delete-all":
                log.Deletelogs("repeat");
                if (Console.ReadLine() == "/delete-all")
                {
                    allTasks.DeleteAll();
                    log.Deletelogs("suc");
                }
                else
                {
                    log.Deletelogs("cancel");
                }

                break;
            case "/perform-task":
                log.EnterSome("task");
                id = int.Parse(Console.ReadLine() ?? throw new InvalidDataException());
                if (allTasks.TaskContainsIn(id))
                {
                    allTasks.PerformTask(id);
                }

                break;
            case "/create-subtask":
                SubTask subTemp = new SubTask();
                log.EnterSome("task");
                id = int.Parse(Console.ReadLine() ?? throw new InvalidDataException());
                if (allTasks.TaskContainsIn(id))
                {
                    subTemp.ParentId = id;
                    log.EnterSome("disc");
                    subTemp.Description = Console.ReadLine() ?? throw new InvalidDataException();
                    allTasks.AddSubTask(subTemp);
                }

                break;
            case "/perform-subtask":
                log.EnterSome("subtask");
                id = int.Parse(Console.ReadLine() ?? throw new InvalidDataException());
                if (allTasks.SubTaskContainsIn(id))
                {
                }

                allTasks.PerformSubTask(id);
                break;
            case "/delete-subtask":
                log.EnterSome("subtask");
                id = int.Parse(Console.ReadLine() ?? throw new InvalidDataException());
                if (allTasks.SubTaskContainsIn(id))
                {
                    allTasks.DeleteSOne(id);
                    log.SuccessfullyRemoving(id, "subtask");
                }

                break;
            case "/complete-tasks":
                allTasks.WriteAllComplete();
                break;
            case "/create-group":
                Group tempGroup = new Group();
                log.EnterSome("name");
                tempGroup.Name = Console.ReadLine() ?? throw new InvalidDataException();
                allTasks.AddGroup(tempGroup);
                break;
            case "/delete-group":
                log.EnterSome("group");
                id = int.Parse(Console.ReadLine() ?? throw new InvalidDataException());
                ;
                if (allTasks.GroupContainsIn(id))
                {
                    allTasks.DeleteGroup(id);
                }

                break;
            case "/add-to":
                log.EnterSome("group");
                id = int.Parse(Console.ReadLine() ?? throw new InvalidDataException());
                ;
                if (allTasks.GroupContainsIn(id))
                {
                    log.EnterSome("task");
                    id1 = int.Parse(Console.ReadLine() ?? throw new InvalidDataException());
                    ;
                    if (allTasks.TaskContainsIn(id1))
                    {
                        allTasks.TaskToGroup(id, id1);
                    }
                }

                break;
            case "/delete-from":
                log.EnterSome("group");
                id = int.Parse(Console.ReadLine() ?? throw new InvalidDataException());
                ;
                if (allTasks.GroupContainsIn(id))
                {
                    log.EnterSome("task");
                    id1 = int.Parse(Console.ReadLine() ?? throw new InvalidDataException());
                    ;
                    if (allTasks.TaskContainsIn(id1))
                    {
                        allTasks.TaskFromGroup(id, id1);
                    }
                }

                break;
            case "/show-group":
                log.EnterSome("group");
                id = int.Parse(Console.ReadLine() ?? throw new InvalidDataException());
                ;
                if (allTasks.GroupContainsIn(id))
                {
                    allTasks.ShowGroup(id);
                }

                break;
            case "/show-groups":
                allTasks.ShowGroups();
                break;
            case "/today":
                allTasks.TodayTasks();
                break;
            case "/set-dl":
                log.EnterSome("task");
                id = int.Parse(Console.ReadLine() ?? throw new InvalidDataException());
                ;
                if (allTasks.TaskContainsIn(id))
                {
                    log.EnterSome("deate1");
                    string[] numsStrings = (Console.ReadLine() ?? throw new InvalidDataException()).Split(", ") ??
                                           throw new InvalidDataException();
                    allTasks.SetDl(id, numsStrings);
                }

                break;
            default:
                log.UnknownCommand();
                break;
        }
    }

    public void SaveToFile()
    {
        File.WriteAllText(path, allTasks.GetJson());
        log.SuccessSave(path);
    }

    public void SaveFromFile()
    {
        log.AvailableTasks();
        string answer = Console.ReadLine();
        log.EnterSome("path");
        path = Console.ReadLine();
        if (string.IsNullOrEmpty(path))
        {
            Environment.Exit(0);
        }

        if (answer == "Y")
        {
            if (!File.Exists(path))
            {
                throw new Exception("Файл не найден");
            }

            string tempJson = File.ReadAllText(path);
            // path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
            allTasks = JsonConvert.DeserializeObject<ListOfTasks>(tempJson);
        }
        else if (answer == "N")
        {
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("ЧЕЛ ХАРОШ");
            Environment.Exit(0);
        }
    }
}