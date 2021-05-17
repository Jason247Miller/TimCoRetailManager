using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using TRMDesktopUI.ViewModels;


namespace TRMDesktopUI
{
    public class Bootstrapper : BootstrapperBase
    {
        //handles the instantiation of most of the classes, new key word will seldomly be used 
        private SimpleContainer _container = new SimpleContainer();
        public Bootstrapper()
        {   
            Initialize(); //initialize the bootstrapper processes 
        }
        protected override void Configure()
        {

            //sets the instance equal itself 
            _container.Instance(_container);
            //singletons
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();
            //reflection 
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            //instead of launching main.xaml launch our code in the SHellViewModel
            DisplayRootViewFor<ShellViewModel>(); 
        }
        protected override object GetInstance(Type service, string key)
        {   //gets the instance of the type and name via a container 
            return _container.GetInstance(service, key);
        }
        protected override IEnumerable<object> GetAllInstances(Type service)
        {  
            return _container.GetAllInstances(service);
        }
        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
