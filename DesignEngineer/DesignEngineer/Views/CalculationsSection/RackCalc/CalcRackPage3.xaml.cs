using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DesignEngineer.Views.CalculationsSection.RackCalc
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalcRackPage3 : ContentPage
    {
        public ArticleViewModel ViewModel { get; private set; }
        public CalcRackPage3()
        {
            InitializeComponent();
            ViewModel = ArticleCollection.Articles.FirstOrDefault(i => i.Article.Name == "31.CalcOfRack.html");
            BindingContext = ViewModel;
        }
    }
}