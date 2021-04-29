using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DesignEngineer.Models;
using DesignEngineer.Views;
using DesignEngineer.Views.MenuSection;
using Xamarin.Forms;

public interface IBaseUrl { string Get(); }

namespace DesignEngineer.ViewModels
{
    public class ArticleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Article Article { get; private set; }
        public BookmarkArticle BookmarkArticle { get; private set; }
        public INavigation Navigation { get; set; }
        public ArticleViewModel()
        {
            Article = new Article();
            BookmarkArticle = new BookmarkArticle();
        }
        public WebViewSource WebViewSource
        {
            get { return Article.WebViewSource; }
            set
            {
                if (Article.WebViewSource != value)
                {
                    UrlWebViewSource urlSource = new UrlWebViewSource();
                    
                    urlSource.Url = Path.Combine(DependencyService.Get<IBaseUrl>().Get(), Name);
                    Article.WebViewSource = urlSource.Url.ToString();
                    OnPropertyChanged("WebViewSource");
                }
            }
        }
        public string Name
        {
            get { return Article.Name; }
            set
            {
                if (Article.Name != value)
                {
                    Article.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public int IdA
        {
            get { return Article.Id; }
            set
            {
                if (Article.Id != value)
                {
                    Article.Id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        public string Title
        {
            get { return Article.Title; }
            set
            {
                if (Article.Title != value)
                {
                    Article.Title = value;
                    OnPropertyChanged("Title");
                }
            }
        }
        public string Discription
        {
            get { return Article.Discription; }
            set
            {
                if (Article.Discription != value)
                {
                    Article.Discription = value;
                    OnPropertyChanged("Discription");
                }
            }
        }
        public bool IsValid
        {
            get
            {
                return ((!string.IsNullOrEmpty(Name.Trim())));
            }
        }
        public ICommand AddBookmark => new Command(async () =>
        {
            try
            {
                BookmarkViewModel y = new BookmarkViewModel();
                y.Navigation = this.Navigation;
                await y.CreateAsync(IdA);
            }

            
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Внимание!",
                    e.ToString(),
                    "Ok");
                throw;
            }
             
        });
       public ICommand DeleteBookmark => new Command(async () =>
        {
            BookmarkViewModel y = new BookmarkViewModel();
            await y.DeleteAsync(IdA);
        });
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}