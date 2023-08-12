namespace TaskPadApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Welcome to TaskPad v1.0");
            bool exitFlag = false;
            TodoManager taskSession = new TodoManager();

            while (!exitFlag)
            {
                Console.WriteLine("Enter the list number of the operation you want to perform from the below list:-");
                Console.WriteLine("1. Add a task");
                Console.WriteLine("2. View all tasks");
                Console.WriteLine("3. View a specific task");
                Console.WriteLine("4. Set task status (Ongoing/Completed)");
                Console.WriteLine("5. Update a task");
                Console.WriteLine("6. Delete a task");
                Console.WriteLine("7. Save tasks to a file");
                Console.WriteLine("8. Load tasks from a file");
                Console.WriteLine("9. Exit");
                int option = 10;
                Console.Write("\nEnter choice: ");
                try
                {
                    option = Convert.ToInt16(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Input expression is not a number. Try again");
                    Console.WriteLine("\n********************************************************\n");

                    continue;
                }

                if (option < 1 || option > 9)
                {
                    Console.WriteLine("Input must lie between 1 to 9. Try again");
                    Console.WriteLine("\n********************************************************\n");
                    continue;
                }
                switch (option)
                {
                    case 1:
                        taskSession.addTask();
                        Console.WriteLine("\n********************************************************\n");
                        break;
                    case 2:
                        taskSession.getTasksOverview();
                        Console.WriteLine("\n********************************************************\n");
                        break;
                    case 3:
                        taskSession.getTaskDetails();
                        Console.WriteLine("\n********************************************************\n");
                        break;
                    case 4:
                        taskSession.setTaskStatus();
                        Console.WriteLine("\n********************************************************\n");
                        break;
                    case 5:
                        taskSession.updateTask();
                        Console.WriteLine("\n********************************************************\n");
                        break;
                    case 6:
                        taskSession.deleteTask();
                        Console.WriteLine("\n********************************************************\n");
                        break;
                    case 7:
                        FileManager.saveTasks(taskSession);
                        Console.WriteLine("\n********************************************************\n");
                        break;
                    case 8:                        
                        FileManager.loadTasks(taskSession);
                        Console.WriteLine("\n********************************************************\n");
                        break;
                    
                    default:
                        if (taskSession.tasks == null || taskSession.tasks.Count == 0 || taskSession.isSaved == true)
                        {
                            Console.WriteLine("Exiting application.");
                            exitFlag = true;
                        }
                        else
                        {
                            Console.WriteLine("Do you want to save the tasks before exiting?");
                            Console.WriteLine("Enter 1 to save the tasks and exit");
                            Console.WriteLine("Enter 2 to exit without saving the tasks");
                            int choice = TodoManager.getInput(1, 2);
                            if (choice == 1)
                            {
                                FileManager.saveTasks(taskSession);
                                exitFlag = true;
                                Console.WriteLine("Exiting application.");
                            }
                            else if (choice == 2)
                            {
                                exitFlag = true;
                                Console.WriteLine("Exiting application.");
                            }
                        }
                        Console.WriteLine("\n********************************************************\n");
                        break;
                }
            }

        }
    }
}