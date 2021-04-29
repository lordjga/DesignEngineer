using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using DesignEngineer.Annotations;
using DesignEngineer.Models;
using Xamarin.Forms;

namespace DesignEngineer.ViewModels
{
    public class ConverterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _metr = true;
        private int i = 0;
        private List<double> numbers;
        LengthModel lengthModel { get; set; }
        public ConverterViewModel()
        {
            lengthModel = new LengthModel();
        }
        public double Metr
        {
            get { return lengthModel.Metr; }
            set
            {
                if (lengthModel.Metr != value)
                {
                    lengthModel.Metr = value;
                    _metr = false;
                    switch (i)
                    {
                        case 0:
                            Foot = Math.Round(lengthModel.Metr * 3.28084, 5);
                            Yard = Math.Round(lengthModel.Metr * 1.093613, 5);
                            Inch = Math.Round(lengthModel.Metr * 39.37008, 5);
                            SeaMile = Math.Round(lengthModel.Metr * 0.00054, 5);
                            LandMile = Math.Round(lengthModel.Metr * 0.000621, 5);
                            break;
                        case 1:
                            Yard = Math.Round(lengthModel.Metr * 1.093613, 5);
                            Inch = Math.Round(lengthModel.Metr * 39.37008, 5);
                            SeaMile = Math.Round(lengthModel.Metr * 0.00054, 5);
                            LandMile = Math.Round(lengthModel.Metr * 0.000621, 5);
                            break;
                        case 2:
                            Foot = Math.Round(lengthModel.Metr * 3.28084, 5);
                            Inch = Math.Round(lengthModel.Metr * 39.37008, 5);
                            SeaMile = Math.Round(lengthModel.Metr * 0.00054, 5);
                            LandMile = Math.Round(lengthModel.Metr * 0.000621, 5);
                            break;
                        case 3:
                            Foot = Math.Round(lengthModel.Metr * 3.28084, 5);
                            Yard = Math.Round(lengthModel.Metr * 1.093613, 5);
                            SeaMile = Math.Round(lengthModel.Metr * 0.00054, 5);
                            LandMile = Math.Round(lengthModel.Metr * 0.000621, 5);
                            break;
                        case 4:
                            Foot = Math.Round(lengthModel.Metr * 3.28084, 5);
                            Yard = Math.Round(lengthModel.Metr * 1.093613, 5);
                            Inch = Math.Round(lengthModel.Metr * 39.37008, 5);
                            LandMile = Math.Round(lengthModel.Metr * 0.000621, 5);
                            break;
                        case 5:
                            Foot = Math.Round(lengthModel.Metr * 3.28084, 5);
                            Yard = Math.Round(lengthModel.Metr * 1.093613, 5);
                            Inch = Math.Round(lengthModel.Metr * 39.37008, 5);
                            SeaMile = Math.Round(lengthModel.Metr * 0.00054, 5);
                            break;
                    }
                    _metr = true;
                    OnPropertyChanged("Metr");
                }
                    
                
                
            }
        }
        public double Foot
        {
            get { return lengthModel.Foot; }
            set
            {
                if (lengthModel.Foot != value)
                {
                    lengthModel.Foot = value;
                    i = 1;
                   if (_metr)
                       Metr = Math.Round(lengthModel.Foot / 3.28084, 5);
                   i = 0;
                    OnPropertyChanged("Foot");
                }
            }
        }
        public double Yard
        {
            get { return lengthModel.Yard; }
            set
            {
                if (lengthModel.Yard != value)
                {
                    lengthModel.Yard = value;
                    i = 2;
                    if (_metr) 
                        Metr = Math.Round(lengthModel.Yard / 1.093613, 5);
                    i = 0;
                    OnPropertyChanged("Yard");
                }
            }
        }
        public double Inch
        {
            get { return lengthModel.Inch; }
            set
            {
                lengthModel.Inch = value;
                i = 3;
                if (_metr)
                    Metr = Math.Round(lengthModel.Inch / 39.37008, 5);
                i = 0;
                OnPropertyChanged("Inch");
            }
        }

        public double SeaMile
        {
            get { return lengthModel.SeaMile; }
            set
            {
                lengthModel.SeaMile = value;
                i = 4;
                if (_metr)
                    Metr = Math.Round(lengthModel.SeaMile / 0.00054, 5);
                i = 0;
                OnPropertyChanged("SeaMile");
            }
        }
        public double LandMile
        {
            get { return lengthModel.LandMile; }
            set
            {
                lengthModel.LandMile = value;
                i = 5;
                if (_metr)
                    Metr = Math.Round(lengthModel.SeaMile / 0.000621, 5);
                i = 0;
                OnPropertyChanged("LandMile");
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
/*
         #region LengthCalc
         //         
        
         
         #endregion
       */