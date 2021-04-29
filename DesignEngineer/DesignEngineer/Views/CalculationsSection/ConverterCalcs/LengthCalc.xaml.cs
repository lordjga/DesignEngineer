using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DesignEngineer.Views.CalculationsSection.ConverterCalcs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LengthCalc : ContentPage
    {
        public LengthCalc()
        {
            InitializeComponent();
           // BindingContext = new ConverterViewModel(); //{ Navigation = this.Navigation };
            BindingContext = new XamlCommands() { Navigation = this.Navigation };
          //  BindingContext = new ConverterViewModel(); //{ Navigation = this.Navigation };
        }
    }
}