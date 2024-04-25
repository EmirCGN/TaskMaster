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
        #region Fields
        private static int selectedOptionIndex = 0;
        private readonly ConsoleColor defaultColor;
        private readonly User currentUser;
        private static DatabaseManager databaseManager = new DatabaseManager(); // Instanz des Datenbankmanagers
        #endregion

        #region Constructors
        public UserInterface(User user)
        {
            currentUser = user;
            defaultColor = Console.ForegroundColor;
            selectedOptionIndex = 0;
        }
        #endregion

        #region Main Menu Methods
        public static void RunMainMenu()
        {
            try
            {
                Console.Clear();
                Console.CursorVisible = false;

                PrintLogo();
                PrintMenuOptions();

                HandleUserInput();

                RunMainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static void PrintLogo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string[] logoLines =
            {
        " /$$$$$$$$                  /$$       /$$      /$$                       /$$                        \r\n|__  $$__/                 | $$      | $$$    /$$$                      | $$                        \r\n   | $$  /$$$$$$   /$$$$$$$| $$   /$$| $$$$  /$$$$  /$$$$$$   /$$$$$$$ /$$$$$$    /$$$$$$   /$$$$$$ \r\n   | $$ |____  $$ /$$_____/| $$  /$$/| $$ $$/$$ $$ |____  $$ /$$_____/|_  $$_/   /$$__  $$ /$$__  $$\r\n   | $$  /$$$$$$$|  $$$$$$ | $$$$$$/ | $$  $$$| $$  /$$$$$$$|  $$$$$$   | $$    | $$$$$$$$| $$  \\__/\r\n   | $$ /$$__  $$ \\____  $$| $$_  $$ | $$\\  $ | $$ /$$__  $$ \\____  $$  | $$ /$$| $$_____/| $$      \r\n   | $$|  $$$$$$$ /$$$$$$$/| $$ \\  $$| $$ \\/  | $$|  $$$$$$$ /$$$$$$$/  |  $$$$/|  $$$$$$$| $$      \r\n   |__/ \\_______/|_______/ |__/  \\__/|__/     |__/ \\_______/|_______/    \\___/   \\_______/|__/      \r\n                                                                                                    \r\n                                                                                                    \r\n                                                                                                    "
    };

            // Bestimme die Breite des Konsolenfensters und die maximale Breite des Logos
            int windowWidth = Console.WindowWidth;
            int maxLogoWidth = logoLines.Max(line => line.Length);

            // Berechne die Anzahl der Leerzeichen, um das Logo in der Mitte zu zentrieren
            int padding = Math.Max((windowWidth - maxLogoWidth) / 2, 0); // Stelle sicher, dass padding nicht negativ ist

            // Gib jede Zeile des Logos aus, zentriert in der Konsolenbreite
            foreach (string line in logoLines)
            {
                Console.WriteLine(new string(' ', padding) + line); // Ausgeben des zentrierten Logos
            }

            Console.WriteLine();
            Console.ResetColor();
        }



        private static void PrintMenuOptions()
        {

            string[] menuOptions = { "Login", "Register", "Exit" }; // Menüoptionen definieren
            int longestOptionLength = menuOptions.Max(option => option.Length); // bestimmt die längste Option
            int padding = (Console.WindowWidth - longestOptionLength) / 2; // Einrückung für Zentrierung berechnen
            for (int i = 0; i < menuOptions.Length; i++) // wiederholt über jede Menüoption um es zu zeigen
            {
                if (i == selectedOptionIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(new string(' ', padding));// Fügt Leerzeichen für die Ausrichtung hinzu.
                    Console.Write("> ");
                }
                else
                {
                    Console.Write(new string(' ', padding + 2)); // Fügt Leerzeichen hinzu, um nicht-ausgewählte Optionen auszurichten.
                }
                Console.WriteLine($"{i + 1}. {menuOptions[i]}");
                Console.ResetColor();
            }
        }

        private static void HandleUserInput()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo.Key) // wechselt je nach gedrückter taste zwischen den
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
        #endregion

        #region Helper Methods
        public static string CenterText(string text, int width)
        {
            return text.PadLeft((width + text.Length) / 2).PadRight(width);
        }

        private static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)// Überprüfen, ob die gedrückte Taste weder Backspace noch Enter ist
                {
                    password += key.KeyChar;// Hinzufügen des Zeichens zum Passwort
                    Console.Write("*");// Ausgeben eines Sternzeichens als Platzhalter für das Passwortzeichen
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0) // Wenn Backspace gedrückt wurde und das Passwort Zeichen enthält
                {
                    password = password.Substring(0, (password.Length - 1));// Entfernen des letzten Zeichens vom Passwort
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);// Wiederholt den Vorgang, bis die Enter-Taste gedrückt wird
            Console.WriteLine();
            return password;
        }

        #endregion

        #region Screen Login
        public static async Task ShowLoginScreen()
        {
            try
            {
                Console.WriteLine("Login:");
                Console.Write("Username: ");
                string username = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(username))// Überprüft ob das Feld für den Benutzernamen leer ist
                {
                    Console.WriteLine("Username cannot be empty. Please try again.");
                    return;
                }
                Console.Write("Password: ");
                string password = ReadPassword();
                if (string.IsNullOrWhiteSpace(password))// Überprüft ob das Feld für das Passwort leer ist
                {
                    Console.WriteLine("Password cannot be empty. Please try again.");
                    return;
                }

                // Aufruf der Methode zur Überprüfung der Login-Daten
                bool loggedIn = await DatabaseManager.CheckLoginAsync(username, password);

                if (loggedIn)
                {
                    Console.WriteLine($"Welcome back, {username}!");
                    await Task.Delay(1000);
                    UserInterface chatUI = new UserInterface(new User(username, password)); // Erstellung der Benutzeroberfläche für den eingeloggten Benutzer
                    chatUI.RunTaskMenu();
                }
                else
                {
                    Console.WriteLine("Login failed. Invalid username or password.");
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during login: {ex.Message}");
            }
        }


        public static async Task ShowRegistrationScreen()
        {
            try
            {
                Console.WriteLine("User Registration:");
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();

                 // Aufruf der Methode zur Überprüfung der Login-Daten
                await DatabaseManager.SaveUserAsync(username, password);

                Console.WriteLine($"User successfully registered! {username}");
                await Task.Delay(2000);

                UserInterface chatUI = new UserInterface(new User(username, password));
                chatUI.RunTaskMenu();

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during registration: {ex.Message}");
            }
        }

        public void RunTaskMenu()
        {
            try
            {
                Console.Clear();
                Console.Title = $"TaskMaster - {currentUser.Username}";
                Console.WriteLine($"Welcome to the Task Menu, {currentUser.Username}!");
                Thread.Sleep(2000);
                ShowTaskMenu(currentUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in the chat menu: {ex.Message}");
            }
        }

        public void ShowTaskMenu(User user)
        {
            bool exitMenu = false;
            do
            {
                Console.Clear();
                Console.WriteLine($"Task Menu - User: {user.Username}");
                Console.WriteLine("1. Show Task List");
                Console.WriteLine("2. Add Task");
                Console.WriteLine("3. Update Task");
                Console.WriteLine("4. Delete Task");
                Console.WriteLine("5. Search Tasks");
                Console.WriteLine("6. Filter Tasks");
                Console.WriteLine("7. Sort Tasks");
                Console.WriteLine("8. Logout");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowTaskListScreen(user);
                        Console.ReadLine();
                        break;
                    case "2":
                        ShowAddTaskScreen(user);
                        Console.ReadLine();
                        break;
                    case "3":
                        ShowUpdateTaskScreen(user).Wait();
                        Console.ReadLine();
                        break;
                    case "4":
                        ShowDeleteTaskScreen(user).Wait();
                        Console.ReadLine();
                        break;
                    case "5":
                        ShowSearchScreen(user).Wait();
                        Console.ReadLine();
                        break;
                    case "6":
                        ShowFilterScreen(user);
                        Console.ReadLine();
                        break;
                    case "7":
                        ShowSortOptionsScreen(user);
                        Console.ReadLine();
                        break;
                    case "8":
                        exitMenu = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            } while (!exitMenu);
        }


        #endregion

        #region Screen Task

        private void FilterByStatus(User user)
        {
            Console.Write("Enter Status to Filter: ");
            string status = Console.ReadLine();

            // Fetch tasks from database based on status
            List<TodoTask> filteredTasks = databaseManager.GetTasksForUserAsync(user)
                                                .Result
                                                .Where(task => task.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                                                .ToList();

            // Display filtered tasks
            DisplayFilteredTasks(filteredTasks);
        }

        private void FilterByPriority(User user)
        {
            int priority;
            do
            {
                Console.Write("Enter Priority to Filter (1-5): ");
            } while (!int.TryParse(Console.ReadLine(), out priority) || priority < 1 || priority > 5);

            // Fetch tasks from database based on priority
            List<TodoTask> filteredTasks = databaseManager.GetTasksForUserAsync(user)
                                                .Result
                                                .Where(task => task.Priority == priority)
                                                .ToList();

            // Display filtered tasks
            DisplayFilteredTasks(filteredTasks);
        }

        private void DisplayFilteredTasks(List<TodoTask> tasks)
        {
            Console.WriteLine("Filtered Tasks:");
            foreach (var task in tasks)
            {
                Console.WriteLine($"Title: {task.Title}, Description: {task.Description}, Due Date: {task.DueDate}, Status: {task.Status}, Priority: {task.Priority}");
            }
        }

        private async void SortTasksByTitle(User user)
        {
            // Retrieve tasks for the user from the database
            List<TodoTask> tasks = await databaseManager.GetTasksForUserAsync(user);

            // Sort tasks by title
            tasks.Sort((task1, task2) => task1.Title.CompareTo(task2.Title));

            // Display sorted tasks
            ShowTaskListScreen(tasks);
        }

        private async void SortTasksByDueDate(User user)
        {
            // Retrieve tasks for the user from the database
            List<TodoTask> tasks = await databaseManager.GetTasksForUserAsync(user);

            // Sort tasks by due date
            tasks.Sort((task1, task2) => task1.DueDate.CompareTo(task2.DueDate));

            // Display sorted tasks
            ShowTaskListScreen(tasks);
        }


        private async Task SortTasksByPriority(User user)
        {
            // Retrieve tasks for the user from the database
            List<TodoTask> tasks = await databaseManager.GetTasksForUserAsync(user);

            // Sort tasks by priority
            tasks.Sort((task1, task2) => task1.Priority.CompareTo(task2.Priority));

            // Display sorted tasks
            ShowTaskListScreen(tasks);
        }

        private async Task SortTasksByStatus(User user)
        {
            // Retrieve tasks for the user from the database
            List<TodoTask> tasks = await databaseManager.GetTasksForUserAsync(user);

            // Sort tasks by status
            tasks.Sort((task1, task2) => task1.Status.CompareTo(task2.Status));

            // Display sorted tasks
            ShowTaskListScreen(tasks);
        }

        private void ShowTaskListScreen(List<TodoTask> tasks)
        {
            Console.WriteLine($"Task List:");
            foreach (var task in tasks)
            {
                Console.WriteLine($"Title: {task.Title}, Description: {task.Description}, Due Date: {task.DueDate}");
                // Weitere Informationen zu Aufgaben anzeigen
            }
        }

        public void ShowTaskListScreen(User user)
        {
            Console.WriteLine($"Task List for User: {user.Username}");
            if (user.Tasks.Count > 0)
            {
                foreach (var task in user.Tasks)
                {
                    Console.WriteLine($"- Title: {task.Title}");
                    Console.WriteLine($"  Description: {task.Description}");
                    Console.WriteLine($"  Due Date: {task.DueDate}");
                    Console.WriteLine($"  Status: {task.Status}");
                    Console.WriteLine($"  Priority: {task.Priority}");
                    Console.WriteLine($"  Notes: {task.Notes}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No tasks available.");
            }
        }

        public void ShowAddTaskScreen(User user)
        {
            Console.WriteLine("Add Task Screen");

            Console.Write("Enter Title: ");
            string title = Console.ReadLine();

            Console.Write("Enter Description: ");
            string description = Console.ReadLine();

            DateTime dueDate;
            bool isValidDate = false;
            do
            {
                Console.Write("Enter Due Date (YYYY-MM-DD HH:MM): ");
                string inputDate = Console.ReadLine();
                isValidDate = DateTime.TryParse(inputDate, out dueDate); //Versucht die eingegebene Zeichenfolge in ein DateTime-Objekt zu konvertieren
                if (!isValidDate)
                {
                    Console.WriteLine("Invalid date format. Please enter a valid date in format (YYYY-MM-DD HH:MM).");
                }
            } while (!isValidDate); //Wiederholt den Loop, wenn das Datum ungültig ist


            Console.Write("Enter Status: ");
            string status = Console.ReadLine();

            int priority; // Eingabe der Priorität mit Validierung
            bool isValidPriority = false;
            do
            {
                Console.Write("Enter Priority (1-5): ");
                string inputPriority = Console.ReadLine();
                isValidPriority = int.TryParse(inputPriority, out priority) && priority >= 1 && priority <= 5;
                if (!isValidPriority)
                {
                    Console.WriteLine("Invalid priority. Please enter a number between 1 and 5.");
                }
            } while (!isValidPriority); //Wiederholt den Loop, wenn die Zahl ungültig ist

            Console.Write("Enter Notes: ");
            string notes = Console.ReadLine();

            TodoTask newTask = new TodoTask // Erstellung des neuen Tasks mit den eingegebenen Informationen
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                Status = status,
                Priority = priority,
                Notes = notes,
                Completed = false // Standardmäßig ist der Task nicht abgeschlossen
            };

            // Qualifizieren Sie den Typnamen DatabaseManager vor dem Aufruf der Methode AddTaskAsync
            DatabaseManager.AddTaskAsync(user, newTask);
            ShowNotificationScreen("Task added successfully.");
        }


        public async Task ShowUpdateTaskScreen(User user)
        {
            try
            {
                Console.WriteLine("Update Task Screen");

                // Anzeigen der vorhandenen Aufgaben des Benutzers zur Auswahl
                List<TodoTask> userTasks = await databaseManager.GetTasksForUserAsync(user);
                if (userTasks.Count == 0)
                {
                    Console.WriteLine("No tasks available for update.");
                    return;
                }

                Console.WriteLine("Select Task to Update:");
                for (int i = 0; i < userTasks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {userTasks[i].Title}");
                }
                Console.Write("Enter Task Number: ");
                int selectedTaskIndex;
                if (!int.TryParse(Console.ReadLine(), out selectedTaskIndex) || selectedTaskIndex < 1 || selectedTaskIndex > userTasks.Count)
                {
                    Console.WriteLine("Invalid task number.");
                    return;
                }
                TodoTask selectedTask = userTasks[selectedTaskIndex - 1];

                // Anzeigen der aktuellen Informationen des ausgewählten Tasks
                Console.WriteLine($"Current Task Information:");
                Console.WriteLine($"Title: {selectedTask.Title}");
                Console.WriteLine($"Description: {selectedTask.Description}");
                Console.WriteLine($"Due Date: {selectedTask.DueDate}");
                Console.WriteLine($"Status: {selectedTask.Status}");
                Console.WriteLine($"Priority: {selectedTask.Priority}");
                Console.WriteLine($"Notes: {selectedTask.Notes}");

                // Eingabe neuer Informationen für den ausgewählten Task
                Console.WriteLine($"Update Task: {selectedTask.Title}");
                Console.Write("Enter New Title: ");
                string newTitle = Console.ReadLine();
                Console.Write("Enter New Description: ");
                string newDescription = Console.ReadLine();
                Console.Write("Enter New Due Date (YYYY-MM-DD HH:MM): ");
                DateTime newDueDate;
                while (!DateTime.TryParse(Console.ReadLine(), out newDueDate))
                {
                    Console.WriteLine("Invalid date format. Please enter a valid date in format (YYYY-MM-DD HH:MM).");
                    Console.Write("Enter New Due Date (YYYY-MM-DD HH:MM): ");
                }
                Console.Write("Enter New Status: ");
                string newStatus = Console.ReadLine();
                int newPriority;
                do
                {
                    Console.Write("Enter New Priority (1-5): ");
                } while (!int.TryParse(Console.ReadLine(), out newPriority) || newPriority < 1 || newPriority > 5);
                Console.Write("Enter New Notes: ");
                string newNotes = Console.ReadLine();

                // Aktualisierung des ausgewählten Tasks mit den neuen Informationen
                selectedTask.Title = newTitle;
                selectedTask.Description = newDescription;
                selectedTask.DueDate = newDueDate;
                selectedTask.Status = newStatus;
                selectedTask.Priority = newPriority;
                selectedTask.Notes = newNotes;

                // Aktualisierung des Tasks in der Datenbank
                await databaseManager.UpdateTaskAsync(user, selectedTask);
                ShowNotificationScreen("Task updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during task update: {ex.Message}");
            }
        }

        public async Task ShowDeleteTaskScreen(User user)
        {
            Console.WriteLine("Delete Task Screen");

            // Anzeigen der vorhandenen Aufgaben des Benutzers zur Auswahl
            List<TodoTask> userTasks = await databaseManager.GetTasksForUserAsync(user);
            if (userTasks.Count == 0)
            {
                Console.WriteLine("No tasks available for delete.");
                return;
            }

            Console.WriteLine("Select Task to Delete:");
            for (int i = 0; i < userTasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {userTasks[i].Title}");
            }
            Console.Write("Enter Task Number: ");
            int selectedTaskIndex;
            if (!int.TryParse(Console.ReadLine(), out selectedTaskIndex) || selectedTaskIndex < 1 || selectedTaskIndex > userTasks.Count)
            {
                Console.WriteLine("Invalid task number.");
                return;
            }
            TodoTask selectedTask = userTasks[selectedTaskIndex - 1];

            // Bestätigung des Löschvorgangs
            Console.WriteLine($"Are you sure you want to delete the task: {selectedTask.Title}? (Y/N)");
            string confirmation = Console.ReadLine();
            if (confirmation.ToUpper() == "Y")
            {
                // Löschen des ausgewählten Tasks aus der Datenbank
                await databaseManager.DeleteTaskAsync(user, selectedTask);
                ShowNotificationScreen("Task deleted successfully.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }

        public async Task ShowSearchScreen(User user)
        {
            Console.WriteLine("Search Screen");

            Console.WriteLine("Enter Search Keyword:");
            string keyword = Console.ReadLine();

            // Suche nach Aufgaben des Benutzers, die das Suchkriterium enthalten
            List<TodoTask> userTasks = await databaseManager.GetTasksForUserAsync(user);
            List<TodoTask> searchResults = userTasks.Where(task =>
                task.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                task.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                task.Notes.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            // Anzeigen der Suchergebnisse
            if (searchResults.Count > 0)
            {
                Console.WriteLine($"Search Results for '{keyword}':");
                foreach (var task in searchResults)
                {
                    Console.WriteLine($"Title: {task.Title}, Description: {task.Description}, Due Date: {task.DueDate}");
                    // Weitere Informationen zu Aufgaben anzeigen
                }
            }
            else
            {
                Console.WriteLine("No tasks found matching the search criteria.");
            }
        }


        public void ShowFilterScreen(User user)
        {
            Console.WriteLine("Filter Screen");

            // Anzeigen der verfügbaren Filteroptionen
            Console.WriteLine("Select Filter Options:");
            Console.WriteLine("1. Filter by Status");
            Console.WriteLine("2. Filter by Priority");
            Console.WriteLine("3. Exit");

            Console.Write("Enter Option: ");
            int option;
            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Invalid option. Please enter a valid option number.");
                return;
            }

            switch (option)
            {
                case 1:
                    FilterByStatus(user);
                    break;
                case 2:
                    FilterByPriority(user);
                    break;
                case 3:
                    return; // Exit
                default:
                    Console.WriteLine("Invalid option. Please enter a valid option number.");
                    break;
            }
        }

        public void ShowSortOptionsScreen(User user)
        {
            Console.WriteLine("Sort Options Screen");
            Console.WriteLine("Select sorting criteria:");
            Console.WriteLine("1. Sort by Title");
            Console.WriteLine("2. Sort by Due Date");
            Console.WriteLine("3. Sort by Priority");
            Console.WriteLine("4. Sort by Status");
            Console.WriteLine("5. Back to Main Menu");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // Sort tasks by title
                    SortTasksByTitle(user);
                    break;
                case "2":
                    // Sort tasks by due date
                    SortTasksByDueDate(user);
                    break;
                case "3":
                    // Sort tasks by priority
                    SortTasksByPriority(user);
                    break;
                case "4":
                    // Sort tasks by status
                    SortTasksByStatus(user);
                    break;
                case "5":
                    // Return to main menu
                    RunTaskMenu();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        }

        public void ShowNotificationScreen(string message)
        {
            Console.WriteLine($"Notification: {message}");
        }
        #endregion
    }
}
