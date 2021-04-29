using System;
using System.Collections.Generic;
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
    public partial class BookmarkPage : ContentPage
    {
        public BookmarkPage()
        {
            InitializeComponent();
            BindingContext = new BookmarkViewModel() { Navigation = this.Navigation };
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
                BookArtList.ItemsSource = ArticleCollection.GetBookmArtResults();
                BookArtList.SelectedItem = null;
            }
            catch (Exception e)
            {
                
            }
            

            base.OnAppearing();
        }
    }
}