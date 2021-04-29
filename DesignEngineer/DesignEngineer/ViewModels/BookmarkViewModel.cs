using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DesignEngineer.Models;
using DesignEngineer.Views;
using DesignEngineer.Views.CalculationsSection;
using DesignEngineer.Views.CalculationsSection.RackCalc;
using DesignEngineer.Views.CalculationsSection.SpringsCalc;
using DesignEngineer.Views.MenuSection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DesignEngineer.ViewModels
{
    public class BookmarkViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }
        public BookmarkArticle BookmarkArticle { get; private set; }

        ArticleViewModel selectedArticle;

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public BookmarkViewModel()
        {
            BookmarkArticle = new BookmarkArticle();
            Navigation = this.Navigation;
            //BookmArtResults = BookmArtResultsCreate();
        }

        public int Id
        {
            get { return BookmarkArticle.Id; }
            set
            {
                if (BookmarkArticle.Id != value)
                {
                    BookmarkArticle.Id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public int UserId
        {
            get { return BookmarkArticle.UserId; }
            set
            {
                if (BookmarkArticle.UserId != value)
                {
                    BookmarkArticle.UserId = value;
                    OnPropertyChanged("UserId");
                }
            }
        }

        public int IdColl
        {
            get { return BookmarkArticle.IdColl; }
            set
            {
                if (BookmarkArticle.IdColl != value)
                {
                    BookmarkArticle.IdColl = value;
                    OnPropertyChanged("IdColl");
                }
            }
        }

        public ArticleViewModel SelectedArticle
        {
            get { return selectedArticle; }
            set
            {
                if (selectedArticle != value)
                {
                    ArticleViewModel tempArticle = value;
                    selectedArticle = null;

                    if (tempArticle.WebViewSource == null)
                    {
                        switch (tempArticle.Name)
                        {
                            case "ConverterCalc":
                                Navigation.PushAsync(new ConverterCalc());
                                break;
                            case "CalcOfHelicalCylSpringsPage":
                                Navigation.PushAsync(new CalcOfHelicalCylSpringsPage());
                                break;
                            case "CalcRackPage":
                                Navigation.PushAsync(new CalcRackPage());
                                break;
                        }
                        OnPropertyChanged("SelectedArticle");
                    }
                    else
                    {
                        OnPropertyChanged("SelectedArticle");
                        Navigation.PushAsync(new ArticlePage(tempArticle));
                        
                    }

                    
                    //BookmArtResults = BookmArtResultsCreate();
                }
            }
        }

        private ObservableCollection<ArticleViewModel> bookmArtResults;// = ArticleCollection.GetBookmArtResults();
        /*
        public ObservableCollection<ArticleViewModel> BookmArtResults
        {
            get { return bookmArtResults; }
            set
            {
                if (bookmArtResults != value)
                {
                    bookmArtResults = value;
                    OnPropertyChanged("BookmArtResults");
                }
            }
        }
        */
        public ObservableCollection<ArticleViewModel> BookmArtResultsCreate()
        {
            ObservableCollection<ArticleViewModel> bookmArtResult = ArticleCollection.GetBookmArtResults();
            return bookmArtResult;
        }

        public async System.Threading.Tasks.Task CreateAsync(int idcoll)
        {
            User user = App.Database.GetItems().FirstOrDefault(x => x.IsActivity);
            if (user != null)
            {
                var item2 = App.DatabaseBookm.GetItems()
                    .Where(x => x.UserId == user.UserId)
                    .FirstOrDefault(y => y.IdColl == idcoll);
                if (item2 != null)
                {
                    bool result = await Application.Current.MainPage.DisplayAlert("Внимание!",
                        "Вы уже добавляли эту страницу в закладки, удалить?",
                        "Ok", "Отмена");
                    if (result)
                    {
                        App.DatabaseBookm.DeleteItem(item2.Id);
                        await Application.Current.MainPage.DisplayAlert("Внимание!", "закладка успешно удалена",
                            "Ok");
                    }
                }
                else
                {
                    UserId = user.UserId;
                    IdColl = idcoll;
                    App.DatabaseBookm.SaveItem(BookmarkArticle);
                    await Application.Current.MainPage.DisplayAlert("Внимание!", "закладка успешно добавлена",
                        "Ok");
                }
            }
            else
            {
               // Navigation = new NavigationProxy();
                if (App.Database.GetItems().Count() == 0)
                {
                    bool result = await Application.Current.MainPage.DisplayAlert("Внимание!",
                        "Для того, чтобы пользоваться настройками, " +
                        "вам небходимо создать свой профиль", "Ok", "Отмена");
                    if (result) await Navigation.PushAsync(new RegistrPage());
                    else await Navigation.PopAsync();
                }
                if (App.Database.GetItems().Any() && user == null)
                {
                    bool result = await Application.Current.MainPage.DisplayAlert("Внимание!",
                        "Для того, чтобы пользоваться настройками, " +
                        "вам небходимо войти в свой профиль", "Ok", "Отмена");
                    if (result) await Navigation.PushAsync(new LoginPage());
                    else await Navigation.PopAsync();
                }
            }
            //BookmArtResults = BookmArtResultsCreate();
        }
        public async System.Threading.Tasks.Task DeleteAsync(int idcoll)
        {
            User user = App.Database.GetItems().FirstOrDefault(x => x.IsActivity);
            if (user != null)
            {
                var item2 = App.DatabaseBookm.GetItems()
                    .Where(x => x.UserId == user.UserId)
                    .FirstOrDefault(y => y.IdColl == idcoll);
                if (item2 != null)
                {
                    bool result = await Application.Current.MainPage.DisplayAlert("Внимание!", "Удалить?",
                        "Ok", "Отмена");
                    if (result)
                    {
                        App.DatabaseBookm.DeleteItem(item2.Id);
                        await Application.Current.MainPage.DisplayAlert("Внимание!", "закладка успешно удалена",
                            "Ok");
                        //BookmArtResults = bookmArtResults;
                        OnPropertyChanged("BookmArtResults");
                        OnPropertyChanged("SelectedArticle");
                    }
                }

            }
            //BookmArtResults = BookmArtResultsCreate();

        }

        public async System.Threading.Tasks.Task CreateCalcAsync(string name)
        {
            User user = App.Database.GetItems().FirstOrDefault(x => x.IsActivity);
            if (user != null)
            {
                int idcoll = ArticleCollection.Articles.FirstOrDefault(x => x.Name == name).IdA;
                var item2 = App.DatabaseBookm.GetItems()
                    .Where(x => x.UserId == user.UserId)
                    .FirstOrDefault(y => y.IdColl == idcoll);
                if (item2 != null)
                {
                    bool result = await Application.Current.MainPage.DisplayAlert("Внимание!",
                        "Вы уже добавляли эту страницу в закладки, удалить?",
                        "Ok", "Отмена");
                    if (result)
                    {
                        App.DatabaseBookm.DeleteItem(item2.Id);
                        await Application.Current.MainPage.DisplayAlert("Внимание!", "закладка успешно удалена",
                            "Ok");
                    }
                }
                else
                {
                    UserId = user.UserId;
                    IdColl = idcoll;
                    App.DatabaseBookm.SaveItem(BookmarkArticle);
                    await Application.Current.MainPage.DisplayAlert("Внимание!", "закладка успешно добавлена",
                        "Ok");
                }
            }
            else
            {
                if (App.Database.GetItems().Count() == 0)
                {
                    bool result = await Application.Current.MainPage.DisplayAlert("Внимание!",
                        "Для того, чтобы пользоваться настройками, " +
                        "вам небходимо создать свой профиль", "Ok", "Отмена");
                    if (result) await Navigation.PushAsync(new RegistrPage());
                    //else await Navigation.PopAsync();

                }

                //User user = App.Database.GetItems().FirstOrDefault(x => x.IsActivity);
                if (App.Database.GetItems().Any() && user == null)
                {
                    bool result = await Application.Current.MainPage.DisplayAlert("Внимание!",
                        "Для того, чтобы пользоваться настройками, " +
                        "вам небходимо войти в свой профиль", "Ok", "Отмена");
                    if (result) await Navigation.PushAsync(new LoginPage());
                    //else await Navigation.PopAsync();
                }
            }
            //BookmArtResults = BookmArtResultsCreate();
        }
        public async System.Threading.Tasks.Task DeleteCalcAsync(string name)
        {
            User user = App.Database.GetItems().FirstOrDefault(x => x.IsActivity);
            if (user != null)
            {
                int idcoll = ArticleCollection.Articles.FirstOrDefault(x => x.Name == name).IdA;
                var item2 = App.DatabaseBookm.GetItems()
                    .Where(x => x.UserId == user.UserId)
                    .FirstOrDefault(y => y.IdColl == idcoll);
                if (item2 != null)
                {
                    bool result = await Application.Current.MainPage.DisplayAlert("Внимание!", "Удалить?",
                        "Ok", "Отмена");
                    if (result)
                    {
                        App.DatabaseBookm.DeleteItem(item2.Id);
                        await Application.Current.MainPage.DisplayAlert("Внимание!", "закладка успешно удалена",
                            "Ok");
                        //BookmArtResults = bookmArtResults;
                        OnPropertyChanged("BookmArtResults");
                        OnPropertyChanged("SelectedArticle");
                    }
                }

            }
            //BookmArtResults = BookmArtResultsCreate();

        }
    }
}
