using Newtonsoft.Json;

namespace Taskii;

public class ListOfTasks
{
    public Logger log = Logger.GetInstnce();
    public Dictionary<int, Task> Dict = new Dictionary<int, Task>();
    public Dictionary<int, SubTask> SubDict = new Dictionary<int, SubTask>();
    public Dictionary<int, Group> GroupDict = new Dictionary<int, Group>();

    public string GetFromFile()
    {
      Environment.Exit(0);  log.AvailableTasks();
        string? answer = Console.ReadLine();
        log.EnterSome("path");
        string? path = Console.ReadLine();
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

            string tempJson1 = File.ReadAllText(path);
            // path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
            ListOfTasks temp = JsonConvert.DeserializeObject<ListOfTasks>(tempJson1);
            Dict = temp.Dict;
            GroupDict = temp.GroupDict;
            SubDict = temp.SubDict;
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

        return path;
    }

    public void SaveToFile(string path)
    {
        string tempJson2 = JsonConvert.SerializeObject(this);
        File.WriteAllText(path, tempJson2);
        log.SuccessSave(path);
    }

    private int LastId()
    {
        if (Dict.Any())
        {
            return Dict.Keys.Max();
        }

        return 0;
    }

    private int LastGId()
    {
        if (GroupDict.Any())
        {
            return GroupDict.Keys.Max();
        }

        return 0;
    }

    private int LastSId()
    {
        if (SubDict.Any())
        {
            return SubDict.Keys.Max();
        }

        return 0;
    }

    public void CheckComplete(int id)
    {
        bool checkFullComplete = true;
        foreach (int i in Dict[id].SubTasks)
        {
            if (!SubDict[i].Complete)
            {
                checkFullComplete = false;
            }
        }

        if (checkFullComplete)
        {
            Dict[id].PerformTask();
        }
        else
        {
            Dict[id].UnPerformTask();
        }
    }

    public bool TaskContainsIn(int id)
    {
        if (!Dict.ContainsKey(id))
        {
            log.TaskNoExist(id);
            return false;
        }

        return true;
    }

    public bool SubTaskContainsIn(int id)
    {
        if (!SubDict.ContainsKey(id))
        {
            log.SubTaskNoExist(id);
            return false;
        }

        return true;
    }

    public bool GroupContainsIn(int id)
    {
        if (!GroupDict.ContainsKey(id))
        {
            log.GroupNoExist(id);
            return false;
        }

        return true;
    }

    public void AddTask()
    {
        string input;
        Task temp = new Task();
        log.EnterSome("disc");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        temp.Description = input;
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

        int nextTask = LastId() + 1;
        temp.Id = nextTask;
        Dict.Add(nextTask, temp);
        log.SuccessfullyAdding(nextTask, "task");
    }

