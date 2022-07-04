using System.Net.Sockets;
using Newtonsoft.Json;

namespace Taskii;

public class ListOfTasks
{
    private Logger log = Logger.GetInstnce();
    public Dictionary<int, Task> Dict = new Dictionary<int, Task>();
    public Dictionary<int, SubTask> SubDict = new Dictionary<int, SubTask>();
    public Dictionary<int, Group> GroupDict = new Dictionary<int, Group>();
    
    public string GetJson()
    {
        return JsonConvert.SerializeObject(this);
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

    private void CheckComplete(int id)
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

    public void AddTask(Task temp)
    {
        int nextTask = LastId() + 1;
        temp.Id = nextTask;
        Dict.Add(nextTask, temp);
        log.SuccessfullyAdding(nextTask, "task");
    }

    public void AddSubTask(SubTask subTemp)
    {
        int nextSubTask = LastSId() + 1;
        subTemp.Id = nextSubTask;
        Dict[subTemp.ParentId].SubTasks.Add(nextSubTask);
        SubDict.Add(nextSubTask, subTemp);
        CheckComplete(subTemp.ParentId);
        log.SuccessfullyAdding(nextSubTask, "subtask");
    }

    public void WriteOne(int id)
    {
        if (Dict[id].SubTasks.Any())
        {
            log.PrintTask(Dict[id]);
            foreach (int subId in Dict[id].SubTasks)
            {
                SubTask subTemp = SubDict[subId];
                log.PrintSubtask(subTemp);
            }
        }
        else
        {
            log.PrintTask(Dict[id]);
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

    public void DeleteOne(int id)
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

    public void DeleteSOne(int id)
    {
        Dict[SubDict[id].ParentId].SubTasks.Remove(id);
        CheckComplete(SubDict[id].ParentId);
        SubDict.Remove(id);
    }

    public void DeleteAll()
    {
        Dict.Clear();
        SubDict.Clear();
        foreach (Group i in GroupDict.Values)
        {
            i.GroupTasks.Clear();
        }
    }

    public void PerformTask(int id)
    {
        if (Dict[id].Complete)
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

    public void PerformSubTask(int id)
    {
        if (SubDict[id].Complete)
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

    public void AddGroup(Group tempGroup)
    {
        int nextGroup = LastGId() + 1;
        tempGroup.Id = nextGroup;
        GroupDict.Add(nextGroup, tempGroup);
        log.SuccessfullyAdding(nextGroup, "group");
    }

    public void TaskToGroup(int gid, int tid)
    {
        if (GroupDict[gid].GroupTasks.Contains(tid))
        {
            log.FromToGroup(tid, gid, 1);
        }
        else
        {
            GroupDict[gid].GroupTasks.Add(tid);
            log.FromToGroup(tid, gid, 2);
        }
    }

    public void TaskFromGroup(int gid, int tid)
    {
        if (!GroupDict[gid].GroupTasks.Contains(tid))
        {
            log.FromToGroup(tid, gid, 3);
        }
        else
        {
            GroupDict[gid].GroupTasks.Remove(tid);
            log.FromToGroup(tid, gid, 4);
        }
    }

    public void DeleteGroup(int id)
    {
        GroupDict.Remove(id);
        log.SuccessfullyRemoving(id, "group");
    }

    public void ShowGroup(int gid)
    {
        log.PrintGroup(GroupDict[gid]);
        foreach (int i in GroupDict[gid].GroupTasks)
        {
            log.WriteSpecial(0);
            WriteOne(i);
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

    public void SetDl(int id, string[] numsStrings)
    {
        Dict[id].Dl = new DateTime(Convert.ToInt32(numsStrings[2]), Convert.ToInt32(numsStrings[1]),
            Convert.ToInt32(numsStrings[0]));
    }
}