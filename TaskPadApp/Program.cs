namespace TaskPadApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool exitFlag = false;
            while (!exitFlag)
            {
                Console.WriteLine("Welcome to TaskPad v1.0");
                Console.WriteLine("Enter the list number of the operation you want to perform from the below list. Entering any other number will result in the termination of the application:-");
                Console.WriteLine("1. Add a task");
                Console.WriteLine("2. View all tasks");
                Console.WriteLine("3. View a specific task");
                Console.WriteLine("4. Set task status (Ongoing/Completed)");
                Console.WriteLine("5. Update a task");
                Console.WriteLine("6. Delete a task");
                Console.WriteLine("7. Save tasks to a file");
                Console.WriteLine("8. Load tasks from a file");
                Console.WriteLine("9. Exit");
                Console.Write("\nEnter number: ");
                int option = Console.Read();
                
                TodoManager taskSession = new TodoManager();
                switch (option)
                {
                    case 1:
                        taskSession.AddTask();
                        break;
                    case 2:
                        taskSession.GetTasksOverview();
                        break;
                    case 3:
                        taskSession.GetTaskDetails();
                        break;
                    case 4:
                        //taskSession.MarkAsCompleted(); TODO
                        break;
                    case 5:
                        taskSession.UpdateTask();
                        break;
                    case 6:
                        //taskSession.DeleteTask(); TODO
                        break;
                    case 7:
                        Console.WriteLine("Enter a unique file name to save the current set of tasks");
                        string savefile = Console.ReadLine();
                        FileManager.SaveTasks(taskSession, savefile);
                        // For every save, generate a unique id number stored from a text file and give name as tasks<number>.json as a default
                        break;
                    case 8:
                        Console.WriteLine("Enter the file name of the task list you want to load");
                        string loadfile = Console.ReadLine();
                        FileManager.LoadTasks(taskSession, loadfile);
                        break;
                    
                    default:
                        Console.WriteLine("Exiting application.");
                        exitFlag = true;
                        break;
                }
            }

        }
    }
}