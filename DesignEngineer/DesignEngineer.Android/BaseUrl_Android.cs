using Xamarin.Forms;
using Xamarin.Forms;
using DesignEngineer.Droid;
[assembly: Dependency(typeof(BaseUrl_Android))]
namespace DesignEngineer.Droid
{
    public class BaseUrl_Android : IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/pages/";
        }
    }
}