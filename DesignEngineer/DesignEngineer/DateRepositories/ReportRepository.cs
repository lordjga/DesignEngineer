using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.Models;
using SQLite;

namespace DesignEngineer
{
    public class ReportRepository
    {
        SQLiteConnection database;
        public ReportRepository(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<Report>();
        }
        public IEnumerable<Report> GetItems()
        {
            return database.Table<Report>().ToList();
        }
        public Report GetItem(int id)
        {
            return database.Get<Report>(id);
        }
        public int DeleteItem(int id)
        {
            return database.Delete<Report>(id);
        }
        public int SaveItem(Report item)
        {
            if (item.IdR != 0)
            {
                database.Update(item);
                return item.IdR;
            }
            else
            {
                return database.Insert(item);
            }
        }
    }
}
