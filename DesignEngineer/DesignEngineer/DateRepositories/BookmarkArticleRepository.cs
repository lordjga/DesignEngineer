using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.Models;
using SQLite;

namespace DesignEngineer
{
    public class BookmarkArticleRepository
    {
        SQLiteConnection database;
        public BookmarkArticleRepository(string databasePath)
        {
            database = new SQLiteConnection(databasePath);
            database.CreateTable<BookmarkArticle>();
        }
        public IEnumerable<BookmarkArticle> GetItems()
        {
            return database.Table<BookmarkArticle>().ToList();
        }
        public BookmarkArticle GetItem(int id)
        {
            return database.Get<BookmarkArticle>(id);
        }
        public int DeleteItem(int id)
        {
            return database.Delete<BookmarkArticle>(id);
        }
        public int SaveItem(BookmarkArticle item)
        {
            if (item.Id != 0)
            {
                database.Update(item);
                return item.Id;
            }
            else
            {
                return database.Insert(item);
            }
        }
    }
}
