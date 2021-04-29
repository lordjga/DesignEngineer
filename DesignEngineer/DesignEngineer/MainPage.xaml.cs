using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.Models;
using DesignEngineer.ViewModels;
using DesignEngineer.Views.CalculationsSection;
using DesignEngineer.Views.GenTechServSection;
using DesignEngineer.Views.MenuSection;
using Xamarin.Forms;

namespace DesignEngineer
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new XamlCommands() { Navigation = this.Navigation };
            OnStartApp();
        }
        
        public async void OnStartApp()
        {
            if (App.Database.GetItems().Count() == 0)
            {
                bool result = await DisplayAlert("Внимание!", "Для того, чтобы пользоваться настройками, " +
                    "вам небходимо создать свой профиль", "Ok", "Отмена");
                if (result) 
                    await this.Navigation.PushAsync(new RegistrPage());
                else await this.Navigation.PopAsync();
            }
            User user = App.Database.GetItems().FirstOrDefault(x => x.IsActivity);
            if (App.Database.GetItems().Any() && user == null)
            {
                bool result = await DisplayAlert("Внимание!", "Для того, чтобы пользоваться настройками, " +
                    "вам небходимо войти в свой профиль", "Ok", "Отмена");
                if (result) await this.Navigation.PushAsync(new LoginPage());
                else await this.Navigation.PopAsync();
            }
        }
        
    }
}
