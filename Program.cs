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
                Console.WriteLine("Введите путь к файлу, например {0}", @"C:\Users\emink\Desktop\FFirsTT\Taskii\Taskii\bin\Debug\net6.0\mypath.json
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
                Console.WriteLine("Введите путь к файлу, например {0}", @"C:\Users\emink\Desktop\FFirsTT\Taskii\Taskii\bin\Debug\net6.0\mypath.json
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
                            numsStrings = input.Split(", ");
                            temp.Dl = new DateTime(Convert.ToInt32(numsStrings[2]), Convert.ToInt32(numsStrings[1]),
                                Convert.ToInt32(numsStrings[0]));
                        }

                        Console.WriteLine("Задача создана, task-id = {0}", allTasks.AddTask(temp));
                        break;
                    case "/all-tasks":
                        allTasks.WriteAll();
                        break;
                    case "/one-task":
                        Console.WriteLine("Введите id задачи");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }

                        tempId = Int32.Parse(input);
                        allTasks.WriteOne(tempId);
                        break;
                    case "/delete-task":
                        Console.WriteLine("Введите id задачи");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }

                        tempId = Int32.Parse(input);
                        allTasks.DeleteOne(tempId);
                        break;
                    case "/delete-all":
                        Console.WriteLine("Повторите команду для удаления всех задач");
                        input = Console.ReadLine();
                        if (input == "/delete-all")
                        {
                            allTasks.DeleteAll();
                        }
                        else
                        {
                            Console.WriteLine("Удаление отменено");
                        }

                        break;
                    case "/perform-task":
                        Console.WriteLine("Введите id задачи");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }

                        tempId = Int32.Parse(input);
                        allTasks.PerformTask(tempId);
                        break;
                    case "/create-subtask":
                        SubTask subtemp = new SubTask();
                        Console.WriteLine("Введите id задачи");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }
                        if (allTasks.TaskContainsIn(Int32.Parse(input)))
                        {
                            subtemp.ParentId = Int32.Parse(input);
                            Console.WriteLine("Введите описание");
                            input = Console.ReadLine();
                            if (string.IsNullOrEmpty(input))
                            {
                                Environment.Exit(0);
                            }

                            subtemp.Description = input;
                            Console.WriteLine("Подзадача создана, subtask-id = {0}", allTasks.AddSubTask(subtemp));
                        }
                        break;
                    case "/perform-subtask":
                        Console.WriteLine("Введите id подзадачи");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }
                        tempId = Int32.Parse(input);
                        allTasks.PerformSubTask(tempId);
                        break;
                    case "/delete-subtask":
                        Console.WriteLine("Введите id подзадачи");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }
                        tempId = Int32.Parse(input);
                        allTasks.DeleteSOne(tempId);
                        break;
                    case "/complete-tasks":
                        allTasks.WriteAllComplete();
                        break;
                    case "/create-group":
                        Group tempGroup = new Group();
                        Console.WriteLine("Введите название");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }
                        tempGroup.Name = input;
                        Console.WriteLine("Группа создана, group-id = {0}", allTasks.AddGroup(tempGroup));
                        break;
                    case "/delete-group":
                        Console.WriteLine("Введите id группы");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }
                        tempId = Int32.Parse(input);
                        allTasks.DeleteGroup(tempId);
                        break;
                    case "/add-to":
                        Console.WriteLine("Введите id группы");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }
                        tempGId = Int32.Parse(input);
                        Console.WriteLine("Введите id задачи");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }
                        tempId = Int32.Parse(input);
                        allTasks.TaskToGroup(tempGId, tempId);
                        break;
                    case "/delete-from":
                        Console.WriteLine("Введите id группы");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }
                        tempGId = Int32.Parse(input);
                        Console.WriteLine("Введите id задачи");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }
                        tempId = Int32.Parse(input);
                        allTasks.TaskFromGroup(tempGId, tempId);
                        break;
                    case "/show-group":
                        Console.WriteLine("Введите id группы");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }
                        allTasks.ShowGroup(Int32.Parse(input));
                        break;
                    case "/show-groups":
                        allTasks.ShowGroups();
                        break;
                    case "/today":
                        allTasks.TodayTasks();
                        break;
                    case "/set-dl":
                        Console.WriteLine("Введите id задачи");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }
                        tempId = Int32.Parse(input);
                        Console.WriteLine("Введите дату дедлайна (число, месяц, год)");
                        input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                        {
                            Environment.Exit(0);
                        }
                        numsStrings = input.Split(", ");
                        allTasks.SetDL(tempId,
                            new DateTime(Convert.ToInt32(numsStrings[2]), Convert.ToInt32(numsStrings[1]),
                                Convert.ToInt32(numsStrings[0])));
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