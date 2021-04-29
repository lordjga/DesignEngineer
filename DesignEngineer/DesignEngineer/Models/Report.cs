using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using DesignEngineer.ViewModels;
using DesignEngineer.Views.MenuSection;
using SQLite;
using SQLiteNetExtensions.Attributes;
using Xamarin.Forms;

namespace DesignEngineer.Models
{
    [Table("Report")]
    public class Report
    {
        [PrimaryKey, AutoIncrement, Column("IdR") ]
        public int IdR { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        
       
    }
}