    public void AddSubTask()
    {
        string? input;
        SubTask subtemp = new SubTask();
        log.EnterSome("task");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        if (TaskContainsIn(Int32.Parse(input)))
        {
            subtemp.ParentId = Int32.Parse(input);
            log.EnterSome("disc");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Environment.Exit(0);
            }

            subtemp.Description = input;
            int nextSubTask = LastSId() + 1;
            subtemp.Id = nextSubTask;
            Dict[subtemp.ParentId].SubTasks.Add(nextSubTask);
            SubDict.Add(nextSubTask, subtemp);
            CheckComplete(subtemp.ParentId);
            log.SuccessfullyAdding(nextSubTask, "subtask");
        }
    }

    public void WriteOne(int id)

    {
        if (id == -1)
        {
            string? input;
            log.EnterSome("task");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Environment.Exit(0);
            }

            id = Int32.Parse(input);
        }

        if (TaskContainsIn(id))
        {
            if (Dict[id].SubTasks.Any())
            {
                // добавить в вывод процент выполнения.
                log.PrintTask(Dict[id]);
                SubTask subT;
                foreach (int subId in Dict[id].SubTasks)
                {
                    subT = SubDict[subId];
                    log.PrintSubtask(subT);
                }
            }
            else
            {
                log.PrintTask(Dict[id]);
            }
        }
    }

    public void WriteAll()
    {
        if (Dict.Any())
        {
            foreach (int id in Dict.Keys)
            {
                WriteOne(id);
            }
        }
        else
        {
            log.EmptyTaskList();
        }
    }

    public void WriteAllComplete()
    {
        if (Dict.Any())
        {
            foreach (int id in Dict.Keys)
            {
                if (Dict[id].Complete)
                {
                    WriteOne(id);
                }
            }
        }
        else
        {
            log.EmptyTaskList();
        }
    }

    public void DeleteOne()
    {
        string? input;
        log.EnterSome("task");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        int id = Int32.Parse(input);
        if (TaskContainsIn(id))
        {
            foreach (int i in Dict[id].SubTasks)
            {
                SubDict.Remove(i);
            }

            foreach (Group i in GroupDict.Values)
            {
                if (i.GroupTasks.Contains(id))
                {
                    i.GroupTasks.Remove(id);
                }
            }

            Dict.Remove(id);
            log.SuccessfullyRemoving(id, "task");
        }
    }

    public void DeleteSOne()
    {
        string? input;
        log.EnterSome("subtask");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        int id = Int32.Parse(input);
        if (SubTaskContainsIn(id))
        {
            Dict[SubDict[id].ParentId].SubTasks.Remove(id);
            CheckComplete(SubDict[id].ParentId);
            SubDict.Remove(id);
            log.SuccessfullyRemoving(id, "subtask");
        }
    }

    public void DeleteAll()
    {
        string? input;
        log.Deletelogs("repeat");
        input = Console.ReadLine();
        if (input == "/delete-all")
        {
            Dict.Clear();
            SubDict.Clear();
            foreach (Group i in GroupDict.Values)
            {
                i.GroupTasks.Clear();
            }

            log.Deletelogs("suc");
        }
        else
        {
            log.Deletelogs("cancel");
        }
    }

    public void PerformTask()
    {
        string? input;
        log.EnterSome("task");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        int id = Int32.Parse(input);
        if (TaskContainsIn(id))
        {
            if (Dict[id].Complete == true)
            {
                log.AlredyComp("task");
            }
            else
            {
                Dict[id].PerformTask();
                if (Dict[id].SubTasks.Any())
                {
                    foreach (int i in Dict[id].SubTasks)
                    {
                        SubDict[i].Complete = true;
                    }
                }

                log.NowComp(id, "task");
            }
        }
    }

    public void PerformSubTask()
    {
        string? input;
        log.EnterSome("subtask");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        int id = Int32.Parse(input);
        if (SubTaskContainsIn(id))
        {
            if (SubDict[id].Complete == true)
            {
                log.AlredyComp("subtask");
            }
            else
            {
                SubDict[id].PerformSubTask();
                CheckComplete(SubDict[id].ParentId);
                log.NowComp(id, "subtask");
            }
        }
    }

    public void AddGroup()
    {
        string? input;
        Group tempGroup = new Group();
        log.EnterSome("name");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        tempGroup.Name = input;

        int nextGroup = LastGId() + 1;
        tempGroup.Id = nextGroup;
        GroupDict.Add(nextGroup, tempGroup);
        log.SuccessfullyAdding(nextGroup, "group");
    }

    public void TaskToGroup()
    {
        string? input;
        log.EnterSome("group");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        int GId = Int32.Parse(input);
        if (GroupContainsIn(GId))
        {
            log.EnterSome("task");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Environment.Exit(0);
            }

            int TId = Int32.Parse(input);
            if (TaskContainsIn(TId))
            {
                if (GroupDict[GId].GroupTasks.Contains(TId))
                {
                    log.FromToGroup(TId, GId, 1);
                }
                else
                {
                    GroupDict[GId].GroupTasks.Add(TId);
                    log.FromToGroup(TId, GId, 2);
                }
            }
        }
    }

    public void TaskFromGroup()
    {
        string? input;
        log.EnterSome("group");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        int GId = Int32.Parse(input);
        if (GroupContainsIn(GId))
        {
            log.EnterSome("task");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Environment.Exit(0);
            }

            int TId = Int32.Parse(input);

            if (TaskContainsIn(TId))
            {
                if (!GroupDict[GId].GroupTasks.Contains(TId))
                {
                    log.FromToGroup(TId, GId, 3);
                }
                else
                {
                    GroupDict[GId].GroupTasks.Remove(TId);
                    log.FromToGroup(TId, GId, 4);
                }
            }
        }
    }

    public void DeleteGroup()
    {
        string? input;
        log.EnterSome("group");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        int tempId = Int32.Parse(input);
        if (GroupContainsIn(tempId))
        {
            GroupDict.Remove(tempId);
            log.SuccessfullyRemoving(tempId, "group");
        }
    }

    public void ShowGroup(int GId)
    {
        if (GId == -1)
        {
            string? input;
            log.EnterSome("group");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Environment.Exit(0);
            }

            GId = Int32.Parse(input);
        }

        if (GroupContainsIn(GId))
        {
            log.PrintGroup(GroupDict[GId]);
            foreach (int i in GroupDict[GId].GroupTasks)
            {
                log.WriteSpecial(0);
                WriteOne(i);
            }
        }
    }

    public void ShowGroups()
    {
        foreach (int i in GroupDict.Keys)
        {
            ShowGroup(i);
        }
    }

    public void TodayTasks()
    {
        int counter = 0;
        foreach (int i in Dict.Keys)
        {
            if (Dict[i].Dl == DateTime.Today)
            {
                if (counter == 0)
                {
                    log.TodayLogs("today", -1);
                }

                counter += 1;
                log.WriteSpecial(counter);
                WriteOne(i);
            }
        }

        if (counter == 0)
        {
            log.TodayLogs("no", -1);
        }
        else
        {
            log.TodayLogs("count", counter);
        }
    }

    public void SetDL()
    {
        string? input;
        log.EnterSome("task");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        int id = Int32.Parse(input);

        if (TaskContainsIn(id))
        {
            log.EnterSome("deate1");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Environment.Exit(0);
            }

            string[] numsStrings = input.Split(", ");
            Dict[id].Dl = new DateTime(Convert.ToInt32(numsStrings[2]), Convert.ToInt32(numsStrings[1]),
                Convert.ToInt32(numsStrings[0]));
        }
    }
}