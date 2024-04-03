using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.model
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<TodoTask> Tasks { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Tasks = new List<TodoTask>();
        }
    }
}
