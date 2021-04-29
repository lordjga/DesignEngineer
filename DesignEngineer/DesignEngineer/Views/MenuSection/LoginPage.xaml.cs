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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new UserPageViewModel() { Navigation = this.Navigation };
            BindingContext = new XamlCommands() { Navigation = this.Navigation };
        }
    }
}