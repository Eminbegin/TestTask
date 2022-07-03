namespace Taskii
{
    class Program
    {
        private static Logger log = Logger.GetInstnce();

        static void Main()
        {
            ListOfTasks allTasks = new ListOfTasks();
            string path = allTasks.GetFromFile();
            log.FirstHelp();
            String? input = Console.ReadLine();
            while (input != "/exit")
            {
                CheckInput(input, allTasks);
                input = Console.ReadLine();
            }

            allTasks.SaveToFile(path);
        }

        static void CheckInput(string input, ListOfTasks allTasks)
        {
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
                    allTasks.AddTask();
                    break;
                case "/all-tasks":
                    allTasks.WriteAll();
                    break;
                case "/one-task":
                    allTasks.WriteOne(-1);
                    break;
                case "/delete-task":
                    allTasks.DeleteOne();
                    break;
                case "/delete-all":
                    allTasks.DeleteAll();
                    break;
                case "/perform-task":
                    allTasks.PerformTask();
                    break;
                case "/create-subtask":
                    allTasks.AddSubTask();
                    break;
                case "/perform-subtask":
                    allTasks.PerformSubTask();
                    break;
                case "/delete-subtask":
                    allTasks.DeleteSOne();
                    break;
                case "/complete-tasks":
                    allTasks.WriteAllComplete();
                    break;
                case "/create-group":
                    allTasks.AddGroup();
                    break;
                case "/delete-group":
                    allTasks.DeleteGroup();
                    break;
                case "/add-to":
                    allTasks.TaskToGroup();
                    break;
                case "/delete-from":
                    allTasks.TaskFromGroup();
                    break;
                case "/show-group":
                    allTasks.ShowGroup(-1);
                    break;
                case "/show-groups":
                    allTasks.ShowGroups();
                    break;
                case "/today":
                    allTasks.TodayTasks();
                    break;
                case "/set-dl":
                    allTasks.SetDL();
                    break;
                default:
                    log.UnknownCommand();
                    break;
            }
        }
    }
}