using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Autofac;
using MVVM_XamarinApp.Services;
using MVVM_XamarinApp.ViewModels;
namespace MVVM_XamarinApp
{
    public partial class App : Application
    {
       // static IContainer container;
        static readonly ContainerBuilder builder = new ContainerBuilder();
        public App()
        {
            InitializeComponent();


            builder.RegisterType<ProductService>();
            builder.RegisterType<ProductViewModel>();


            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
