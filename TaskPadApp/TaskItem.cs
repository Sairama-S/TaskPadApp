using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        public List<TaskItem> tasks = new List<TaskItem>();

        private TaskItem? getTaskById()
        {
            Console.Write("Enter task ID: ");
            int taskId = Convert.ToInt32(Console.ReadLine());

            var taskEnumerable = tasks.Where(task => task.Id == taskId);
            if (!taskEnumerable.Any())
            {
                Console.WriteLine($"Task ID {taskId} does not exist. Enter an ID of one of the existing tasks");
                return null;
            }
            return taskEnumerable.ToArray()[0];
        }

        public void AddTask()
        {
            TaskItem item = new TaskItem();

            Console.WriteLine("Enter title: ");
            item.Title += Console.ReadLine();

            Console.WriteLine("Enter description: ");
            item.Description += Console.ReadLine();

            item.Id = Convert.ToInt32(File.ReadAllText("nextTaskId.txt"));
            File.WriteAllText("nextTaskId.txt", (item.Id + 1).ToString());

            tasks.Add(item);            
        }

        public void GetTaskDetails()
        {
            Console.Write("Enter task ID: ");
            int taskId = Convert.ToInt32(Console.ReadLine());
            if (taskId <= 0 || taskId > tasks.Count)
            {
                Console.WriteLine("Invalid Task ID. Enter an ID of one of the existing tasks");
            }
            else
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    if (tasks[i].Id == taskId)
                    {
                        Console.WriteLine($"ID: {tasks[i].Id}");
                        Console.WriteLine($"Title: {tasks[i].Title}");
                        Console.WriteLine($"Description:\n{tasks[i].Description}");
                        Console.WriteLine($"Task Status: {tasks[i].Status}");
                        break;
                    }
                }
            }            
        }

        public void GetTasksOverview()
        {
            Console.WriteLine("Tasks Menu\n");
            foreach (TaskItem item in tasks)
            {
                Console.WriteLine($"ID: {item.Id}\t Title: {item.Title}");
            }
        }

        public void SetTaskStatus()
        {
            Console.WriteLine("To set the status as Completed, enter 1");
            Console.WriteLine("To set the status as Ongoing, enter 2");
            Console.Write("Enter key: ");
            int statusChoice = Convert.ToInt32(Console.ReadLine());
            if (statusChoice == 1)
            {
                target.Status = TaskStatus.Completed;
            }
            else if (statusChoice == 2)
            {
                target.Status = TaskStatus.Ongoing;
            }
            else
            {
                Console.WriteLine("Incorrect key. Please try again.");
            }
        }

        public void UpdateTask()
        {
            Console.Write("Enter task ID: ");
            int taskId = Convert.ToInt32(Console.ReadLine());
            
            var taskEnumerable = tasks.Where(task => task.Id == taskId);
            if (!taskEnumerable.Any()) 
            {
                Console.WriteLine($"Task ID {taskId} does not exist. Enter an ID of one of the existing tasks");
                return;
            }

            foreach (TaskItem target in taskEnumerable)
            {
                Console.WriteLine("To update task title, enter 1");
                Console.WriteLine("To update task description, enter 2");
                Console.WriteLine("To update task status, enter 3");
                Console.Write("Enter key: ");
                int userChoice = Convert.ToInt32(Console.ReadLine());
                if (userChoice == 1)
                {
                    Console.Write("Enter new title: ");
                    target.Title = Console.ReadLine();
                    Console.WriteLine("Title updated");
                }
                else if (userChoice == 2)
                {
                    Console.Write("Enter new description: ");
                    target.Description = Console.ReadLine();
                    Console.WriteLine("Description updated");
                }
                else if (userChoice == 3)
                {
                    //
                }
                else
                {
                    Console.WriteLine("Incorrect key. Please try again.");
                }
            }
        }
    }

    public static class FileManager
    {        
        public static void LoadTasks(TodoManager taskSessionObj, string fname)
        {        
            if (!File.Exists(fname))
            {
                Console.WriteLine($"File: {fname}.json does not exist. Enter an existing file name.");
                return;
            }
            string jsonstr = File.ReadAllText(fname + ".json");
            taskSessionObj.tasks = JsonSerializer.Deserialize<List<TaskItem>>(jsonstr);
            Console.WriteLine($"Tasks loaded from file {fname}.json successfully.");
        }

        public static void SaveTasks(TodoManager taskSessionObj, string fname)
        {
            if (taskSessionObj.tasks == null || taskSessionObj.tasks.Count == 0)
            {
                Console.WriteLine("There are no task items to save.");
                return;
            }
            fname = String.IsNullOrEmpty(fname) ? "taskList" : fname;
            File.WriteAllText(fname + ".json", JsonSerializer.Serialize<List<TaskItem>>(taskSessionObj.tasks));
            Console.WriteLine($"Tasks saved to {fname}.json successfully.");
        }
    }
}
