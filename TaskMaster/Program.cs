using System.Diagnostics;
using TaskMaster.database;
using TaskMaster.ui;

class Program
{
    static void Main(string[] args)
    {

        using(var dbContext = new DatabaseManager())
        {
            dbContext.InitializeDatabase();
        }

        DatabaseManager dbManager = new DatabaseManager();
        UserInterface userInterface = new UserInterface(null);

        UserInterface.RunMainMenu();
    }
}