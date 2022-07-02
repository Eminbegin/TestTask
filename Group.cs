namespace Taskii;

public class Group
{
    public string Name;
    public int Id;
    public List<int> GroupTasks = new List<int>();
    public Group()
    {
        this.Id = -1;
        this.Name = "";
        this.GroupTasks = new List<int>();
    }

    public Group(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
}