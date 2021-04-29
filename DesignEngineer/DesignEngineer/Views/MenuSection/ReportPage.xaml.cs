using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class ReportPage : ContentPage
    {
        public ReportPage()
        {
            InitializeComponent();
            BindingContext = new ReportViewModel() { Navigation = this.Navigation };
            BindingContext = new XamlCommands() { Navigation = this.Navigation };
        }
        protected override async void OnAppearing()
        {
            try
            {
                if (App.Database.GetItems().Count() == 0)
                {
                    bool result = await Application.Current.MainPage.DisplayAlert("Внимание!", "Для того, чтобы пользоваться настройками, " +
                        "вам небходимо создать свой профиль", "Ok", "Отмена");
                    if (result) await Navigation.PushAsync(new RegistrPage());
                    else await Navigation.PopAsync();
                }
                User user = App.Database.GetItems().FirstOrDefault(x => x.IsActivity);
                if (App.Database.GetItems().Any() && user == null)
                {
                    bool result = await Application.Current.MainPage.DisplayAlert("Внимание!", "Для того, чтобы пользоваться настройками, " +
                        "вам небходимо войти в свой профиль", "Ok", "Отмена");
                    if (result) await Navigation.PushAsync(new LoginPage());
                    else await Navigation.PopAsync();
                }
                ObservableCollection<Report> reportRes = new ObservableCollection<Report>(App.DatabaseReport.GetItems().Where(x => x.UserId == user.UserId));
                BookArtList.ItemsSource = reportRes;
            }
            catch (Exception e)
            {
                
            }
            base.OnAppearing();
        }

        public void DeleteCommand(int id)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var top = App.DatabaseReport.GetItem(id);
            File.Delete(Path.Combine(folderPath, top.Name));
            App.DatabaseReport.DeleteItem(id);
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            
            
        }
    }
}