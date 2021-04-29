using System.IO;
using Android.Content.Res;
using Xamarin.Forms;
using DesignEngineer.Droid;
[assembly: Dependency(typeof(BaseAsset_Android))]
namespace DesignEngineer.Droid
{
    public class BaseAsset_Android : IBaseAsset
    {
        public string Get(string filename)
        {
            string text;
            AssetManager assets = Android.App.Application.Context.Assets;
            using (StreamReader reader = new StreamReader(assets.Open(Path.Combine("pages/", filename))))
            {
                text = reader.ReadToEnd();
            }

            return text;
        }
        
    }
}