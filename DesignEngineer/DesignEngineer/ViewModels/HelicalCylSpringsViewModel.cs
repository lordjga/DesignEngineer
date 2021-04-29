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
    public class HelicalCylSpringsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }
        private bool isCalc = false;
        private bool isRawData = false;
        
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #region const

        private string errorDivByZero_deltaStr =
            "Ошибка в вычислении: в одном из действий возникла попытка деления на 0. Относительный инерционный зазор должен быть меньше 1.";
        private string errorDivByZero_tauStr =
            "Ошибка в вычислении: в одном из действий возникла попытка деления на 0. Допускаемое касательное напряжение пружины должно быть больше 0.";
        private string errorUnknownClassStr =
            "Неизвестный класс пружины:";
        private string errorUnknownRankStr = "Неизвестный разряд пружины: ";
        private string errorRankMismatchStr = "Выбранный разряд пружины недопустим для данного класса.";
        private string errorDeltaStr = "Значение относительного инерционного зазора должно входить в интервал (0; 1).";
        private string errorF1lt0Str = "Сила пружины при предварительной деформации должна быть больше нуля.";

        private string errorF2ltF1Str =
            "Сила пружины при рабочей деформации должна быть больше силы пружины при предварительной деформации.";

        private string error_hStr = "Рабочий ход пружины должен быть больше нуля.";
        private string error_tauStr = "Допускаемое касательное напряжение пружины должно быть больше нуля.";
        private string error_nStr = "Число рабочих витков должно быть больше нуля.";
        private string error_n2Str = "Число опорных витков должно быть не меньше нуля.";
        private string error_n3Str = "Число обработанных витков должно быть не меньше нуля.";

        private string error_roStr =
            "Динамическая плотность материала должна быть больше нуля.\\nДля пружинной стали обычно равна 8000 Н*с^2/м^4.";

        private string error_GStr =
            "Модуль сдвига должен быть больше нуля. Для пружинной стали обычно равен 78500 МПа.";

        private string error_kStr =
            "Ошибка в вычислении коэффициента кривизны витка пружины: в одном из действий возникла попытка деления на 0. Измените значение индекса пружины.";

        private string warningF3MismatchStr =
            "Сила пружины при максимальной деформации не соответствует выбранным классу и разряду.";
        private string warning_vKStr = "Критическая скорость пружины выше максимальной.";
        private string warning_iStr = "Значение индекса пружины не входит в интервал рекомендуемых значений [4; 12].";

        private string warningDelta12Str =
            "Для одножильных пружин сжатия классов I и II рекомендуемые значения относительного инерционного зазора составляют интервал [0.05; 0.25].";

        private string warningDelta3Str =
            "Для одножильных пружин сжатия класса III рекомендуемые значения относительного инерционного зазора составляют интервал [0.10; 0.40].";

        private string warning_dStr =
            "Введённое значение диаметра проволоки не соответствует классу и разряду пружины.";

        private string warning_d_F3Str =
            "Введенное значение диаметра проволоки соответствует классу и разряду пружины,\\nно не соответствует силе пружины при максимальной деформации.";

        private string warning_d_todo =
            "Уменьшите силу пружины при рабочей деформации или относительный \\инерционный зазор либо увеличьте диаметр проволоки.";

        private char class1 = 'I';
        private string class2 = "II";
        private string class3 = "III";

        private string conditionOk = "Выполнено";
        private string conditionNotOk = "Не выполнено";

        private string admissVal = "Допустимые значения принадлежат интервалу";
        private string tryTo = "Попробуйте ";
        private string increase = "увеличить ";
        private string decrease = "уменьшить ";
        private string orStr = " или ";
        private string F2Str = "силу пружины при рабочей деформации";
        private string deltaStr = "относительный инерционный зазор пружины";
        private int param_A = -4;

        #endregion



        private HelicalCylSpring spring;
        public ICommand Calc { get; }

        public HelicalCylSpringsViewModel()
        {
            spring = new HelicalCylSpring();
            Calc = new Command(CalcCommand);
        }

        

        #region Sv
        public List<string> ClassNum
        {
            get
            {
                return new List<string> { "I", "II", "III" };
            }
        }
        private string _selectedClassNum;
        public string SelectedClassNum
        {
            get
            {
                return _selectedClassNum;
            }
            set
            {
                _selectedClassNum = value;
                Class = value;
                OnPropertyChanged("SelectedClassNum");
            }
        }
        public List<int> RankNum
        {
            get
            {
                return new List<int> { 1, 2, 3, 4 };
            }
        }
        private int _selectedRankNum;
        public int SelectedRankNum
        {
            get
            {
                return _selectedRankNum;
            }
            set
            {
                _selectedRankNum = value;
                Rank = value;
                OnPropertyChanged("SelectedRankNum");
            }
        }
        public string Class
        {
            get { return spring.Class; }
            set
            {
                if (spring.Class != value)
                {

                    if ((Delta >= 0.05 && Delta <= 0.25) & (value == class1.ToString() || value == class2))
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", warningDelta12Str, "Ok");
                        SelectedClassNum = class3;
                    }
                    if ((Delta >= 0.1 && Delta <= 0.4) & (value == class3))
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", warningDelta3Str, "Ok");
                        SelectedClassNum = class2;
                    }
                    if (value == class3 & (Rank == 1 || Rank == 4))
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", errorRankMismatchStr, "Ok");
                        SelectedClassNum = class2;
                    }
                    spring.Class = value;
                    OnPropertyChanged("Class");
                }
            }
        }
        public int Rank
        {
            get { return spring.rank; }
            set
            {
                if (spring.rank != value)
                {
                    spring.rank = value;
                    if (Class == class3 & (value == 1 || value == 4))
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", errorRankMismatchStr, "Ok");
                        SelectedRankNum = 2;
                    }
                    OnPropertyChanged("Rank");
                }
            }
        }
        public float D1
        {
            get { return spring.D1; }
            set
            {
                if (spring.D1 != value)
                {
                    spring.D1 = value;
                    OnPropertyChanged("D1");
                }
            }
        }
        public float Delta
        {
            get { return spring.delta; }
            set
            {
                if (spring.delta != value)
                {
                    if (value >= 1 || value < 0)
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", errorDeltaStr, "Ok");
                        value = 0.1f;
                    }
                    if ((value <= 0.05 || value >= 0.25) & (Class == class1.ToString() || Class == class2))
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", warningDelta12Str, "Ok");
                        //value = 0.2f;
                    }
                    if ((value <= 0.1 || value >= 0.4) & (Class == class3))
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", warningDelta3Str, "Ok");
                        //value = 0.3f;
                    }
                    spring.delta = value;
                    OnPropertyChanged("Delta");
                }
            }
        }
        public float F1
        {
            get { return spring.F1; }
            set
            {
                if (spring.F1 != value)
                {
                    if (value < 0)
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", errorF1lt0Str, "Ok");
                        value = 0f;
                    }
                    spring.F1 = value;
                    OnPropertyChanged("F1");
                }
            }
        }
        public float F2
        {
            get { return spring.F2; }
            set
            {
                if (spring.F2 != value)
                {
                    if (value < F1)
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", errorF2ltF1Str, "Ok");

                    }
                    spring.F2 = value;
                    OnPropertyChanged("F2");
                }
            }
        }
        public float h
        {
            get { return spring.h; }
            set
            {
                if (spring.h != value)
                {
                    if (value <= 0)
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", error_hStr, "Ok");
                        value = 1f;
                    }
                    spring.h = value;
                    OnPropertyChanged("h");
                }
            }
        }
        public float tau
        {
            get { return spring.tau; }
            set
            {
                if (spring.tau != value)
                {
                    if (value <= 0)
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", error_tauStr, "Ok");
                        value = 1f;
                    }
                    spring.tau = value;
                    OnPropertyChanged("tau");
                }
            }
        }
        public float d
        {
            get { return spring.d; }
            set
            {
                if (spring.d != value)
                {
                    spring.d = value;

                    OnPropertyChanged("d");
                }
            }
        }
        public float n
        {
            get { return spring.n; }
            set
            {
                if (spring.n != value)
                {
                     if (value <= 0)
                     {
                         Application.Current.MainPage.DisplayAlert("Внимание!", error_nStr, "Ok");
                         value = 1f;
                     }
                     spring.n = value;
                     OnPropertyChanged("n");
                }
            }
        }
        public float n2
        {
            get { return spring.n2; }
            set
            {
                if (spring.n2 != value)
                {
                    if (value < 0)
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", error_n2Str, "Ok");
                        value = 0f;
                    }
                    spring.n2 = value;
                    OnPropertyChanged("n2");
                }
            }
        }
        public float n3
        {
            get { return spring.n3; }
            set
            {
                if (spring.n3 != value)
                {
                    if (value < 0)
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", error_n3Str, "Ok");
                        value = 0f;
                    }
                    spring.n3 = value;
                    OnPropertyChanged("n3");
                }
            }
        }
        public float vMax
        {
            get { return spring.vMax; }
            set
            {
                if (spring.vMax != value)
                {
                    spring.vMax = value;
                    OnPropertyChanged("vMax");
                }
            }
        }
        public float G
        {
            get { return spring.G; }
            set
            {
                if (spring.G != value)
                {
                    spring.G = value;
                    OnPropertyChanged("G");
                }
            }
        }
        public float Ro
        {
            get { return spring.ro; }
            set
            {
                if (spring.ro != value)
                {
                    if (value <= 0)
                    {
                        Application.Current.MainPage.DisplayAlert("Внимание!", error_roStr, "Ok");
                        value = 1f;
                    }
                    spring.ro = value;
                    OnPropertyChanged("Ro");
                }
            }
        }
        public float F3
        {
            get { return spring.F3; }
            set
            {
                if (spring.F3 != value)
                {
                    spring.F3 = value;

                    OnPropertyChanged("F3");
                }
            }
        }
        public float Ds
        {
            get { return spring.Ds; }
            set
            {
                if (spring.Ds != value)
                {
                    spring.Ds = value;

                    OnPropertyChanged("Ds");
                }
            }
        }

        public float D2
        {
            get { return spring.D2; }
            set
            {
                if (spring.D2 != value)
                {
                    spring.D2 = value;

                    OnPropertyChanged("D2");
                }
            }
        }
        public float i
        {
            get { return spring.i; }
            set
            {
                if (spring.i != value)
                {
                    spring.i = value;

                    OnPropertyChanged("i");
                }
            }
        }
        public float k
        {
            get { return spring.k; }
            set
            {
                if (spring.k != value)
                {
                    spring.k = value;

                    OnPropertyChanged("k");
                }
            }
        }
        public float c
        {
            get { return spring.c; }
            set
            {
                if (spring.c1 != value)
                {
                    spring.c = value;

                    OnPropertyChanged("c");
                }
            }
        }
        public float c1
        {
            get { return spring.c1; }
            set
            {
                if (spring.c1 != value)
                {
                    spring.c1 = value;

                    OnPropertyChanged("c1");
                }
            }
        }
        public float n1
        {
            get { return spring.n1; }
            set
            {
                if (spring.n1 != value)
                {
                    spring.n1 = value;

                    OnPropertyChanged("n1");
                }
            }
        }
        public float s1
        {
            get { return spring.s1; }
            set
            {
                if (spring.s1 != value)
                {
                    spring.s1 = value;

                    OnPropertyChanged("s1");
                }
            }
        }
        public float s2
        {
            get { return spring.s2; }
            set
            {
                if (spring.s2 != value)
                {
                    spring.s2 = value;

                    OnPropertyChanged("s2");
                }
            }
        }
        public float s3
        {
            get { return spring.s3; }
            set
            {
                if (spring.s3 != value)
                {
                    spring.s3 = value;

                    OnPropertyChanged("s3");
                }
            }
        }

        public float s_3
        {
            get { return spring.s_3; }
            set
            {
                if (spring.s_3 != value)
                {
                    spring.s_3 = value;

                    OnPropertyChanged("s_3");
                }
            }
        }
        public float t
        {
            get { return spring.t; }
            set
            {
                if (spring.t != value)
                {
                    spring.t = value;

                    OnPropertyChanged("t");
                }
            }
        }
        public float l0
        {
            get { return spring.l0; }
            set
            {
                if (spring.l0 != value)
                {
                    spring.l0 = value;

                    OnPropertyChanged("l0");
                }
            }
        }
        public float l1
        {
            get { return spring.l1; }
            set
            {
                if (spring.l1 != value)
                {
                    spring.l1 = value;

                    OnPropertyChanged("l1");
                }
            }
        }
        public float l2
        {
            get { return spring.l2; }
            set
            {
                if (spring.l2 != value)
                {
                    spring.l2 = value;

                    OnPropertyChanged("l2");
                }
            }
        }
        public float l3
        {
            get { return spring.l3; }
            set
            {
                if (spring.l3 != value)
                {
                    spring.l3 = value;

                    OnPropertyChanged("l3");
                }
            }
        }
        public float tau1
        {
            get { return spring.tau1; }
            set
            {
                if (spring.tau1 != value)
                {
                    spring.tau1 = value;

                    OnPropertyChanged("tau1");
                }
            }
        }
        public float tau2
        {
            get { return spring.tau2; }
            set
            {
                if (spring.tau2 != value)
                {
                    spring.tau2 = value;

                    OnPropertyChanged("tau2");
                }
            }
        }
        public float tau3
        {
            get { return spring.tau3; }
            set
            {
                if (spring.tau3 != value)
                {
                    spring.tau3 = value;

                    OnPropertyChanged("tau3");
                }
            }
        }
        public float l
        {
            get { return spring.l; }
            set
            {
                if (spring.l != value)
                {
                    spring.l = value;

                    OnPropertyChanged("l");
                }
            }
        }
        public float m
        {
            get { return spring.m; }
            set
            {
                if (spring.m != value)
                {
                    spring.m = value;

                    OnPropertyChanged("m");
                }
            }
        }
        public float V
        {
            get { return spring.V; }
            set
            {
                if (spring.V != value)
                {
                    spring.V = value;

                    OnPropertyChanged("V");
                }
            }
        }
        public float U
        {
            get { return spring.U; }
            set
            {
                if (spring.U != value)
                {
                    spring.U = value;

                    OnPropertyChanged("U");
                }
            }
        }
        public float vK
        {
            get { return spring.vK; }
            set
            {
                if (spring.vK != value)
                {
                    spring.vK = value;

                    OnPropertyChanged("vK");
                }
            }
        }
        public string conditionF3
        {
            get { return spring.conditionF3; }
            set
            {
                if (spring.conditionF3 != value)
                {
                    spring.conditionF3 = value;

                    OnPropertyChanged("conditionF3");
                }
            }
        }
        public string conditionvK
        {
            get { return spring.conditionvK; }
            set
            {
                if (spring.conditionvK != value)
                {
                    spring.conditionvK = value;

                    OnPropertyChanged("conditionvK");
                }
            }
        }


        #endregion

        #region fCalc

        public void fCalc_F3()
        {
            //Расчёт силы пружины при максимальной деформации
            try
            {
                F3 = F2 / (1 - Delta);
            }
            catch (Exception e)
            {
                Application.Current.MainPage.DisplayAlert("Внимание!", errorDivByZero_deltaStr, "Ok");
            }
        }
        public void fCheck_F3()
        {
            //Проверка силы пружины при максимальной деформации на допустимость для данного разряда
            int result;
            string todo;
            float[] mas;
            if (Class == class1.ToString())
                mas = new[] { 1f, 850f, 1f, 800f, 140f, 6000f, 2800f, 180000f };
            else if (Class == class2)
                mas = new[] { 1.5f, 1400f, 1.25f, 1250f, 236f, 10000f, 4500f, 280000f };
            else
            {
                if ((Rank == 1 || Rank == 4))
                {
                    Application.Current.MainPage.DisplayAlert("Внимание!", errorRankMismatchStr, "Ok");
                }
                mas = new[] { 0f, 0f, 315f, 14000f, 6000f, 20000f, 0f, 0f };
            }

            result = 1;
            if (!(mas[(Rank - 1) * 2] <= F3 && F3 <= mas[(Rank - 1) * 2 + 1]))
            {
                todo = decrease;
                if (F3 < mas[(Rank - 1) * 2]) todo = increase;

                Application.Current.MainPage.DisplayAlert("Внимание!", warningF3MismatchStr + "\n" + admissVal + " [" + mas[(Rank - 1) * 2]
                                                                       + "; " + +mas[(Rank - 1) * 2 + 1] + "].\n" + tryTo + todo + F2Str + orStr + deltaStr + ".", "Ok");
                result = 0;
            }

            if (result == 0)
                conditionF3 = conditionNotOk;
            else
                conditionF3 = conditionOk;
        }
        public void fCalc_Ds()
        {
            //Расчёт среднего диаметра пружины
            Ds = D1 - d;
        }
        public void fCalc_D2()
        {
            //Расчёт внутреннего диаметра пружины
            D2 = D1 - 2 * d;
        }
        public void fCalc_i()
        {
            //Расчёт индекса пружины
            i = Ds / d;
            if (!(i >= 4 || i <= 12)) Application.Current.MainPage.DisplayAlert("Внимание!", warning_iStr, "Ok");
        }
        public void fCalc_k()
        {
            //Расчёт коэффициента, учитывающего кривизну витка пружины
            try
            {
                k = (4 * i - 1) / (4 * i + param_A) + 0.615f / i;
            }
            catch (Exception e)
            {
                Application.Current.MainPage.DisplayAlert("Внимание!", error_kStr, "Ok");
            }
        }
        public void fCalc_tau3()
        {
            //Расчёт максимального касательного напряжения пружины
            tau3 = k * 8f * F3 * Ds / (float)(Math.PI * Math.Pow(d, 3));
        }
        public void fCalc_vK()
        {
            //Расчёт критической скорости пружины сжатия

            vK = 1000 * tau3 * (1 - F2 / F3) / (float)(Math.Sqrt(2f * G * Ro));
            if (vK > vMax)
            {
                conditionvK = conditionNotOk;
                Application.Current.MainPage.DisplayAlert("Внимание!", warning_vKStr, "Ok");
            }
            else conditionvK = conditionOk;

        }
        public void fCalc_c()
        {
            //Расчёт жесткости пружины
            c = (F2 - F1) / h;
        }
        public void fCalc_n()
        {
            //Расчёт числа витков пружины
            fCalc_Ds();
            fCalc_c();
            n = G * (float)(Math.Pow(d, 4) / (8 * c * (float)(Math.Pow(Ds, 3))));
            n = (float)Math.Round(n * 2 - 0.5) / 2;
        }
        public void fCalc_c1()
        {
            //Расчёт жесткости одного витка пружины
            c1 = n * c;
        }
        public void fCalc_n1()
        {
            //Расчёт полного числа витков пружины
            n1 = n + n2;
        }
        public void fCalc_s123()
        {
            //Расчёт предварительной, рабочей и максимальной деформаций пружины
            s1 = F1 / c;
            s2 = F2 / c;
            s3 = F3 / c;
        }
        public void fCalc_s_3()
        {
            //Расчёт максимальной деформации одного витка пружины
            s_3 = s3 / n;
        }
        public void fCalc_l3()
        {
            //Расчёт длины пружины при максимальной деформации
            l3 = d * (n1 + 1 - n3);
        }
        public void fCalc_l0()
        {
            //Расчёт длины пружины в свободном состоянии
            l0 = l3 + s3;
        }
        public void fCalc_l1()
        {
            //Расчёт длины пружины при предварительной деформации
            l1 = l0 - s1;
        }
        public void fCalc_l2()
        {
            //Расчёт длины пружины при рабочей деформации
            l2 = l0 - s2;
        }
        public void fCalc_t()
        {
            //Расчёт шага пружины в свободном состоянии
            t = d + s3 / n;
        }
        public void fCalc_tau12()
        {
            //Расчёт напряжений в пружине при предварительной и рабочей деформации
            tau1 = tau3 * F1 / F3;
            tau2 = tau3 * F2 / F3;
        }
        public void fCalc_l()
        {
            //Расчёт длины развёрнутой пружины
            l = (float)Math.PI * Ds * n1;
        }
        public void fCalc_m()
        {
            //Расчёт массы пружины
            m = 19.25f * Ds * (float)Math.Pow(d, 2) * n1 / 1000000;
        }
        public void fCalc_V()
        {
            //Расчёт объёма, занимаемого пружиной
            V = 0.78f * l1 * (float)Math.Pow(D1, 2);
        }
        public void fCalc_U()
        {
            //Расчёт максимальной энергии, накапливаемой пружиной
            U = F3 * s3 / 2;
        }

        #endregion
        public ICommand NjCommand => new Command( () =>
        {
            if (G!=0 && d!=0 && F2!=0  && h != 0 && D1 != 0)
            {
                fCalc_n();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Внимание!", "Сначала введите все остальные данные или внесите количество витков вручную", "Ok");
            }
            
        });
        public void CalcCommand()
        {
            isCalc = false;
            if (Class==null||Rank==null)
            {
                Application.Current.MainPage.DisplayAlert
                    ("Внимание", "Введите исходные данные", "Ок");
            }
            else
            {
                fCalc_F3();
                fCheck_F3();
                fCalc_Ds();
                fCalc_D2();
                fCalc_i();
                fCalc_k();
                fCalc_tau3();
                fCalc_vK();
                fCalc_c();
                fCalc_n1();
                fCalc_c1();
                fCalc_s123();
                fCalc_l3();
                fCalc_l0();
                fCalc_l1();
                fCalc_l2();
                fCalc_t();
                fCalc_s_3();
                fCalc_tau12();
                fCalc_l();
                fCalc_m();
                fCalc_V();
                fCalc_U();
                isCalc = true;
            }
            
        }
        public ICommand SaveCom => new Command(async () =>
         {
             
                 string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                 string filename = "SourceDataHelicalCylSprings.txt";
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
                 string ishtext = SelectedClassNum + " " + SelectedRankNum + " " + D1 + " " +
                                  Delta + " " + F1 + " " + F2 + " " + h + " " + tau + " " +
                                  d + " " + n + " " + n2 + " " + n3 + " " + vMax + " " +
                                  G + " " + Ro;
                 File.WriteAllText(Path.Combine(folderPath, filename), ishtext);
                 await Application.Current.MainPage.DisplayAlert
                     ("Сохранение", "Исходные данные сохранены", "Ок");


         });
        public ICommand DownCom => new Command(async () =>
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filename = "SourceDataHelicalCylSprings.txt";
            if (String.IsNullOrEmpty(filename)) await Application.Current.MainPage.DisplayAlert
                ("Подтверждение", "Файла не существует", "Ок"); ;
            // если файл существует
            if (File.Exists(Path.Combine(folderPath, filename)))
            {
                // запрашиваем разрешение на перезапись
                string ishtext2 = File.ReadAllText(Path.Combine(folderPath, filename));
                String[] words = ishtext2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                SelectedClassNum = words[0];
                SelectedRankNum = Convert.ToInt32(words[1]);
                D1 = Convert.ToSingle(words[2]);
                Delta = Convert.ToSingle(words[3]);
                F1 = Convert.ToSingle(words[4]);
                F2 = Convert.ToSingle(words[5]);
                h = Convert.ToSingle(words[6]);
                tau = Convert.ToSingle(words[7]);
                d = Convert.ToSingle(words[8]);
                n = Convert.ToSingle(words[9]);
                n2 = Convert.ToSingle(words[10]);
                n3 = Convert.ToSingle(words[11]);
                vMax = Convert.ToSingle(words[12]);
                G = Convert.ToSingle(words[13]);
                Ro = Convert.ToSingle(words[14]);
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
                    string filename = "ReportSourceDataHelicalCylSprings.txt";
                    string filenameCount = "ReportSourceDataHelicalCylSpringsCount.txt";
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
                        string reporttext = "Расчёт винтовых цилиндрических пружин сжатия без предварительного напряжения из стали круглого сечения\n" +
                                            "Входные переменные расчета:\n" +
                                        "Класс пружины - " + Class + "\n" +
                                        "Разряд пружины - " + Rank + "\n" +
                                        "Наружный диаметр пружины - " + D1 + " мм\n" +
                                        "Относительный инерционный зазор пружины - " + Delta + "\n" +
                                        "Сила пружины при предварительной деформации - " + F1 + " Н\n" +
                                        "Сила пружины при рабочей деформации - " + F2 + " Н\n" +
                                        "Рабочий ход пружины - " + h + " мм\n" +
                                        "Допускаемое касательное напряжение пружины - " + tau + " МПа\n" +
                                        "Диаметр проволоки - " + d + " мм\n" +
                                        "Число рабочих витков пружины - " + n + "\n" +
                                        "Число опорных витков (с двух сторон) - " + n2 + "\n" +
                                        "Число обработанных витков (с двух сторон) - " + n3 + "\n" +
                                        "Наибольшая скорость перемещения подвижного конца пружины при нагружении или разгрузке - " +
                                        vMax + " м/с\n" +
                                        "Модуль сдвига - " + G + " МПа\n" +
                                        "Динамическая (гравитационная) плотность материала - " + Ro + " Н*с^2/м^4\n\n" +

                                        "Выходные переменные расчета:\n" +
                                        "Сила пружины при максимальной деформации - " + F3 + " Н\n" +
                                        "Средний диаметр пружины - " + Ds + " мм\n" +
                                        "Внутренний диаметр пружины - " + D2 + " мм\n" +
                                        "Индекс пружины - " + i + " \n" +
                                        "Коэффициент, учитывающий кривизну витка пружины - " + k + " \n" +
                                        "Жесткость пружины - " + c + " Н/мм\n" +
                                        "Жесткость одного витка пружины - " + c1 + " Н/мм\n" +
                                        "Полное число витков пружины - " + n1 + " \n" +
                                        "Предварительная деформация пружины - " + s1 + " мм\n" +
                                        "Рабочая деформация пружины - " + s2 + " мм\n" +

                                        "Максимальная деформация пружины - " + s3 + " мм\n" +
                                        "Максимальная деформация одного витка пружины - " + s_3 + " мм\n" +
                                        "Шаг пружины в свободном состоянии - " + t + " мм\n" +
                                        "Длина пружины в свободном состоянии - " + l0 + " мм\n" +
                                        "Длина пружины при предварительной деформации - " + l1 + " мм\n" +
                                        "Длина пружины при рабочей деформации - " + l2 + " мм\n" +
                                        "Длина пружины при максимальной деформации - " + l3 + " мм\n" +
                                        "Напряжение в пружине при предварительной деформации - " + tau1 + " МПа\n" +
                                        "Напряжение в пружине при рабочей деформации - " + tau2 + " МПа\n" +
                                        "Максимальное касательное напряжение пружины - " + tau3 + " МПа\n" +
                                        "Длина развернутой пружины - " + l + " мм\n" +
                                        "Масса пружины - " + m + " кг\n" +
                                        "Объем, занимаемый пружиной - " + V + " мм^3\n" +
                                        "Максимальная энергия, накапливаемая пружиной, или работа деформации - " + U + " мДж\n" +
                                        "Критическая скорость пружины - " + vK + " м/с\n" +
                                        "Сила пружины при максимальной деформации соответствует выбранному классу и разряду - " + conditionF3 + " \n" +
                                        "Критическая скорость пружины ниже максимальной - " + conditionvK + " \n";

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
                            report.Title = String.Concat(count.ToString(), ". Расчет винтовых цилиндрических пружин");
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
            await y.CreateCalcAsync("CalcOfHelicalCylSpringsPage");
        });
        public ICommand DeleteBookmark => new Command(async () =>
        {
            BookmarkViewModel y = new BookmarkViewModel();
            await y.DeleteCalcAsync("CalcOfHelicalCylSpringsPage");
        });
    }
}
