using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.Models;
using SQLite;

namespace DesignEngineer
{
    public class UserRepository
    {
        SQLiteConnection database;
        public UserRepository(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<User>();
        }
        public IEnumerable<User> GetItems()
        {
            return database.Table<User>().ToList();
        }
        public User GetItem(int id)
        {
            return database.Get<User>(id);
        }
        public int DeleteItem(int id)
        {
            return database.Delete<User>(id);
        }
        public int SaveItem(User item)
        {
            if (item.UserId != 0)
            {
                database.Update(item);
                return item.UserId;
            }
            else
            {
                return database.Insert(item);
            }
        }
    }
}
