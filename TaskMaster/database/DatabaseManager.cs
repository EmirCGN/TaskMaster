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
        public void CreateUser(string username, string password)
        {

        }

        public void UpdateUser(User user)
        {

        }
        public void DeleteUser(string username)
        {
        }

        public User GetUserByUsername(string username)
        {
            return null;
        }

    }
}
