using Xamarin.Forms;
using DesignEngineer.UWP;
[assembly: Dependency(typeof(BaseUrl_Windows))]
namespace DesignEngineer.UWP
{
     
    public class BaseUrl_Windows : IBaseUrl
    {
        public string Get()
        {
            return "ms-appx-web:///";
        }
    }
}