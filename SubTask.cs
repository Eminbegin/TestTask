namespace Taskii;

public class SubTask
{
    public int Id { get; set; }
    
    public int ParentId { get; set; }
    public string Description  { get; set; }
    public bool Complete { get; set; }
    public SubTask()
    {
        this.Id = -1;
        this.ParentId = -1;
        this.Description = "";
        this.Complete = false;
    }
    public SubTask(int id, string description)
    {
        this.Id = id;
        this.Description = description;
    }

    public void PerformSubTask()
    {
        Complete = true;
    }
}