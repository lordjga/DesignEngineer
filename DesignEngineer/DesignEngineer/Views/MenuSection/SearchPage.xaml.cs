using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DesignEngineer.Views.MenuSection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel() { Navigation = this.Navigation };
        }
        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            searchResults.ItemsSource = ArticleCollection.GetSearchResults(e.NewTextValue);
        }
    }
}