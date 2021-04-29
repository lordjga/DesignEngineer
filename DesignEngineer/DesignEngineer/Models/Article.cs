using System.ComponentModel;
using Xamarin.Forms;

namespace DesignEngineer.Models
{
    public class Article //: INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public WebViewSource WebViewSource { get; set; }
        public string Title { get; set; }
        public string Discription { get; set; }

    }
}