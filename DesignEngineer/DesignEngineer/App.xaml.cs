using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DesignEngineer.Models;
using DesignEngineer.Themes;
using DesignEngineer.ViewModels;
using DesignEngineer.Views.CalculationsSection.ConverterCalcs;
using DesignEngineer.Views.MenuSection;
using ThemingDemo;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DesignEngineer
{
    public partial class App : Application
    {
        public const string DATABASE_NAME = "UsersConfig.db";
        public static UserRepository database;
        public static BookmarkArticleRepository databaseBookm;
        public static ReportRepository databaseReport;
        public static UserRepository Database
        {
            get
            {
                if (database == null)
                {
                    // путь, по которому будет находиться база данных
                    string dbPath =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            DATABASE_NAME);
                    // если база данных не существует (еще не скопирована)
                    if (!File.Exists(dbPath))
                    {
                        // получаем текущую сборку
                        var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                        // берем из нее ресурс базы данных и создаем из него поток
                        using (Stream stream = assembly.GetManifestResourceStream($"DesignEngineer.{DATABASE_NAME}"))
                        {
                            using (FileStream fs = new FileStream(dbPath, FileMode.OpenOrCreate))
                            {
                                stream.CopyTo(fs); // копируем файл базы данных в нужное нам место
                                fs.Flush();
                            }
                        }
                    }

                    database = new UserRepository(dbPath);
                }

                return database;
            }
        }
        public static BookmarkArticleRepository DatabaseBookm
        {
            get
            {
                if (databaseBookm == null)
                {
                    // путь, по которому будет находиться база данных
                    string dbPath =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            DATABASE_NAME);
                    // если база данных не существует (еще не скопирована)
                    if (!File.Exists(dbPath))
                    {
                        // получаем текущую сборку
                        var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                        // берем из нее ресурс базы данных и создаем из него поток
                        using (Stream stream = assembly.GetManifestResourceStream($"DesignEngineer.{DATABASE_NAME}"))
                        {
                            using (FileStream fs = new FileStream(dbPath, FileMode.OpenOrCreate))
                            {
                                stream.CopyTo(fs); // копируем файл базы данных в нужное нам место
                                fs.Flush();
                            }
                        }
                    }

                    databaseBookm = new BookmarkArticleRepository(dbPath);
                }

                return databaseBookm;
            }
        }
        public static ReportRepository DatabaseReport
        {
            get
            {
                if (databaseReport == null)
                {
                    string dbPath =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            DATABASE_NAME);
                    if (!File.Exists(dbPath))
                    {
                        var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                        using (Stream stream = assembly.GetManifestResourceStream($"DesignEngineer.{DATABASE_NAME}"))
                        {
                            using (FileStream fs = new FileStream(dbPath, FileMode.OpenOrCreate))
                            {
                                stream.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                    databaseReport = new ReportRepository(dbPath);
                }
                return databaseReport;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            
            
        }

        protected override void OnStart()
        {
            ArticleCollection article = new ArticleCollection();

            bool isActivity = App.Database.GetItems().Any(x => x.IsActivity);
            if (isActivity)
            {
                Theme theme = (Theme)Enum.Parse(typeof(Theme), App.Database.GetItems().FirstOrDefault(x => x.IsActivity).Theme);
                Preferences.Set("theme", theme.ToString());
                ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
                if (mergedDictionaries != null)
                {
                    mergedDictionaries.Clear();

                    switch (theme)
                    {
                        case Theme.Dark:
                            mergedDictionaries.Add(new DarkTheme());
                            break;
                        case Theme.Light:
                        default:
                            mergedDictionaries.Add(new LightTheme());
                            break;
                    }
                }
            }

            

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
