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

            container.PerRequest<MainViewModel>()
                     .Singleton<IGraphSpace, GraphSpace>(); // bo popatrz tutaj jest to jakies kurwa szare, a dla , przeciez jest win8 ustawiony jako start up...
#if WINDOWS_APP
                     container.PerRequest<BluetoothViewModel>() ;
#endif
                     
            
        }//hm, mimo wszystko sie wykonuje, popierdolone to jest...zastanawia mnie czemu sie nie przestawia ten viewmodel...powiedz mi czy to Shared to jest jakis inny Assembly niż reszta projektu ? hm mów po ludzku :D, hm, chyba ten sam*

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