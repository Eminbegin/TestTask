/*using System.ComponentModel;
using System.Threading.Tasks.Dataflow;
using System.IO;
using System.Text.Json;*/

using System.ComponentModel.Design;
using System.Threading.Tasks.Dataflow;

namespace Taskii;

using Newtonsoft.Json;

public class ListOfTasks
{
    public Dictionary<int, Task> Dict = new Dictionary<int, Task>();
    public Dictionary<int, SubTask> SubDict = new Dictionary<int, SubTask>();
    public Dictionary<int, Group> GroupDict = new Dictionary<int, Group>();

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
            Console.WriteLine("не существует задачи с id = {0}", id);
            return false;
        }

        return true;
    }

    public bool SubTaskContainsIn(int id)
    {
        if (!SubDict.ContainsKey(id))
        {
            Console.WriteLine("не существует подзадачи с id = {0}", id);
            return false;
        }

        return true;
    }

    public bool GroupContainsIn(int id)
    {
        if (!GroupDict.ContainsKey(id))
        {
            Console.WriteLine("не существует группы с id = {0}", id);
            return false;
        }

        return true;
    }

    public void AddTask()
    {
        string input;
        Task temp = new Task();
        Console.WriteLine("Введите описание");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        temp.Description = input;
        Console.WriteLine("Введите дату дедлайна (число, месяц, год); если его нет, введите \"-\"");
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
        Console.WriteLine("Задача создана, task-id = {0}", nextTask);
    }

    public void AddSubTask()
    {
        string? input;
        SubTask subtemp = new SubTask();
        Console.WriteLine("Введите id задачи");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        if (TaskContainsIn(Int32.Parse(input)))
        {
            subtemp.ParentId = Int32.Parse(input);
            Console.WriteLine("Введите описание");
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
            Console.WriteLine("Подзадача создана, subtask-id = {0}", nextSubTask);
        }
    }

    public void WriteOne(int id)

    {
        if (id == -1)
        {
            string? input;
            Console.WriteLine("Введите id задачи");
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
                Console.WriteLine("[{0}] {1}task-id = {2}, {3}", Dict[id].Complete ? "x" : " ",
                    Dict[id].Dl == DateTime.MaxValue ? "" : Dict[id].Dl.ToShortDateString() + " ", id,
                    Dict[id].Description);
                SubTask subT;
                foreach (int subId in Dict[id].SubTasks)
                {
                    subT = SubDict[subId];
                    Console.WriteLine("  - [{0}] subtask-id = {1}, {2}", subT.Complete ? "x" : " ", subT.Id,
                        subT.Description);
                }
            }
            else
            {
                Console.WriteLine("[{0}] {1}task-id = {2}, {3}", Dict[id].Complete ? "x" : " ",
                    Dict[id].Dl == DateTime.MaxValue ? "" : Dict[id].Dl.ToShortDateString() + " ", id,
                    Dict[id].Description);
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
            Console.WriteLine("Список задач пуст");
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
            Console.WriteLine("Список задач пуст");
        }
    }

    public void DeleteOne()
    {
        string? input;
        Console.WriteLine("Введите id задачи");
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
            Console.WriteLine("Задача с id = {0} удалена", id);
        }
    }

    public void DeleteSOne()
    {
        string? input;
        Console.WriteLine("Введите id подзадачи");
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
            Console.WriteLine("Подзадача с id = {0} удалена", id);
        }
    }

    public void DeleteAll()
    {
        string? input;
        Console.WriteLine("Повторите команду для удаления всех задач");
        input = Console.ReadLine();
        if (input == "/delete-all")
        {
            Dict.Clear();
            SubDict.Clear();
            foreach (Group i in GroupDict.Values)
            {
                i.GroupTasks.Clear();
            }

            Console.WriteLine("Все задачи были удалены");
        }
        else
        {
            Console.WriteLine("Удаление отменено");
        }
    }

    public void PerformTask()
    {
        string? input;
        Console.WriteLine("Введите id задачи");
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
                Console.WriteLine("Задача уже выполнена");
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

                Console.WriteLine("Задача с id = {0} теперь выполена", id);
            }
        }
    }

    public void PerformSubTask()
    {
        string? input;
        Console.WriteLine("Введите id подзадачи");
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
                Console.WriteLine("Подзадача уже выполнена");
            }
            else
            {
                SubDict[id].PerformSubTask();
                CheckComplete(SubDict[id].ParentId);
                Console.WriteLine("Задача с id = {0} теперь выполена", id);
            }
        }
    }

    public void AddGroup()
    {
        string? input;
        Group tempGroup = new Group();
        Console.WriteLine("Введите название");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        tempGroup.Name = input;

        int nextGroup = LastGId() + 1;
        tempGroup.Id = nextGroup;
        GroupDict.Add(nextGroup, tempGroup);
        Console.WriteLine("Группа создана, group-id = {0}", nextGroup);
    }

    public void TaskToGroup()
    {
        string? input;
        Console.WriteLine("Введите id группы");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        int GId = Int32.Parse(input);
        if (GroupContainsIn(GId))
        {
            Console.WriteLine("Введите id задачи");
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
                    Console.WriteLine("Задача с task-id = {0} уже содержится в группе с group-id = {1}", TId, GId);
                }
                else
                {
                    GroupDict[GId].GroupTasks.Add(TId);
                    Console.WriteLine("Задача с task-id = {0} теперь содержится в группе с group-id = {1}", TId, GId);
                }
            }
        }
    }

    public void TaskFromGroup()
    {
        string? input;
        Console.WriteLine("Введите id группы");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        int GId = Int32.Parse(input);
        if (GroupContainsIn(GId))
        {
            Console.WriteLine("Введите id задачи");
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
                    Console.WriteLine("Задача с task-id = {0} и так не содержится в группе с group-id = {1}", TId, GId);
                }
                else
                {
                    GroupDict[GId].GroupTasks.Remove(TId);
                    Console.WriteLine("Задача с task-id = {0} теперь не содержится в группе с group-id = {1}", TId,
                        GId);
                }
            }
        }
    }

    public void DeleteGroup()
    {
        string? input;
        Console.WriteLine("Введите id группы");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        int tempId = Int32.Parse(input);
        if (GroupContainsIn(tempId))
        {
            GroupDict.Remove(tempId);
            Console.WriteLine("Группа удалена");
        }
    }

    public void ShowGroup(int GId)
    {
        if (GId == -1)
        {
            string? input;
            Console.WriteLine("Введите id группы");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Environment.Exit(0);
            }

            GId = Int32.Parse(input);
        }

        if (GroupContainsIn(GId))
        {
            Console.WriteLine("group-id = {0}, {1}", GId, GroupDict[GId].Name);
            foreach (int i in GroupDict[GId].GroupTasks)
            {
                Console.Write(" *");
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
                    Console.WriteLine("Задачи на сегодня:");
                }

                counter += 1;
                Console.Write("   {0})", counter);
                WriteOne(i);
            }
        }

        if (counter == 0)
        {
            Console.WriteLine("Задач на сегодня нет");
        }
        else
        {
            Console.WriteLine("Количесвто сегодняшних задач равно {0}", counter);
        }
    }

    public void SetDL()
    {
        string? input;
        Console.WriteLine("Введите id задачи");
        input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Environment.Exit(0);
        }

        int id = Int32.Parse(input);

        if (TaskContainsIn(id))
        {
            Console.WriteLine("Введите дату дедлайна (число, месяц, год)");
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