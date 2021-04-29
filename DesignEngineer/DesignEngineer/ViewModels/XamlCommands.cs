using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DesignEngineer.Models;
using DesignEngineer.Views;
using DesignEngineer.Views.AxlesAndShafts;
using DesignEngineer.Views.CalculationsSection;
using DesignEngineer.Views.CalculationsSection.ConverterCalcs;
using DesignEngineer.Views.CalculationsSection.RackCalc;
using DesignEngineer.Views.CalculationsSection.SpringsCalc;
using DesignEngineer.Views.GenTechServSection;
using DesignEngineer.Views.MenuSection;
using DesignEngineer.Views.SurfaceRoughnessSection;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace DesignEngineer.ViewModels
{
    public class XamlCommands
    {
        public INavigation Navigation { get; set; }
        public XamlCommands()
        {
        }
        public ICommand BToWebViewOnClicked => new Command((parameter) =>
        {
            try
            {
                Navigation.PushAsync(new ArticlePage(Convert.ToInt32(parameter)));
            }
            catch
            {
                Application.Current.MainPage.DisplayAlert("Внимание!", "Данный раздел находится в разработке", "Ok");
            }
            
        });
        public ICommand BChangeAccClicked => new Command(async (parameter) =>
        {
            UserPageViewModel y = new UserPageViewModel();
            await y.ChangeAccAsync(parameter.ToString());
            if (parameter.ToString() == "Change")
            {
                await Navigation.PushAsync(new LoginPage());
            }
            await this.Navigation.PopAsync();
        });
        public ICommand BToPagesOnClicked => new Command((parameter) =>
        {
            try
            {
                switch (parameter.ToString())
                {
                    case "Alarm":
                        Application.Current.MainPage.DisplayAlert("Внимание!", "Данный раздел находится в разработке", "Ok");
                        break;


                    case "Search":
                        Navigation.PushAsync(new SearchPage());
                        break;
                    case "Settings":
                        Navigation.PushAsync(new SettingsPage());
                        break;
                    case "About":
                        Navigation.PushAsync(new AboutPage());
                        break;
                    case "Bookmark":
                        Navigation.PushAsync(new BookmarkPage());
                        break;
                    case "Report":
                        Navigation.PushAsync(new ReportPage());
                        break;

                    case "GenTechServ":
                        Navigation.PushAsync(new PageGenTechServ());
                        break;
                    case "Calculations":
                        Navigation.PushAsync(new PageCalculations());
                        break;
                    case "SurfaceRoughness":
                        Navigation.PushAsync(new PageSurfaceRoughness());
                        break;
                    case "AxlesAndShafts":
                        Navigation.PushAsync(new AxlesAndShaftsPage());
                        break;
                        
                    case "Converter":
                        Navigation.PushAsync(new ConverterCalc());
                        break;
                    case "LengthCalc":
                        Navigation.PushAsync(new LengthCalc());
                        break;
                    case "MassCalc":
                        Navigation.PushAsync(new MassCalc());
                        break;
                    case "VolumeCalc":
                        Navigation.PushAsync(new VolumeCalc());
                        break;
                    case "SpeedCalc":
                        Navigation.PushAsync(new SpeedCalc());
                        break;
                    case "AreaCalc":
                        Navigation.PushAsync(new AreaCalc());
                        break;
                    case "TemperatureCalc":
                        Navigation.PushAsync(new TemperatureCalc());
                        break;
                    case "AngleCalc":
                        Navigation.PushAsync(new AngleCalc());
                        break;
                    case "PressureCalc":
                        Navigation.PushAsync(new PressureCalc());
                        break;
                    case "EnergyCalc":
                        Navigation.PushAsync(new EnergyCalc());
                        break;
                    case "PowerCalc":
                        Navigation.PushAsync(new PowerCalc());
                        break;

                    case "Springs":
                        Navigation.PushAsync(new CalcOfHelicalCylSpringsPage());
                        break;
                    case "Rack":
                        Navigation.PushAsync(new CalcRackPage());
                        break;

                }
            }
            catch (Exception e)
            {
                Application.Current.MainPage.DisplayAlert("Внимание", e.ToString(), "Ok");
            }
        });
        
        
    }
    
}
