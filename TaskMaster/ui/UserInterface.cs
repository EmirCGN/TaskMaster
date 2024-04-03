using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMaster.database;
using TaskMaster.model;

namespace TaskMaster.ui
{
    public class UserInterface
    {

        private static int selectedOptionIndex = 0;
        private readonly ConsoleColor defaultColor;

        public static void ShowRegistrationScreen()
        {
            try
            {
                Console.Clear();
                Console.CursorVisible = false;
            }catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }
        }

        private static void PrintLogo()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string[] logoLines =
            {

            };
            int windowWidth = Console.WindowWidth;
            foreach (string line in logoLines)
            {
                Console.WriteLine(CenterText(line, windowWidth));
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        private static void PrintMenuOptions()
        {
            Console.WriteLine("\t\t\t\t\t\t Please select an option:");
            string[] menuOptions = { "Login", "Register", "Exit" };
            int longestOptionLength = menuOptions.Max(option => option.Length);
            int padding = (Console.WindowWidth - longestOptionLength) / 2;
            for (int i = 0; i < menuOptions.Length; i++)
            {
                if (i == selectedOptionIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(new string(' ', padding));
                    Console.Write("> ");
                }
                else
                {
                    Console.Write(new string(' ', padding + 2));
                }
                Console.WriteLine($"{i + 1}. {menuOptions[i]}");
                Console.ResetColor();
            }
        }

        private static void HandleUserInput()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedOptionIndex > 0)
                    {
                        selectedOptionIndex--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (selectedOptionIndex < 2)
                    {
                        selectedOptionIndex++;
                    }
                    break;
                case ConsoleKey.Enter:
                    PerformSelectedAction(selectedOptionIndex);
                    break;
            }
        }

        private static void PerformSelectedAction(int selectedOptionIndex)
        {
            switch (selectedOptionIndex)
            {
                case 0:
                    ShowLoginScreen();
                    break;
                case 1:
                    ShowRegistrationScreen();
                    break;
                case 2:
                    Environment.Exit(0);
                    break;
            }
        }


        public static string CenterText(string text, int width)
        {
            return text.PadLeft((width + text.Length) / 2).PadRight(width);
        }
        private static void ShowLoginScreen()
        {

        }

        //private static void ShowRegistrationScreen()
        //{
        //    try
        //    {
        //        Console.WriteLine("User Registration:");
        //        Console.Write("Username: ");
        //        string username = Console.ReadLine();
        //        Console.Write("Password: ");
        //        string password = Console.ReadLine();
        //        Console.Write("Language: ");
        //        string language = Console.ReadLine();

        //        DatabaseManager.SaveUser(username, password, language);

        //        Console.WriteLine($"User successfully registered! {username}");
        //        Thread.Sleep(2000);

        //        ConsoleUI chatUI = new ConsoleUI(new User { Username = username });
        //        chatUI.RunChatMenu();

        //        Console.WriteLine("Press any key to continue...");
        //        Console.ReadKey();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred during registration: {ex.Message}");
        //    }
        //}

        public void ShowTaskListScreen(User user)
        {
            Console.WriteLine($"Task List for User: {user.Username}");
            foreach (var task in user.Tasks)
            {
                Console.WriteLine($"Title: {task.Title}, Description: {task.Description}, Due Date: {task.DueDate}");
                // Weitere Informationen zu Aufgaben anzeigen
            }
        }

        public void ShowAddTaskScreen(User user)
        {
            Console.WriteLine("Add Task Screen");
            // Implementierung des Bildschirms zum Hinzufügen einer Aufgabe
        }

        public void ShowUpdateTaskScreen(User user)
        {
            Console.WriteLine("Update Task Screen");
            // Implementierung des Bildschirms zum Aktualisieren einer Aufgabe
        }

        public void ShowDeleteTaskScreen(User user)
        {
            Console.WriteLine("Delete Task Screen");
            // Implementierung des Bildschirms zum Löschen einer Aufgabe
        }

        public void ShowSearchScreen(User user)
        {
            Console.WriteLine("Search Screen");
            // Implementierung des Suchbildschirms
        }

        public void ShowFilterScreen(User user)
        {
            Console.WriteLine("Filter Screen");
            // Implementierung des Filterbildschirms
        }

        public void ShowSortOptionsScreen(User user)
        {
            Console.WriteLine("Sort Options Screen");
            // Implementierung des Bildschirms mit Sortieroptionen
        }

        public void ShowNotificationScreen(string message)
        {
            Console.WriteLine($"Notification: {message}");
            // Implementierung des Bildschirms für Benachrichtigungen
        }
    }
}
