using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DesignEngineer.Models;
using DesignEngineer.Views;
using Xamarin.Forms;

namespace DesignEngineer.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }
        public User User { get; private set; }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public SettingsViewModel()
        {
            User = App.Database.GetItems().FirstOrDefault(x=>x.IsActivity);
        }
        public int UserId
        {
            get { return User.UserId; }
            set
            {
                if (User.UserId != value)
                {
                    User.UserId = value;
                    OnPropertyChanged("UserId");
                }
            }
        }
        public string Login
        {
            get { return User.Login; }
            set
            {
                if (User.Login != value)
                {
                    User.Login = value;
                    OnPropertyChanged("Login");
                }
            }
        }
        public string Password
        {
            get { return User.Password; }
            set
            {
                if (User.Password != value)
                {
                    User.Password = value;
                    OnPropertyChanged("Password");
                }
            }
        }
        public bool IsActivity
        {
            get { return User.IsActivity; }
            set
            {
                if (User.IsActivity != value)
                {
                    User.IsActivity = value;
                    OnPropertyChanged("IsActivity");
                }
            }
        }
        public string Theme
        {
            get { return User.Theme; }
            set
            {
                if (User.Theme != value)
                {
                    User.Theme = value;
                    OnPropertyChanged("Theme");
                }
            }
        }
        public ICommand LeaveProfCommand => new Command(async () =>
        {
            bool result = await Application.Current.MainPage.DisplayAlert("Внимание!",
                "Вы действительно хотите выйти из профиля?", "Ok", "Отмена");

                    if (result)
                    {
                        User = App.Database.GetItems().FirstOrDefault(x => x.IsActivity);
                        IsActivity = false;
                        App.Database.SaveItem(User);
                        await this.Navigation.PopAsync();
            }
        });
    }
}
