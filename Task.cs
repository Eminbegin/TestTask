using System.Numerics;

namespace Taskii;

public class Task
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime Dl { get; set; }
    public bool Complete { get; set; }
    public List<int> SubTasks { get; set; }

    public Task()
    {
        this.Id = -1;
        this.Description = "";
        this.Dl = DateTime.MaxValue; //ToShortDateString()
        this.Complete = false;
        this.SubTasks = new List<int>();
    }

    public Task(int id, string description, DateTime dl)
    {
        this.Id = id;
        this.Description = description;
        this.Dl = dl;
    }

    public void PerformTask()
    {
        Complete = true;
    }

    public void UnPerformTask()
    {
        Complete = false;
    }
}