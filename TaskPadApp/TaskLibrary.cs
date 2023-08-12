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
    
    public class TaskItem
    {
        int id;
        string title = "";
        string description = "";
        TaskStatus status = TaskStatus.Ongoing;
        
        public int Id { get { return id; } set { id = value; } }
        public string Title { get { return title; } set { title = value; } }
        public string Description { get { return description; } set { description = value; } }
        public TaskStatus Status { get { return status; } set { status = value; } }
    }

    public class TodoManager
    {
        public List<TaskItem> tasks = new();
        public bool isSaved = false;

        private TaskItem? _getTaskById()
        {
            Console.Write("Enter task ID: ");
            int taskId = Convert.ToInt32(Console.ReadLine());

            var taskEnumerable = tasks.Where(task => task.Id == taskId);
            if (!taskEnumerable.Any())
            {
                Console.WriteLine($"Task ID {taskId} does not exist. Enter an ID of one of the existing tasks");
                return null;
            }

            TaskItem taskItem = taskEnumerable.First();
            Console.WriteLine($"Task {taskItem.Id} successfully retrieved.");
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
                Console.WriteLine("Input is not an integer. Please try again");
                return 0;
            }
            if (input < first || input > last)
            {
                Console.WriteLine("Enter a number only in the given range. Please try again");
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
            Console.WriteLine($"Your task with ID {item.Id} has been added.");
        }

        public void getTaskDetails()
        {
            TaskItem task = _getTaskById();
            if (task != null)
            {
                var taskDetail = new ConsoleTable("ID", task.Id.ToString());
                //taskDetail.AddRow("ID", task.Id);
                taskDetail.AddRow("Title", task.Title);
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
                    string status = tasks[i].Status == TaskStatus.Ongoing ? "Ongoing" : "Completed";
                    taskTable.AddRow(tasks[i].Id, tasks[i].Title, tasks[i].Status);
                    //Console.WriteLine($"\tID: {tasks[i].Id}\t Title: {tasks[i].Title}\t Task Status: {status}"); 
                }
                taskTable.Write();
            }
            else
            {
                Console.WriteLine("There are no tasks to display. Enter 1 to add a task.");
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
                    Console.WriteLine("Task status set to Completed.");
                }
                else if (statusChoice == 2)
                {
                    task.Status = TaskStatus.Ongoing;
                    Console.WriteLine("Task status set to Ongoing.");
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
                    Console.WriteLine("Title updated");
                }
                else if (userChoice == 2)
                {
                    Console.Write("Enter new description: ");
                    task.Description = Console.ReadLine();
                    Console.WriteLine("Description updated");
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
                Console.WriteLine($"Task {task.Id} deleted.");
            }

            isSaved= false;
        }
    }

    public static class FileManager
    {        
        public static void loadTasks(TodoManager taskSessionObj)
        {
            string[] files = Directory.GetFiles("../../../", "*.json");
            if (files.Length == 0)
            {
                Console.WriteLine("There are currently no files to load your tasks..");
                return;
            }

            Console.WriteLine("Enter the name of one of the following load files:");
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = files[i].Substring(9).Replace(".json", "");
                Console.WriteLine($"{i + 1}. {fileName}");
            }
            Console.Write("\n>");
            string loadfile = Console.ReadLine();
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
                Console.WriteLine($"File: {loadfile}.json does not exist. Enter an existing file name.");
                return;
            }
            taskSessionObj.tasks = JsonSerializer.Deserialize<List<TaskItem>>(jsonstr);
            Console.WriteLine($"Tasks loaded from file successfully.");
            taskSessionObj.isSaved = true;
        }

        public static void saveTasks(TodoManager taskSessionObj)
        {
            if (taskSessionObj.tasks == null || taskSessionObj.tasks.Count == 0)
            {
                Console.WriteLine("There are no task items to save.");
                return;
            }

            Console.WriteLine("Enter a unique file name to save the current set of tasks");
            string savefile = Console.ReadLine();
            if (string.IsNullOrEmpty(savefile))
            {
                Console.WriteLine("Enter a non-empty string.");
                return;
            }
            File.WriteAllText($"../../../{savefile}.json", JsonSerializer.Serialize<List<TaskItem>>(taskSessionObj.tasks));
            Console.WriteLine($"Tasks saved to {savefile}.json successfully.");
            taskSessionObj.isSaved = true;
        }
    }
}
