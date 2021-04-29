using System;
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
    public partial class CalcOfHelicalCylSpringsPage : TabbedPage
    {
        public CalcOfHelicalCylSpringsPage()
        {
            InitializeComponent();
            BindingContext = new HelicalCylSpringsViewModel() { Navigation = this.Navigation };
        }
        
    }
}