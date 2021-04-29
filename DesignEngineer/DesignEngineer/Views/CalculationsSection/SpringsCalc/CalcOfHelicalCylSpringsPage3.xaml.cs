using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DesignEngineer.Views.CalculationsSection.SpringsCalc
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalcOfHelicalCylSpringsPage3 : ContentPage
    {
        public ArticleViewModel ViewModel { get; private set; }
        public CalcOfHelicalCylSpringsPage3()
        {
            InitializeComponent();
            ViewModel = ArticleCollection.Articles.FirstOrDefault(i => i.Article.Name == "30.CalcOfHelicalCylSprings.html");
            BindingContext = ViewModel;
        }
    }
}