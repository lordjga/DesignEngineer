using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.ViewModels;
using DesignEngineer.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DesignEngineer.Views.GenTechServSection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageGenTechServ : ContentPage
    {
        public PageGenTechServ()
        {
            InitializeComponent();
            BindingContext = new XamlCommands() { Navigation = this.Navigation };
        }
        
    }
}