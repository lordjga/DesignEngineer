using System.Collections;
using DesignEngineer.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DesignEngineer.Views.CalculationsSection;
using DesignEngineer.Views.CalculationsSection.RackCalc;
using DesignEngineer.Views.CalculationsSection.SpringsCalc;
using Xamarin.Forms;

namespace DesignEngineer.ViewModels
{
    public class SearchViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ArticleCollection articleCollection { get; set; }
        ArticleViewModel selectedArticle;
        public INavigation Navigation { get; set; }

        public SearchViewModel()
        {
            articleCollection = new ArticleCollection();
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
                    OnPropertyChanged("SelectedArticle");
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
                    }
                    else
                    {
                        Navigation.PushAsync(new ArticlePage(tempArticle));
                    }
                    
                }
            }
        }

        public ICommand PerformSearch => new Command<string>((string query) =>
        {
            SearchResults = ArticleCollection.GetSearchResults(query);
        });

        List<ArticleViewModel> searchResults = ArticleCollection.Articles;
        public List<ArticleViewModel> SearchResults
        {
            get
            {
                return searchResults;
            }
            set
            {
                searchResults = value;
                OnPropertyChanged("SearchResults");
            }
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}