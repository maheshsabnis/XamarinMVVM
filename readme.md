# This project is an implementation of MVVM using Xamarin

Step 1: Create a new Xamarin Form Application and name it as MVVM_XamarinApp. In this project add 
 new folders of name Models, Services, ViewModels and Views.

Step 2: In the Models folder add a new class file and name it as ProductInfo.cs. In this file add 
the following code

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

The above ProductInfo class implemente INotifyPropertyChanged interface and implements its "PropertyChanged"
event. The method "OnPropertyChanged()" raised the "PropertyChanged" event. This method is called in
each proeprty of the PropertyInfo class


Step 3: In the "Services" folder, add a new class file and name it as ProductService.cs. In this file
add the following code. The class in this code contains method to perform GET /POST requests to the
REST APIs


using MVVM_XamarinApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_XamarinApp.Services
{
    public class ProductService
    {
        string url;
        HttpClient client;

        public ProductService()
        {
            url = "https://apiapptrainingnewapp.azurewebsites.net/api/Products";
            client = new HttpClient();
        }

        public async Task<List<ProductInfo>> GetData()
        {
            List<ProductInfo> products = new List<ProductInfo>();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<ProductInfo>>(content);
            }

            return products;
        }

        public async Task<ProductInfo> PostProduct(ProductInfo product)
        {
            
            string json = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                string respData = await response.Content.ReadAsStringAsync();
                product = JsonConvert.DeserializeObject<ProductInfo>(respData);
            }
            return product;
        }
    }
}



Step 4: In the ViewModels folder add a new class file. Name thid file as ProductViewModel.cs. This file
contains the ViewModel class. This class will contain properties and command objects. These properties and
command objects will be bind with the XAML.



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


Step 5: Modify the MaingPage.xaml for DataBinding


<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MVVM_XamarinApp.MainPage"
             xmlns:viewmodel="clr-namespace:MVVM_XamarinApp.ViewModels">
    <ContentPage.Resources>
        <ResourceDictionary>
            <viewmodel:ProductViewModel x:Key="prductViewModel"></viewmodel:ProductViewModel>
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout BindingContext="{Binding Source={StaticResource prductViewModel}}">
        <Label Text="The List of Products"></Label>

        <Button x:Name="btnGet" Text="Get Data" 
                Command="{Binding Path=GetCommand}" ></Button>
        
        <ListView x:Name="listViewProduct" 
                  ItemsSource="{Binding Products}"
                  >
            <ListView.ItemTemplate>
                <DataTemplate>
                    <ViewCell>
                        <StackLayout Orientation="Horizontal">
                            <Label Text="{Binding Path=ProductRowId}"></Label>
                            <Label Text="{Binding Path=ProductId}"></Label>
                            <Label Text="{Binding Path=ProductName}"></Label>
                            <Label Text="{Binding Path=CategoryName}"></Label>
                            <Label Text="{Binding Path=Manufacturer}"></Label>
                            <Label Text="{Binding Path=Description}"></Label>
                            <Label Text="{Binding Path=BasePrice}"></Label>
                            <Button Text="Update" BackgroundColor="Red" Clicked="Button_Clicked"></Button>
                        </StackLayout>
                    </ViewCell>
                </DataTemplate>
            </ListView.ItemTemplate>
            
        </ListView>
        <Button x:Name="btnNavigate" Text="Navigate" IsEnabled="{Binding IsAddEnabled}" Command="{Binding NavigationCommand}"></Button>
    </StackLayout>
</ContentPage>

The above xaml contain the command binding for the Commanding properties from the ViewModel class.

Step 5: In Views, add a new file and name it as AddProductPage.xaml. Modify the page as follows

<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MVVM_XamarinApp.Views.AddProductPage"
             xmlns:local="clr-namespace:MVVM_XamarinApp.ViewModels">
    <ContentPage.Resources>
        <ResourceDictionary>
            <local:ProductViewModel x:Key="viewmodel"/>
        </ResourceDictionary>
    </ContentPage.Resources>
    <ContentPage.Content>
        <StackLayout BindingContext="{Binding Source={StaticResource viewmodel}}">
            <Label Text="Add Product" WidthRequest="60" TextColor="Yellow"  BackgroundColor="Red"/>

            <StackLayout Orientation="Horizontal">
                <Label Text="Product Row Id"></Label>
                <Entry Placeholder="Enter Product Row Id" Text="{Binding Path=Product.ProductRowId}" IsReadOnly="True"></Entry>
            </StackLayout>
            <StackLayout Orientation="Horizontal">
                <Label Text="Product Id"></Label>
                <Entry Placeholder="Enter Product Id" Text="{Binding Path=Product.ProductId}"></Entry>
            </StackLayout>
            <StackLayout Orientation="Horizontal">
                <Label Text="Product Name"></Label>
                <Entry Placeholder="Enter Product Name" Text="{Binding Path=Product.ProductName}"></Entry>
            </StackLayout>
            <StackLayout Orientation="Horizontal">
                <Label Text="Category Name"></Label>
                <Entry Placeholder="Enter Category Name" Text="{Binding Path=Product.CategoryName}"></Entry>
            </StackLayout>
            <StackLayout Orientation="Horizontal">
                <Label Text="Manufacturer"></Label>
                <Entry Placeholder="Enter Manufacturer" Text="{Binding Path=Product.Manufacturer}"></Entry>
            </StackLayout>
            <StackLayout Orientation="Horizontal">
                <Label Text="Description"></Label>
                <Editor Placeholder="Enter Description" Text="{Binding Path=Product.Description}"></Editor>
            </StackLayout>
            <StackLayout Orientation="Horizontal">
                <Label Text="Base Price"></Label>
                <Entry Placeholder="Enter Base Price" Text="{Binding Path=Product.BasePrice}"></Entry>
            </StackLayout>

            <StackLayout Orientation="Horizontal">
                <Button Text="Add Product" Command="{Binding AddProductCommand}"
                        ></Button>
            </StackLayout>
            
        </StackLayout>
    </ContentPage.Content>
</ContentPage>


Run the application. 



