using MVVM_XamarinApp.Models;
using MVVM_XamarinApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Autofac;
namespace MVVM_XamarinApp.ViewModels
{
    /// <summary>
    /// The Base View Model class. This call implements INotifyPropertyChanged interface
    /// and implements its event. This event will be raiase for each property changed in the view
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string pName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pName));
            }
        }

    }

    /// <summary>
    /// The ViewModel class
    /// </summary>
    public class ProductViewModel  : ViewModelBase
    {
        private ProductInfo _Product;

        private ObservableCollection<ProductInfo> _Products;

        private   ProductService service;

        private bool _IsAddEnabled;

        public ProductViewModel()
        {
            
            Product = new ProductInfo();
            Products = new ObservableCollection<ProductInfo>();

            service = new ProductService();
            _IsAddEnabled = false;
            
            ClearCommand = new Command(ClearInputs);
            AddProductCommand = new Command(AddProduct);
            GetCommand = new Command(GetData);
            NavigationCommand = new Command(Navigate); 
        }

 
        // The Command and other properties

        public ICommand AddProductCommand { private set; get; }
        public ICommand ClearCommand { private set; get; }
        public ICommand GetCommand { private set; get; }
        public ICommand NavigationCommand { private set; get; }

        public ProductInfo Product
        {
            get { return _Product; }
            set 
            {
                _Product = value;
                OnPropertyChanged("Product");
            }
        }

        public bool IsAddEnabled
        {
            get { return _IsAddEnabled; }
            set 
            {
                _IsAddEnabled = value;
                OnPropertyChanged("IsAddEnabled");
            }
        }
        public ObservableCollection<ProductInfo> Products
        {
            get { return _Products; }
            set 
            {
                _Products = value;
                OnPropertyChanged("Products");
            }
        }

        private void ClearInputs()
        {
            Product = new ProductInfo();
        }

        /// <summary>
        /// Adding new Products
        /// </summary>
        private async void AddProduct()
        {
           Product =  await service.PostProduct(Product);
            if (Product.ProductRowId > 0)
            {
                 GetData();
                
                await App.Current.MainPage.Navigation.PopModalAsync();
            }
        }
        /// <summary>
        /// Getting the data
        /// </summary>
        private async void GetData()
        {
            Products.Clear();
            foreach (ProductInfo product in await service.GetData())
            {
                _Products.Add(product);
            }
            if (_Products.Count > 0)
            {
                IsAddEnabled = true;
            }
        }

        /// <summary>
        /// Navigate to the other page
        /// </summary>
        private async void Navigate()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new Views.AddProductPage());
        }

    }
}
