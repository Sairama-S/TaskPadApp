using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ConsoleTables;

namespace TaskPadApp
{
    public enum TaskStatus
    {
        Ongoing,
        Completed
    }

    //public enum TaskPriority
    //{
    //    Low,
    //    Normal,
    //    High,
    //    Urgent
    //}

    public class TaskItem
    {        
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TaskStatus Status { get; set; }
        //public TaskPriority Priority { get; set; }
        //public DateTime DueDate { get; set; }
    }

    public class TodoManager
    {
        public List<TaskItem> tasks = new();
        public bool isSaved = false;

        private TaskItem? _getTaskById()
        {
            if (tasks.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("There are no tasks present right now. Enter 1 to add a task.");
                Console.ForegroundColor = ConsoleColor.White;
                return null;
            }
            Console.Write("Enter task ID: ");
            int taskId = Convert.ToInt32(Console.ReadLine());

            var taskEnumerable = tasks.Where(task => task.Id == taskId);
            if (!taskEnumerable.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Task ID {taskId} does not exist. Enter an ID of one of the existing tasks");
                Console.ForegroundColor = ConsoleColor.White;
                return null;
            }

            TaskItem taskItem = taskEnumerable.First();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Task {taskItem.Id} successfully retrieved.");
            Console.ForegroundColor = ConsoleColor.White;
            return taskItem;
        }

        public static int getInput(int first, int last)
        {
            int input;
            try
            {
                input = Convert.ToInt16(Console.ReadLine());
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Input is not an integer. Please try again");
                Console.ForegroundColor = ConsoleColor.White;
                return 0;
            }
            if (input < first || input > last)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Enter a number only in the given range. Please try again");
                Console.ForegroundColor = ConsoleColor.White;
                return 0;
            }
            return input;

        }

        public void addTask()
        {
            TaskItem item = new TaskItem();

            Console.WriteLine("Enter title: ");
            item.Title += Console.ReadLine();

            Console.WriteLine("Enter description: ");
            item.Description += Console.ReadLine();

            item.Id = Convert.ToInt32(File.ReadAllText("nextTaskId.txt"));
            File.WriteAllText("nextTaskId.txt", (item.Id + 1).ToString());

            tasks.Add(item);
            isSaved = false;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Your task with ID {item.Id} has been added.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void getTaskDetails()
        {            
            TaskItem task = _getTaskById();
            if (task != null)
            {
                var taskDetail = new ConsoleTable("ID", task.Id.ToString());
                //taskDetail.AddRow("ID", task.Id);
                taskDetail.AddRow("Title", task.Title);
                //string status = task.Status == TaskStatus.Completed ? " \u001b[32m Completed \u001b[0m " : " \u001b[33m Ongoing \u001b[0m ";
                taskDetail.AddRow("Task Status", task.Status);
                taskDetail.AddRow("Description", task.Description);
                taskDetail.Write();
            }
        }

        public void getTasksOverview()
        {
            if (tasks.Count > 0)
            {
                Console.WriteLine("Tasks Menu\n");
                List<TaskItem>.Enumerator tasksEnumerator = tasks.GetEnumerator();
                var taskTable = new ConsoleTable("ID", "Title", "Status");
                for(int i = 0; i < tasks.Count; i++)
                {
                    //string status = tasks[i].Status == TaskStatus.Completed ? " \u001b[32m Completed \u001b[0m " : " \u001b[33m Ongoing \u001b[0m ";
                    taskTable.AddRow(tasks[i].Id, tasks[i].Title, tasks[i].Status);
                    //Console.WriteLine($"\tID: {tasks[i].Id}\t Title: {tasks[i].Title}\t Task Status: {status}"); 
                }
                taskTable.Write();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("There are no tasks to display. Enter 1 to add a task.");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void setTaskStatus()
        {
            TaskItem task = _getTaskById();
            if (task != null)
            {
                Console.WriteLine("To set task status as Completed, enter 1");
                Console.WriteLine("To set task status as Ongoing, enter 2");
                Console.Write("Enter choice: ");
                int statusChoice = getInput(1, 2);
                if (statusChoice == 1)
                {
                    task.Status = TaskStatus.Completed;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Task status set to Completed.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (statusChoice == 2)
                {
                    task.Status = TaskStatus.Ongoing;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Task status set to Ongoing.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                isSaved = false;
            }
        }

        public void updateTask()
        {
            TaskItem task = _getTaskById();

            if (task != null)
            {
                Console.WriteLine("To update task title, enter 1");
                Console.WriteLine("To update task description, enter 2");
                Console.Write("Enter choice: ");
                int userChoice = getInput(1, 2);
                if (userChoice == 1)
                {
                    Console.Write("Enter new title: ");
                    task.Title = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Title updated");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (userChoice == 2)
                {
                    Console.Write("Enter new description: ");
                    task.Description = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Description updated");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                isSaved = false;
            }
        }

        public void deleteTask()
        {
            TaskItem task = _getTaskById();
            if (task != null)
            {
                tasks.Remove(task);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Task {task.Id} deleted.");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }

            isSaved= false;
        }
    }

    public static class FileManager
    {        
        public static void loadTasks(TodoManager taskSessionObj, string loadfile = null)
        {
            if (loadfile == null)
            {
                string[] files = Directory.GetFiles("../../../", "*.json");
                if (files.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("There are currently no files to load your tasks..");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

                Console.WriteLine("Enter the name of one of the following load files:");
                for (int i = 0; i < files.Length; i++)
                {
                    string fileName = files[i].Substring(9).Replace(".json", "");
                    Console.WriteLine($"{i + 1}. {fileName}");
                }
                Console.Write("\n>");
                loadfile = Console.ReadLine();
            }
            //if (!File.Exists($"{fname}"))
            //{
            //    Console.WriteLine($"File: {fname}.json does not exist. Enter an existing file name.");
            //    return;
            //}
            string jsonstr = "";
            try
            {
                jsonstr = File.ReadAllText("../../../" + loadfile + ".json");
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"File: {loadfile}.json does not exist. Enter an existing file name.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            taskSessionObj.tasks = JsonSerializer.Deserialize<List<TaskItem>>(jsonstr);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Tasks loaded from file successfully.");
            Console.ForegroundColor = ConsoleColor.White;
            taskSessionObj.isSaved = true;
        }

        public static void saveTasks(TodoManager taskSessionObj)
        {
            if (taskSessionObj.tasks == null || taskSessionObj.tasks.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("There are no task items to save.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            Console.WriteLine("Enter a unique file name to save the current set of tasks");
            string savefile = Console.ReadLine();
            if (string.IsNullOrEmpty(savefile))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Enter a non-empty string.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            File.WriteAllText($"../../../{savefile}.json", JsonSerializer.Serialize<List<TaskItem>>(taskSessionObj.tasks));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Tasks saved to {savefile}.json successfully.");
            Console.ForegroundColor = ConsoleColor.White;
            taskSessionObj.isSaved = true;
        }
    }
}
