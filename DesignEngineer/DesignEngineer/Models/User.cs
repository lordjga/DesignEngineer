using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DesignEngineer.Models
{
    [Table("User")]
    public class User
    {
        [PrimaryKey, AutoIncrement, Column("UserId")]
        public int UserId { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsActivity { get; set; }
        public string Theme { get; set; }
        /*
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<BookmarkArticle> BookmarkArticles { get; set; }*/
        // public List<BookmarkArticle> BookmarkArticles { get; set; }
    }
}
