using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using DesignEngineer.Models;
using DesignEngineer.Views;
using DesignEngineer.Views.MenuSection;
using Xamarin.Forms;

namespace DesignEngineer.ViewModels
{
    public class UserPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }
        public User User { get; private set; }
        private string passProv { get; set; }
        private string loginProv { get; set; }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public UserPageViewModel()
        {
            User = new User();
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
                    if (Regex.IsMatch(value, @"^[a-zA-Z]+$"))
                    {
                        LoginProv = "";
                        User.Login = value;
                    }
                    else
                    {
                        LoginProv = "Только латиница";
                    }

                    if (value.Length==0)
                    {
                        User.Login = value;
                    }
                    OnPropertyChanged("Login");
                }
            }
        }
        public string LoginProv
        {
            get { return loginProv; }
            set
            {
                if (loginProv != value)
                {
                    

                    loginProv = value;
                    OnPropertyChanged("LoginProv");
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
                    if (value.Length<4)
                    {
                        PassProv = "Пароль должен быть не менее 4 символов";
                    }
                    else
                    {
                        PassProv = "";
                    }
                    OnPropertyChanged("Password");
                }
            }
        }
        public string PassProv
        {
            get { return passProv; }
            set
            {
                if (passProv != value)
                {
                    passProv = value;
                    OnPropertyChanged("PassProv");
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
        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(Login.Trim()) || !string.IsNullOrEmpty(Password.Trim()));
            }
        }
        public ICommand LoginCommand => new Command(async () =>
        {
            if (string.IsNullOrEmpty(Login.Trim()) || string.IsNullOrEmpty(Password.Trim()) ||Password.Length<4)
                await Application.Current.MainPage.DisplayAlert("Внимание!",
                    "Введите данные", "Ok");
            else
            {
                User = App.Database.GetItems().FirstOrDefault(x => x.Login == Login);
                if (User==null)
                {
                    await Application.Current.MainPage.DisplayAlert("Внимание!",
                        "Такого пользователя не существует", "Ok");
                }
                else
                {
                    if (User.Password!=Password)
                    {
                        await Application.Current.MainPage.DisplayAlert("Внимание!",
                            "Пароль не верен", "Ok");
                    }
                    else
                    {
                        IsActivity = true;
                        App.Database.SaveItem(User);
                        await this.Navigation.PopAsync();
                    }
                }
            }
        });
        public ICommand CancelCommand => new Command(async () =>
        {
            await this.Navigation.PopAsync();
        });
        public ICommand RegCommand => new Command(async () =>
        {
            await Navigation.PushAsync(new RegistrPage());
        });
        public ICommand SaveCommand => new Command(async () =>
        {
            
            if (string.IsNullOrEmpty(Login.Trim()) || string.IsNullOrEmpty(Password.Trim()) || Password.Length < 4)
                await Application.Current.MainPage.DisplayAlert("Внимание!",
                    "Введите данные", "Ok");
            else
            {
                if (App.Database.GetItems().FirstOrDefault(x => x.Login == Login) != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Внимание!",
                        "Такой пользователь уже существует", "Ok");
                }
                else
                {
                   /* User = new User();
                    User.Login = Login;
                    User.Password = Password;*/
                    IsActivity = true;
                    Theme = "Light";
                    App.Database.SaveItem(User);
                    await Application.Current.MainPage.DisplayAlert("",
                            "Поздравляю с регистрацией", "Ok");
                    await this.Navigation.PopAsync();
                    await this.Navigation.PopAsync();
                }
            }
        });

        public async System.Threading.Tasks.Task ChangeAccAsync(string param)
        {
            User = App.Database.GetItems().FirstOrDefault(x => x.IsActivity);
            switch (param)
            {
                case "Exit":
                    bool result = await Application.Current.MainPage.DisplayAlert("Внимание!",
                        "Вы действительно хотите выйти из профиля?", "Ok", "Отмена");
                    if (result)
                    {
                        IsActivity = false;
                        App.Database.SaveItem(User);
                    }
                    break;
                case "Change":
                    bool result2 = await Application.Current.MainPage.DisplayAlert("Внимание!",
                        "Вы действительно хотите сменить профиль?", "Ok", "Отмена");
                    if (result2)
                    {
                        IsActivity = false;
                        App.Database.SaveItem(User);
                    }
                    break;
                case "Delete":
                    bool result3 = await Application.Current.MainPage.DisplayAlert("Внимание!",
                        "Вы действительно хотите удалить профиль?", "Ok", "Отмена");
                    if (result3)
                    {
                        App.Database.DeleteItem(User.UserId);
                    }
                    break;
            }
        }
    }
}
