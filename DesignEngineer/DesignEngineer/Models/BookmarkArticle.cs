using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DesignEngineer.Models
{
    [Table("BookmarkArticle")]
    public class BookmarkArticle
    {
        
        public int UserId { get; set; }
        public int IdColl { get; set; }
        [PrimaryKey, AutoIncrement, Column("Id")] //, ForeignKey(typeof(User))
        public int Id { get; set; }
        /*  [ManyToOne]
          public User User { get; set; }*/
    }
}
