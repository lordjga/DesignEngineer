using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.Models;
using DesignEngineer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DesignEngineer.Views.MenuSection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportView : ContentPage
    {
        public Report Model { get; private set; }
        public ReportView(Report m)
        {
            InitializeComponent();
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Model = m;
            BindingContext =  new XamlCommands() { Navigation = this.Navigation };
            Label1.Text = File.ReadAllText(Path.Combine(folderPath, Model.Name));
            //ReportView.TitleProperty = 
            Title = Model.Title;
        }

        private async void ImageButton_OnClickedAsync(object sender, EventArgs e)
        {
            bool result = await Application.Current.MainPage.DisplayAlert("Внимание!", "Удалить отчет?", "Ok", "Отмена");
            if (result)
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                File.Delete(Path.Combine(folderPath, Model.Name));
                App.DatabaseReport.DeleteItem(Model.IdR);
                 Navigation.PopAsync();
            }
        }
    }
}