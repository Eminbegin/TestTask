/*using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Linq;
using Microsoft.Extensions.Logging;*/

using System.Reflection;
using Newtonsoft.Json;

namespace Taskii
{
    class Program
    {
        static void Main()
        {
            ListOfTasks? allTasks;
            int tempId;
            int tempGId;
            string[] numsStrings;
            Console.WriteLine("Есть ли сохранённые задачи? Y/N?");
            string? path = Console.ReadLine();
            if (path == "Y")
            {
                Console.WriteLine("Введите путь к файлу, например {0}",
                    @"C:\Users\emink\Desktop\FFirsTT\Taskii\Taskii\bin\Debug\net6.0\mypath.json
                ");
                path = Console.ReadLine();
                if (string.IsNullOrEmpty(path))
                {
                    Environment.Exit(0);
                }

                if (!File.Exists(path))
                {
                    throw new Exception("Файл не найден");
                }

                string tempJson1 = File.ReadAllText(path);
                // path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
                allTasks = JsonConvert.DeserializeObject<ListOfTasks>(tempJson1);
            }
            else if (path == "N")
            {
                Console.WriteLine("Введите путь к файлу, например {0}",
                    @"C:\Users\emink\Desktop\FFirsTT\Taskii\Taskii\bin\Debug\net6.0\mypath.json
                ");
                path = Console.ReadLine();
                if (string.IsNullOrEmpty(path))
                {
                    Environment.Exit(0);
                }

                allTasks = new ListOfTasks();
            }
            else
            {
                Console.WriteLine("ЧЕЛ ХАРОШ");
                allTasks = new ListOfTasks();
                Environment.Exit(0);
            }

            Console.WriteLine("Используйте /help, чтобы увидеть список команд");
            string? input = Console.ReadLine();
            while (input != "/exit")
            {
                switch (input)
                {
                    case "/help":
                        Console.WriteLine("/help-t \t\t-\t cписок команд для задач");
                        Console.WriteLine("/help-s \t\t-\t cписок команд для подзадач");
                        Console.WriteLine("/help-g \t\t-\t cписок команд для групп");
                        Console.WriteLine("/exit \t\t\t-\t выход");
                        break;
                    case "/help-t":
                        Console.WriteLine("/create-task \t\t-\t создаёт новой задачи");
                        Console.WriteLine("/delete-task \t\t-\t удаляет задачу по id");
                        Console.WriteLine("/perform-task \t\t-\t отмечает задачу по id");
                        Console.WriteLine("/set-dl \t\t-\t устанвливает дедлайн на задачу");
                        Console.WriteLine("/today \t\t\t-\t выводит список задач, с дедлайном сегодня");
                        Console.WriteLine("/one-task \t\t-\t выводит задачу по id");
                        Console.WriteLine("/all-tasks \t\t-\t выводит все задачи");
                        Console.WriteLine("/complete-tasks \t-\t выводит все задачи");
                        Console.WriteLine("/delete-all \t\t-\t удаляет все задачи");
                        break;
                    case "/help-s":
                        Console.WriteLine("/create-subtask \t-\t создаёт новой подзадачи");
                        Console.WriteLine("/delete-subtask \t-\t удаляет подзадачу по id");
                        Console.WriteLine("/perform-subtask \t-\t отмечает подзадачу по id");
                        break;
                    case "/help-g":
                        Console.WriteLine("/create-group \t\t-\t создаёт группу");
                        Console.WriteLine("/delete-group \t\t-\t удаляет группу");
                        Console.WriteLine("/add-to \t\t-\t добавляет задачу в группу");
                        Console.WriteLine("/delete-from \t\t-\t удаляет задачу из группы");
                        Console.WriteLine("/show-group \t\t-\t выводит группу");
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
                        Console.WriteLine("Неизвестная команда");
                        break;
                }

                input = Console.ReadLine();
            }

            Console.WriteLine("Все задачи сохранены в файл {0}", path);
            string tempJson2 = JsonConvert.SerializeObject(allTasks);
            File.WriteAllText(path, tempJson2);
        }
    }
}