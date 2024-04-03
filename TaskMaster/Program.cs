using System.Diagnostics;
using TaskMaster.database;
using TaskMaster.ui;

class Program
{
    static void Main(string[] args)
    {
        DatabaseManager dbManager = new DatabaseManager();
        UserInterface userInterface = new UserInterface();
        TaskController taskController = new TaskController(dbManager, userInterface); 

        userInterface.RunMainMenu();
    }
}