using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DesignEngineer.Views.CalculationsSection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConverterCalc : ContentPage
    {
        public ConverterCalc()
        {
            InitializeComponent();
           // BindingContext = new ConverterViewModel(); //{ Navigation = this.Navigation };
            BindingContext = new XamlCommands() { Navigation = this.Navigation };

        }
    }
}