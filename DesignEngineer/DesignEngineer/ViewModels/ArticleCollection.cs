using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using DesignEngineer.Models;
using HtmlAgilityPack;
using Xamarin.Forms;

public interface IBaseAsset { string Get(string filename); }
namespace DesignEngineer.ViewModels
{

    public class ArticleCollection
    {
        public static List<ArticleViewModel> Articles { get; set; }
        public static ObservableCollection<ArticleViewModel> BArticles { get; set; }


        static ArticleCollection()
        {
            Articles = new List<ArticleViewModel>();
            BArticles = new ObservableCollection<ArticleViewModel>();
            Articles.Add(new ArticleViewModel {  Name = "1.UnitsOfPhysicalQuantities.html" });
            Articles.Add(new ArticleViewModel {  Name = "2.SolutionOfTriangles.html"});
            Articles.Add(new ArticleViewModel {  Name = "3.TrigonometricDependencies.html"});
            Articles.Add(new ArticleViewModel {  Name = "4.ElementsOfStrengthOfMaterials.html"});
            Articles.Add(new ArticleViewModel {  Name = "5.FlatShapes.html"});
            Articles.Add(new ArticleViewModel {  Name = "6.SurfacesAndVolumesOfBodies.html"});
            Articles.Add(new ArticleViewModel {  Name = "7.PermissibleStressesAndMechanicalPropertiesOfMaterials.html" });
            Articles.Add(new ArticleViewModel {  Name = "8.TheCoefficientsOfFriction.html"});
            Articles.Add(new ArticleViewModel {  Name = "9.HardnessOfMetalsAndAlloys.html"});

            Articles.Add(new ArticleViewModel {  Name = "10.BasicRoughnessParametersAndTheirDesignations.html" });
            Articles.Add(new ArticleViewModel {  Name = "11.ControlTheRoughnessAndTheSurface.html"});
            Articles.Add(new ArticleViewModel {  Name = "12.SignsOfRoughnessOfTheSurfaces.html" });
            Articles.Add(new ArticleViewModel {  Name = "13.MatingSurfaces.html" });
            Articles.Add(new ArticleViewModel {  Name = "14.ScrapedSurfaces.html" });
            Articles.Add(new ArticleViewModel {  Name = "15.FitSurface.html" });
            Articles.Add(new ArticleViewModel {  Name = "16.FreeSurfaces.html" });
            Articles.Add(new ArticleViewModel {  Name = "17.TupicalSurface.html" });
            Articles.Add(new ArticleViewModel {  Name = "18.SurfacesDependingOnTheProcessingMethods.html"});
            Articles.Add(new ArticleViewModel {  Name = "19.SurfacesOfHolesAndShafts.html"    });

            Articles.Add(new ArticleViewModel { Name = "20.DesignTypesAndDimensions.html" });
            Articles.Add(new ArticleViewModel { Name = "21.TheorCalcOfAxes.html" });
            Articles.Add(new ArticleViewModel { Name = "22.CylindrAndConicalShaftEnds.html" });
            Articles.Add(new ArticleViewModel { Name = "23.DesignOfShafts.html" });
            Articles.Add(new ArticleViewModel { Name = "24.TheorCalcOfRigidity.html" });
            Articles.Add(new ArticleViewModel { Name = "25.TheorStrengthCalc.html" });
            Articles.Add(new ArticleViewModel { Name = "26.TorqueOutput.html" });
            Articles.Add(new ArticleViewModel { Name = "27.LoadsOnTheShaft.html" });
            Articles.Add(new ArticleViewModel { Name = "28.ReactOfSupportsAndBendingMoments.html" });
            Articles.Add(new ArticleViewModel { Name = "29.ExampleOfCalcOfTheShaft.html" });

            Articles.Add(new ArticleViewModel { Name = "30.CalcOfHelicalCylSprings.html" });
            Articles.Add(new ArticleViewModel { Name = "31.CalcOfRack.html" });
            
            int i = 1;
           foreach (var item in Articles)
           {
               item.WebViewSource = "";
               i++;
           }

            Parsing(Articles);
            
            Articles.Add(new ArticleViewModel { Name = "ConverterCalc", Title = "Конвертер величин", 
                Discription = "Масса, Длина, Объем, Скорость, Площадь, Температура, Угол, Энергия, Давление, Мощность" });
            Articles.Add(new ArticleViewModel
            {
                Name = "CalcOfHelicalCylSpringsPage",
                Title = "Расчет винтовых цилиндрических пружин",
                Discription = "Расчёт винтовых цилиндрических пружин сжатия без предварительного напряжения из стали круглого сечения"
            });
            Articles.Add(new ArticleViewModel
            {
                Name = "CalcRackPage",
                Title = "Расчет реек",
                Discription = "Формулы для расчета реек"
            });
            i = 1;
            foreach (var item in Articles)
            {
                item.IdA = i;
                i++;
            }
        }
        public static void Parsing(List<ArticleViewModel> Articles)
        {
            HtmlDocument document = new HtmlDocument();
            foreach (var item in Articles)
            {
                string stream = DependencyService.Get<IBaseAsset>().Get(item.Name);
                document.LoadHtml(stream);
                var node = document.DocumentNode.SelectNodes("//h1");
                var node2 = document.DocumentNode.SelectNodes("//h2");
                foreach (var item1 in node)
                {
                    item.Title = item1.InnerHtml;
                }
                foreach (var item1 in node2)
                {
                    item.Discription += item1.InnerHtml;
                }
            }
        }
        public static List<ArticleViewModel> GetSearchResults(string queryString)
        {
            var normalizedQuery = queryString?.ToLower() ?? "";
            return Articles.Where(f => f.Title.ToLowerInvariant().Contains(normalizedQuery)|| f.Discription.ToLowerInvariant().Contains(normalizedQuery)).ToList();
        }
        public static ObservableCollection<ArticleViewModel> GetBookmArtResults()
        {
            User user = App.Database.GetItems().FirstOrDefault(x => x.IsActivity);
            var bookmarkArticle = App.DatabaseBookm.GetItems().Where(x => x.UserId == user.UserId);
            BArticles = new ObservableCollection<ArticleViewModel>();
            foreach (var item in bookmarkArticle)
            {
                BArticles.Add(Articles.FirstOrDefault(f => f.IdA == item.IdColl));
            }
            return BArticles;
        }
    }
}
