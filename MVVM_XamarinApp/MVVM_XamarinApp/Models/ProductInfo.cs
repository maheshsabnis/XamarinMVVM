using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MVVM_XamarinApp.Models
{
    public class ProductInfo : INotifyPropertyChanged
    {
        int _ProductRowId;
        string _ProductId;
        string _ProductName;
        string _CategoryName;
        string _Manufacturer;
        string _Description;
        int _BasePrice;

        public int ProductRowId
        {
            get
            {
                return _ProductRowId;
            }
            set
            {
                _ProductRowId = value;
                OnPropertyChanged("ProductRowId");
            }
        }


        public string ProductId
        {
            get
            {
                return _ProductId;
            }
            set
            {
                _ProductId = value;
                OnPropertyChanged("ProductId");
            }
        }

        public string ProductName
        {
            get
            {
                return _ProductName;
            }
            set
            {
                _ProductName = value;
                OnPropertyChanged("ProductName");
            }
        }

        public string CategoryName
        {
            get
            {
                return _CategoryName;
            }
            set
            {
                _CategoryName = value;
                OnPropertyChanged("CategoryName");
            }
        }

        public string Manufacturer
        {
            get
            {
                return _Manufacturer;
            }
            set
            {
                _Manufacturer = value;
                OnPropertyChanged("Manufacturer");
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }

        public int BasePrice
        {
            get
            {
                return _BasePrice;
            }
            set
            {
                _BasePrice = value;
                OnPropertyChanged("BasePrice");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string pName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pName));
            }
        }
    }
}
