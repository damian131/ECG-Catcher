using System;
using System.Collections.Generic;
using Caliburn.Micro;
using ECGCatcher.Views;
using ECGCatcher.ViewModels;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using ECGCatcher.Models;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace ECGCatcher
{
    public sealed partial class App
    {
        private WinRTContainer container;

        public App()
        {
            InitializeComponent();
        }

        protected override void Configure()
        {
            container = new WinRTContainer();

            container.RegisterWinRTServices();

            container.Singleton<MainViewModel>()
                     .Singleton<IGraphSpace, GraphSpace>()
                     .Singleton<BluetoothPanelViewModel>();
#if WINDOWS_APP
            container.Singleton<BluetoothViewModel>();
#endif
                     
            
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            container.RegisterNavigationService(rootFrame);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            DisplayRootView<MainView>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}