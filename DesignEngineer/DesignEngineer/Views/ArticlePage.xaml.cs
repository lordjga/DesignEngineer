using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.Models;
using HtmlAgilityPack;
using DesignEngineer.ViewModels;
using ThemingDemo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//public interface IBaseUrl { string Get(); }

namespace DesignEngineer.Views
{
    
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ArticlePage : ContentPage
    {
        public ArticleViewModel ViewModel { get; private set; }
        public ArticlePage(ArticleViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            ViewModel.Navigation = this.Navigation;
            BindingContext = ViewModel;
        }
        public ArticlePage(int id)
        {
            InitializeComponent();
            ViewModel = ArticleCollection.Articles.FirstOrDefault(i => i.Article.Id == id);
            ViewModel.Navigation = this.Navigation;
            BindingContext = ViewModel;

        }
        
    }
}
