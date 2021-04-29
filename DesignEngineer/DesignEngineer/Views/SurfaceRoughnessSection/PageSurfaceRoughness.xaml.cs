using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignEngineer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DesignEngineer.Views.SurfaceRoughnessSection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSurfaceRoughness : ContentPage
    {
        public PageSurfaceRoughness()
        {
            InitializeComponent();
            BindingContext = new XamlCommands() { Navigation = this.Navigation };
        }
    }
}