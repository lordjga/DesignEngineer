using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DesignEngineer.Views.AxlesAndShafts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AxlesAndShaftsPage : ContentPage
    {
        public AxlesAndShaftsPage()
        {
            InitializeComponent();
            BindingContext = new XamlCommands() { Navigation = this.Navigation };
        }
    }
}