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
    public partial class CalcRackPage1 : ContentPage
    {
        public CalcRackPage1()
        {
            InitializeComponent();
            //BindingContext = new HelicalCylSpringsViewModel();
        }
        
    }
}