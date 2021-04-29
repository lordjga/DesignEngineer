using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DesignEngineer.Models;
using DesignEngineer.Views;
using DesignEngineer.Views.MenuSection;
using Xamarin.Forms;

namespace DesignEngineer.ViewModels
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }
        public Report Report { get; private set; }
        public User user { get; private set; }
        Report selectedReport;
        
        private ObservableCollection<Report> reportResults;

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public ReportViewModel()
        {
            Report = new Report();
            //reportResults = ReportResultsCreate();
        }
        public int IdR
        {
            get { return Report.IdR; }
            set
            {
                if (Report.IdR != value)
                {
                    Report.IdR = value;
                    OnPropertyChanged("IdR");
                }
            }
        }
        public int UserId
        {
            get { return Report.UserId; }
            set
            {
                if (Report.UserId != value)
                {
                    Report.UserId = value;
                    OnPropertyChanged("UserId");
                }
            }
        }

        public string Name
        {
            get { return Report.Name; }
            set
            {
                if (Report.Name != value)
                {
                    Report.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public string Title
        {
            get { return Report.Title; }
            set
            {
                if (Report.Title != value)
                {
                    Report.Title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        
        public ObservableCollection<Report> ReportResults
        {
            get
            {
                return reportResults;
            }
            set
            {
                if (reportResults != value)
                {
                    reportResults = value;
                    OnPropertyChanged("ReportResults");
                }
                
            }
        }
        public Report SelectedReport
        {
            get { return selectedReport; }
            set
            {
                if (selectedReport != value)
                {
                    Report temp = value;
                    selectedReport = null;
                    
                    Navigation.PushAsync(new ReportView(temp));
                    OnPropertyChanged("SelectedReport");
                    ReportResults = ReportResultsCreate();

                }
            }
        }
        
        
        public ObservableCollection<Report> ReportResultsCreate()
        {
            user = App.Database.GetItems().FirstOrDefault(x => x.IsActivity);
            ObservableCollection<Report> reportRes =  new ObservableCollection<Report>(App.DatabaseReport.GetItems().Where(x => x.UserId == user.UserId));
            return reportRes;
        }


    }
}
