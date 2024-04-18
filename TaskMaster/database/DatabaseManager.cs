using Microsoft.Data.Sqlite; 
using Microsoft.EntityFrameworkCore; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using TaskMaster.model; 

namespace TaskMaster.database 
{
    public class DatabaseManager : DbContext 
    {
        public static string connectionString { get; set; } = @"Data Source=C:\Users\korog\source\repos\TaskMaster\TaskMaster\database\TaskDB.db";
        public DbSet<User> Users { get; set; } = null; 

        public void InitializeDatabase() 
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString)) // Verbindung zur Datenbank herstellen
            {
                connection.Open(); // Verbindung öffnen bzw. starten
                CreateTablesIfNotExists(connection); // Ruft die Methode ab
            }
        }

        public async Task CreateTablesIfNotExists(SqliteConnection connection) // Asynchrone Methode zum Erstellen von Tabellen, falls sie nicht existieren
        {
            string usersTableQuery = @"
        CREATE TABLE IF NOT EXISTS Users (
            Username TEXT PRIMARY KEY,
            Password TEXT NOT NULL
        );"; // SQL-Abfrage zum Erstellen der Benutzer-Tabelle

            using (SqliteCommand command = new SqliteCommand(usersTableQuery, connection))
            {
                command.ExecuteNonQuery(); // SQL Befehl ausführen
            }

            string tasksTableQuery = @"
            CREATE TABLE IF NOT EXISTS Tasks (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId TEXT,
            Title TEXT NOT NULL,
            Description TEXT,
            DueDate DATETIME,
            Status TEXT,
            Priority INTEGER,
            Notes TEXT,
            Completed INTEGER,
            FOREIGN KEY (UserId) REFERENCES Users(Username) ON DELETE CASCADE
             );"; // SQL-Abfrage zum Erstellen der Task-Tabelle

            using (SqliteCommand command = new SqliteCommand(tasksTableQuery, connection))
            {
                command.ExecuteNonQuery(); // SQL Befehl ausführen
            }
        }

        public static async Task SaveUserAsync(string username, string password) // Asynchrone Methode zum Speichern eines Benutzers
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString)) // Verbindung zur Datenbank herstellen
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";// SQL-Abfrage zum Einfügen eines Benutzers
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username); // Benutzernamen-Parameter hinzufügen
                    command.Parameters.AddWithValue("@Password", password);// Passwort-Parameter hinzufügen
                    await command.ExecuteNonQueryAsync();// SQL-Befehl ausführen
                }
            }
        }

        public static async Task<bool> CheckLoginAsync(string username, string password)// Asynchrone Methode zum Überprüfen des Logins
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))// Verbindung zur Datenbank herstellen
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";// SQL-Abfrage zum Zählen der Benutzer
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    object result = await command.ExecuteScalarAsync();// SQL-Befehl ausführen und Ergebnis abrufen
                    if (result != null && result != DBNull.Value)// Wenn das Ergebnis gültig ist
                    {
                        long count = (long)result;// Ergebnis in eine Ganzzahl konvertieren
                        return count > 0;// Überprüfen, ob die Anzahl größer als Null ist
                    }
                    return false;// Andernfalls falsch zurückgeben
                }
            }
        }

        public static async Task DeleteUserAsync(string username)// Asynchrone Methode zum Löschen eines Benutzers
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))// Verbindung zur Datenbank herstellen
            {
                await connection.OpenAsync();
                string query = "DELETE FROM Users WHERE Username = @Username";// SQL-Abfrage zum Löschen eines Benutzers
                using (SqliteCommand command = new SqliteCommand(query, connection))// SQL-Befehl erstellen
                {
                    command.Parameters.AddWithValue("@Username", username);// Benutzernamen-Parameter hinzufügen
                    await command.ExecuteNonQueryAsync();// SQL-Befehl ausführen
                }
            }
        }

        public static async Task<User> GetUserByUsername(string username)// Asynchrone Methode zum Abrufen eines Benutzers nach Benutzernamen
        {
            return null;
        }

        #region Task Management
        public static async Task AddTaskAsync(User user, TodoTask task)// Asynchrone Methode zum Hinzufügen einer Aufgabe
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))// Verbindung zur Datenbank herstellen
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Tasks (Username, Title, Description, DueDate, Status, Priority, Notes, Completed) VALUES (@Username, @Title, @Description, @DueDate, @Status, @Priority, @Notes, @Completed)";// SQL-Abfrage zum Einfügen des Tasks
                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    // Die ganzen Parameter werden hier hinzugefügt
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@Description", task.Description);
                    command.Parameters.AddWithValue("@DueDate", task.DueDate);
                    command.Parameters.AddWithValue("@Status", task.Status);
                    command.Parameters.AddWithValue("@Priority", task.Priority);
                    command.Parameters.AddWithValue("@Notes", task.Notes);
                    command.Parameters.AddWithValue("@Completed", task.Completed);
                    await command.ExecuteNonQueryAsync(); // SQL-Befehl ausführen
                }
            }
        }

        public async Task UpdateTaskAsync(User user, TodoTask task) // Asynchrone Methode zum Aktualisieren einer Aufgabe
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString)) // Verbindung zur Datenbank herstellen
            {
                await connection.OpenAsync(); // Verbindung öffnen
                string query = "UPDATE Tasks SET Title = @Title, Description = @Description, DueDate = @DueDate, Status = @Status, Priority = @Priority, Notes = @Notes, Completed = @Completed WHERE Username = @Username AND Title = @Title"; // SQL-Abfrage zum Aktualisieren einer Aufgabe
                using (SqliteCommand command = new SqliteCommand(query, connection)) // SQL-Befehl erstellen
                {
                    command.Parameters.AddWithValue("@Username", user.Username); // Benutzernamen-Parameter hinzufügen
                    command.Parameters.AddWithValue("@Title", task.Title); // Titel-Parameter hinzufügen
                    command.Parameters.AddWithValue("@Description", task.Description); // Beschreibungs-Parameter hinzufügen
                    command.Parameters.AddWithValue("@DueDate", task.DueDate); // Fälligkeitsdatum-Parameter hinzufügen
                    command.Parameters.AddWithValue("@Status", task.Status); // Status-Parameter hinzufügen
                    command.Parameters.AddWithValue("@Priority", task.Priority); // Prioritäts-Parameter hinzufügen
                    command.Parameters.AddWithValue("@Notes", task.Notes); // Notizen-Parameter hinzufügen
                    command.Parameters.AddWithValue("@Completed", task.Completed); // Abgeschlossenheits-Parameter hinzufügen
                    await command.ExecuteNonQueryAsync(); // SQL-Befehl ausführen
                }
            }
        }

        public async Task DeleteTaskAsync(User user, TodoTask task) // Asynchrone Methode zum Löschen einer Aufgabe
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString)) // Verbindung zur Datenbank herstellen
            {
                await connection.OpenAsync(); // Verbindung öffnen
                string query = "DELETE FROM Tasks WHERE Username = @Username AND Title = @Title"; // SQL-Abfrage zum Löschen einer Aufgabe
                using (SqliteCommand command = new SqliteCommand(query, connection)) // SQL-Befehl erstellen
                {
                    command.Parameters.AddWithValue("@Username", user.Username); // Benutzernamen-Parameter hinzufügen
                    command.Parameters.AddWithValue("@Title", task.Title); // Titel-Parameter hinzufügen
                    await command.ExecuteNonQueryAsync(); // SQL-Befehl ausführen
                }
            }
        }

        public async Task<List<TodoTask>> GetTasksForUserAsync(User user) // Asynchrone Methode zum Abrufen von Aufgaben für einen Benutzer
        {
            List<TodoTask> tasks = new List<TodoTask>(); // Neue Liste für Aufgaben erstellen
            using (SqliteConnection connection = new SqliteConnection(connectionString)) // Verbindung zur Datenbank herstellen
            {
                await connection.OpenAsync(); // Verbindung öffnen
                string query = "SELECT * FROM Tasks WHERE Username = @Username"; // SQL-Abfrage zum Abrufen von Aufgaben für einen Benutzer
                using (SqliteCommand command = new SqliteCommand(query, connection)) // SQL-Befehl erstellen
                {
                    command.Parameters.AddWithValue("@Username", user.Username); // Benutzernamen-Parameter hinzufügen
                    using (SqliteDataReader reader = await command.ExecuteReaderAsync()) // SQL-Befehl ausführen und Ergebnis abrufen
                    {
                        while (await reader.ReadAsync()) // Loop wird solange es Daten zu lesen gibt laufen
                        {
                            TodoTask task = new TodoTask // Neue Aufgabe erstellen und initialisieren
                            {
                                Title = reader["Title"].ToString(), // Titel auslesen und setzen
                                Description = reader["Description"].ToString(), // Beschreibung auslesen und setzen
                                DueDate = Convert.ToDateTime(reader["DueDate"]), // Fälligkeitsdatum auslesen und konvertieren
                                Status = reader["Status"].ToString(), // Status auslesen und setzen
                                Priority = Convert.ToInt32(reader["Priority"]), // Priorität auslesen und konvertieren
                                Notes = reader["Notes"].ToString(), // Notizen auslesen und setzen
                                Completed = Convert.ToBoolean(reader["Completed"]) // Abgeschlossenheitsstatus auslesen und konvertieren
                            };
                            tasks.Add(task); // Aufgabe zur Liste hinzufügen
                        }
                    }
                }
            }
            return tasks; // Liste mit Aufgaben zurückgeben
        }
        #endregion
    }
}
