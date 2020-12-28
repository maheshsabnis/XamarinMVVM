using MVVM_XamarinApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MVVM_XamarinApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("ddd", ((Button)sender).Text, "Cancel");
        }

        //private async void btnGet_Clicked(object sender, EventArgs e)
        //{
        //    ProductService serv = new ProductService();
        //    var res = await serv.GetData();


        //}
    }
}
