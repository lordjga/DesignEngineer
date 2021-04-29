using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DesignEngineer.Models;
using DesignEngineer.Views;
using DesignEngineer.Views.MenuSection;
using Xamarin.Forms;

namespace DesignEngineer.ViewModels
{
    public class RackCalcViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        private string stCalcB_error_01 =
            "Значение коэффициента для вычисления ширины рейки должно быть в интервале от 2 до 10";
        private Rack rack;
        private bool isCalc = false;
        private bool isRawData = false;
        public ICommand Calc { get; }
        public RackCalcViewModel()
        {
            rack = new Rack();
            Calc = new Command(CalcCommand);
        }
        #region Sv
        public int Beta1
        {
            get { return rack.beta1; }
            set
            {
                if (rack.beta1 != value)
                {
                    rack.beta1 = value;
                    OnPropertyChanged("Beta1");
                }
            }
        }
        public int Beta2
        {
            get { return rack.beta2; }
            set
            {
                if (rack.beta2 != value)
                {
                    rack.beta2 = value;
                    OnPropertyChanged("Beta2");
                }
            }
        }
        public int Beta3
        {
            get { return rack.beta3; }
            set
            {
                if (rack.beta3 != value)
                {
                    rack.beta3 = value;
                    OnPropertyChanged("Beta3");
                }
            }
        }
        public List<float> MnNum
        {
            get
            {
                return new List<float>
                {
                    0.05f, 0.055f, 0.06f, 0.07f, 0.08f, 0.09f, 1f, 1.125f, 1.25f, 1.375f, 1.5f,
                    1.6f, 1.75f, 2f, 2.25f, 2.5f, 2.75f, 3f, 3.15f, 3.5f, 4f, 4.5f, 5f, 5.5f, 6f, 
                    6.3f, 7f, 8f, 9f, 10, 11, 12, 12.5f, 14, 16, 18, 20, 22, 25, 28, 32, 36, 40, 
                    45, 50, 55, 60, 70, 80, 90, 100
                };
            }
        }
        private float _selectedMnNum;
        public float SelectedMnNum
        {
            get
            {
                return _selectedMnNum;
            }
            set
            {
                _selectedMnNum = value;
                Mn = value;
                OnPropertyChanged("SelectedMnNum");
            }
        }
        public float Mn
        {
            get { return rack.mn; }
            set
            {
                if (rack.mn != value)
                {
                    rack.mn = value;
                    OnPropertyChanged("Mn");
                }
            }
        }
        public float Kb
        {
            get { return rack.kb; }
            set
            {
                if (rack.kb != value)
                {
                    if (value < 2.0f || value > 10.0f)
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", stCalcB_error_01, "Ok");
                        rack.kb = 2.0f;
                    }
                    else
                    {
                        rack.kb = value;
                    }
                        
                    OnPropertyChanged("Kb");
                }
            }
        }
        public int Z
        {
            get { return rack.z; }
            set
            {
                if (rack.z != value)
                {
                    rack.z = value;
                    OnPropertyChanged("Z");
                }
            }
        }

        public float Mt
        {
            get { return rack.mt; }
            set
            {
                if (rack.mt != value)
                {
                    rack.mt = value;
                    OnPropertyChanged("Mt");
                }
            }
        }
        public float Pn
        {
            get { return rack.pn; }
            set
            {
                if (rack.pn != value)
                {
                    rack.pn = value;
                    OnPropertyChanged("Pn");
                }
            }
        }
        public float Pt
        {
            get { return rack.pt; }
            set
            {
                if (rack.pt != value)
                {
                    rack.pt = value;
                    OnPropertyChanged("Pt");
                }
            }
        }
        public float Ha
        {
            get { return rack.ha; }
            set
            {
                if (rack.ha != value)
                {
                    rack.ha = value;
                    OnPropertyChanged("Ha");
                }
            }
        }
        public float H
        {
            get { return rack.h; }
            set
            {
                if (rack.h != value)
                {
                    rack.h = value;
                    OnPropertyChanged("H");
                }
            }
        }
        public float B
        {
            get { return rack.b; }
            set
            {
                if (rack.b != value)
                {
                    rack.b = value;
                    OnPropertyChanged("B");
                }
            }
        }
        public float B1
        {
            get { return rack.b1; }
            set
            {
                if (rack.b1 != value)
                {
                    rack.b1 = value;
                    OnPropertyChanged("B1");
                }
            }
        }
        public float L
        {
            get { return rack.L; }
            set
            {
                if (rack.L != value)
                {
                    rack.L = value;
                    OnPropertyChanged("L");
                }
            }
        }
        public float Gamma
        {
            get { return rack.gamma; }
            set
            {
                if (rack.gamma != value)
                {
                    rack.gamma = value;
                    OnPropertyChanged("Gamma");
                }
            }
        }
        public int Gamma1
        {
            get { return rack.gamma1; }
            set
            {
                if (rack.gamma1 != value)
                {
                    rack.gamma1 = value;
                    OnPropertyChanged("Gamma1");
                }
            }
        }
        public int Gamma2
        {
            get { return rack.gamma2; }
            set
            {
                if (rack.gamma2 != value)
                {
                    rack.gamma2 = value;
                    OnPropertyChanged("Gamma2");
                }
            }
        }
        public int Gamma3
        {
            get { return rack.gamma3; }
            set
            {
                if (rack.gamma3 != value)
                {
                    rack.gamma3 = value;
                    OnPropertyChanged("Gamma3");
                }
            }
        }

        #endregion

        #region fCalc

        public void fCalcMt()
        {
            //Вычисли модуль основной (торцовый)
            Mt = (float) (Mn / Math.Cos((Beta1 + Beta2 / 60f + Beta3 / 3600f) * (float)(Math.PI / 180)));

        }
        public void fCalcP()
        {
            //Вычисли нормальный и торцовый шаг
            Pn = (float)(Math.PI * Mn);

            Pt = Pn / (float) Math.Cos((Beta1 + Beta2 / 60f + Beta3 / 3600f) * (float)(Math.PI / 180));

        }
        public void fCalcH()
        {
            //Вычисли высоту головки зуба и высоту зуба
            Ha = Mn;
            H = 2.25f * Mn;

        }
        public void fCalcB()
        {
            //Вычисли ширину рейки и длину косого зуба
            B = Kb * Mn;
            B1 = B / (float) Math.Cos((Beta1 + Beta2 / 60f + Beta3 / 3600f) * (float)(Math.PI / 180));

        }
        public ICommand LCommand => new Command(() =>
        {
            //Вычисли линейное перемещение рейки, соответствующее углу поворота ( gamma ) колеса или червяка
            if (Mn != 0 && Kb != 0 && Z != 0)
            {
                fCalcP();
                L = (Gamma1 + Gamma2 / 60f + Gamma3 / 3600f) * Pt * Z / 360.0f;
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Внимание!", "Сначала введите все остальные данные", "Ok");
            }
            
        });
        public ICommand GammaCommand => new Command(() =>
        {
            //Вычисли угол поворота колеса, соответсвующий линейному перемещению (L) рейки
            if (Mn != 0 && Kb != 0 && Z !=0)
            {
                fCalcP();
                Gamma = (L * 360.0f / Pt / Z);
                Gamma1 = (int)Gamma;
                Gamma2 = (int)((Gamma - Gamma1) * 60);
                Gamma3 = (int)(((Gamma - Gamma1) * 60 - Gamma2) * 60);
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Внимание!", "Сначала введите все остальные данные", "Ok");
            }
        });

        #endregion
        
        public void CalcCommand()
        {
            isCalc = false;
            if (Mn == 0 || Kb == 0 || Z == 0)
            {
                Application.Current.MainPage.DisplayAlert
                    ("Внимание", "Введите исходные данные", "Ок");
            }
            else
            {
                fCalcMt();
                fCalcP();
                fCalcH();
                fCalcB();
            }
            isCalc = true;
        }
        public ICommand SaveCom => new Command(async () =>
         {
             string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                 string filename = "SourceDataRackCalc.txt";
                 if (String.IsNullOrEmpty(filename)) return;
                 // если файл существует
                 if (File.Exists(Path.Combine(folderPath, filename)))
                 {
                     // запрашиваем разрешение на перезапись
                     bool isRewrited = await Application.Current.MainPage.DisplayAlert
                         ("Подтверждение", "Файл уже существует, перезаписать его?", "Да", "Нет");
                     if (isRewrited == false) return;
                 }
                 // перезаписываем файл
                 string ishtext = Beta1 + " " + Beta2 + " " + Beta3 + " " +
                                  SelectedMnNum + " " + Kb + " " + Z + " " + L + " " + Gamma1 + " " +
                                  Gamma2 + " " + Gamma3;
                 File.WriteAllText(Path.Combine(folderPath, filename), ishtext);
                 await Application.Current.MainPage.DisplayAlert
                     ("Сохранение", "Исходные данные сохранены", "Ок");
         });
        
        public ICommand DownCom => new Command(async () =>
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filename = "SourceDataRackCalc.txt";
            if (String.IsNullOrEmpty(filename)) await Application.Current.MainPage.DisplayAlert
                ("Подтверждение", "Файла не существует", "Ок"); ;
            // если файл существует
            if (File.Exists(Path.Combine(folderPath, filename)))
            {
                // запрашиваем разрешение на перезапись
                string ishtext2 = File.ReadAllText(Path.Combine(folderPath, filename));
                String[] words = ishtext2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                Beta1 = Convert.ToInt32(words[0]);
                Beta2 = Convert.ToInt32(words[1]);
                Beta3 = Convert.ToInt32(words[2]);
                SelectedMnNum = Convert.ToSingle(words[3]);
                Kb = Convert.ToSingle(words[4]);
                Z = Convert.ToInt32(words[5]);
                L = Convert.ToSingle(words[6]);
                Gamma1 = Convert.ToInt32(words[7]);
                Gamma2 = Convert.ToInt32(words[8]);
                Gamma3 = Convert.ToInt32(words[9]);

                await Application.Current.MainPage.DisplayAlert
                    ("Сохранение", "Исходные данные загружены", "Ок");
            }
        });
        
        public ICommand ReportCommand => new Command(async () =>
        {

            User user = App.Database.GetItems().FirstOrDefault(x => x.IsActivity);
            if (user != null)
            {
                if (isCalc)
                {
                    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string filename = "ReportSourceDataRackCalc.txt";
                    string filenameCount = "ReportSourceDataRackCalcCount.txt";
                    int count = 1;
                    bool isRewrited = await Application.Current.MainPage.DisplayAlert
                        ("Подтверждение", "Создать файл отчета?", "Да", "Нет");
                    if (isRewrited == false) return;

                    try
                    {
                        if (!File.Exists(Path.Combine(folderPath, filenameCount))) File.WriteAllText(Path.Combine(folderPath, filenameCount), "1");
                        else
                        {
                            count = Convert.ToInt32(File.ReadAllText(Path.Combine(folderPath, filenameCount)));
                            count++;
                            File.WriteAllText(Path.Combine(folderPath, filenameCount), count.ToString());
                        }
                        string reporttext =
                            "Расчёт реек\n\n" +
                            "Входные переменные расчета:\n" +
                            "Угол наклона зуба - " + Beta1 + "°" + Beta2 + "'" + Beta3 + "''\n" +
                            "Модуль нормальный - " + Mn + "\n" +
                            "Коэффициент для вычисления ширины рейки - " + Kb + "\n" +
                            "Число зубьев колеса или число заходов червяка - " + Z + "\n" +

                            "Выходные переменные расчета:\n" +
                            "Модуль основной (торцовый) - " + Mt + "\n" +
                            "Шаг нормальный - " + Pn + " мм\n" +
                            "Шаг торцовый - " + Pt + " мм\n" +
                            "Высота головки зуба - " + Ha + " мм\n" +
                            "Высота зуба - " + H + " мм\n" +
                            "Ширина рейки - " + B + " мм\n" +
                            "Длина косого зуба - " + B1 + " мм\n" +
                            "Линейное перемещение рейки, соответствующее углу поворота ( gamma )колеса или червяка - " + L + " мм\n" +
                            "Угол поворота колеса, соответсвующий линейному перемещению (L) рейки - " + Gamma1 + "°" + Gamma2 + "'" + Gamma3 + "''\n";
;
                        filename = String.Concat(count.ToString(), filename);
                        File.WriteAllText(Path.Combine(folderPath, filename), reporttext);
                        Report report = new Report();
                        report.UserId = user.UserId;
                        report.Name = filename;

                        bool isTitle = await Application.Current.MainPage.DisplayAlert
                                                ("Подтверждение", "Свое или стандартное имя?", "Свое", "Станд.");
                        if (isTitle == true)
                        {
                            string result = await Application.Current.MainPage.DisplayPromptAsync("Title", "Введите имя документа");
                            report.Title = result;
                            App.DatabaseReport.SaveItem(report);
                        }
                        else
                        {
                            report.Title = String.Concat(count.ToString(), ". Расчет рейки");
                            App.DatabaseReport.SaveItem(report);
                        }

                        
                    }
                    catch (Exception e)
                    {
                        await Application.Current.MainPage.DisplayAlert
                            ("Внимание", e.ToString(), "Ок");
                        throw;
                    }

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert
                        ("Внимание", "Сначала произведите расчет", "ок");
                }
            }

            else
            {
                if (App.Database.GetItems().Count() == 0)
                {
                    bool result = await Application.Current.MainPage.DisplayAlert("Внимание!", "Для того, чтобы пользоваться настройками, " +
                        "вам небходимо создать свой профиль", "Ok", "Отмена");
                    if (result) await Navigation.PushAsync(new RegistrPage());
                    else await Navigation.PopAsync();
                }
                if (App.Database.GetItems().Any() && user == null)
                {
                    bool result = await Application.Current.MainPage.DisplayAlert("Внимание!", "Для того, чтобы пользоваться настройками, " +
                        "вам небходимо войти в свой профиль", "Ok", "Отмена");
                    if (result) await Navigation.PushAsync(new LoginPage());
                    else await Navigation.PopAsync();
                }
            }
        });
        public ICommand AddBookmark => new Command(async () =>
        {
            BookmarkViewModel y = new BookmarkViewModel();
            await y.CreateCalcAsync("CalcRackPage");
        });
        public ICommand DeleteBookmark => new Command(async () =>
        {
            BookmarkViewModel y = new BookmarkViewModel();
            await y.DeleteCalcAsync("CalcRackPage");
        });

    }
}
