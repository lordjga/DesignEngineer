using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.Models;
using DesignEngineer.Themes;
using DesignEngineer.ViewModels;
using ThemingDemo;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DesignEngineer.Views.MenuSection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private User user;
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new XamlCommands() { Navigation = this.Navigation };
           // BindingContext = new SettingsViewModel() { Navigation = this.Navigation };

            user = App.Database.GetItems().FirstOrDefault(x => x.IsActivity);
            
            if (user!=null)
            {
                statusLabel.Text = $"{user.Theme} theme loaded.";
            }
        }

        void OnPickerSelectionChanged(object sender, EventArgs e)
        {
            bool isActivity = App.Database.GetItems().Any(x => x.IsActivity);
            
            Picker picker = sender as Picker;
            Theme theme = (Theme)picker.SelectedItem;   // (Theme)Enum.Parse(typeof(Theme),"Dark");
            Preferences.Set("theme", theme.ToString());
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (theme)
                {
                    case Theme.Dark:
                        if (user!=null)
                        {
                            user.Theme= "Dark";
                            App.Database.SaveItem(user);
                        }
                        mergedDictionaries.Add(new DarkTheme());
                        break;
                    case Theme.Light:
                    default:
                        if (user!=null)
                        {
                            user.Theme = "Light";
                            App.Database.SaveItem(user);
                        }
                        mergedDictionaries.Add(new LightTheme());
                        break;
                }
                statusLabel.Text = $"{theme.ToString()} theme loaded.";
            }
        }
        protected override async void OnAppearing()
        {
            
            if (App.Database.GetItems().Count() == 0)
            {
                bool result = await Application.Current.MainPage.DisplayAlert("Внимание!", "Для того, чтобы пользоваться настройками, " +
                                                                             "вам небходимо создать свой профиль", "Ok", "Отмена");
                if (result) await Navigation.PushAsync(new RegistrPage());
                else await Navigation.PopAsync();
            }
            User user = App.Database.GetItems().FirstOrDefault(x => x.IsActivity);
            if (App.Database.GetItems().Any()&&user == null)
            {
                bool result = await Application.Current.MainPage.DisplayAlert("Внимание!", "Для того, чтобы пользоваться настройками, " +
                                                                             "вам небходимо войти в свой профиль", "Ok", "Отмена");
                if (result) await Navigation.PushAsync(new LoginPage());
                else await Navigation.PopAsync();
            }
            
            if (App.Database.GetItems().FirstOrDefault(x => x.IsActivity) != null)
            {
                login.Text = "Приветствую, " + App.Database.GetItems().FirstOrDefault(x => x.IsActivity).Login.ToString();
            }
            
            base.OnAppearing();
        }
    }
}