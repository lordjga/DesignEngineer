using Xamarin.Forms;
using DesignEngineer.iOS;
using Foundation;
[assembly: Dependency(typeof(BaseUrl_IOS))]
namespace DesignEngineer.iOS
{
    //
    public class BaseUrl_IOS : IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath;
        }
    }
}