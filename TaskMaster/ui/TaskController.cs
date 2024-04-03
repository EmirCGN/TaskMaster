using TaskMaster.database;
using TaskMaster.model;

namespace TaskMaster.ui
{
    public class TaskController
    {
        private readonly DatabaseManager databaseManager;
        private readonly UserInterface userInterface;

        public TaskController(DatabaseManager dbManager, UserInterface ui)
        {
            databaseManager = dbManager;
            userInterface = ui;
        }

        public void HandleLogin()
        {
            userInterface.ShowLoginScreen();
        }

        public void HandleRegistration()
        {
            userInterface.ShowRegistrationScreen();
        }

        public void HandleTaskList(User user)
        {
            userInterface.ShowTaskListScreen(user);
        }

        public void HandleAddTask(User user)
        {
            userInterface.ShowAddTaskScreen(user);
        }

        public void HandleUpdateTask(User user)
        {
            userInterface.ShowUpdateTaskScreen(user);
        }

        public void HandleDeleteTask(User user)
        {
            userInterface.ShowDeleteTaskScreen(user);
        }

        public void HandleSearchTasks(User user)
        {
            userInterface.ShowSearchScreen(user);
        }

        public void HandleFilterTasks(User user)
        {
            userInterface.ShowFilterScreen(user);
        }

        public void HandleSortTasks(User user)
        {
            userInterface.ShowSortOptionsScreen(user);
        }

        public void HandleTaskCompletion(User user, TodoTask task)
        {
            // Implementierung der Aufgabenabschlusslogik
            userInterface.ShowNotificationScreen($"Task '{task.Title}' marked as completed.");
        }

        public void HandleTaskPriority(User user, TodoTask task)
        {
            // Implementierung der Aufgabenprioritätslogik
            userInterface.ShowNotificationScreen($"Priority updated for task '{task.Title}'.");
        }

        public void HandleTaskNotes(User user, TodoTask task)
        {
            // Implementierung der Aufgabennotizlogik
            userInterface.ShowNotificationScreen($"Notes updated for task '{task.Title}'.");
        }
    }
}
