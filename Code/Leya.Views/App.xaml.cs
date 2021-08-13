/// Written by: Yulia Danilova
/// Creation Date: 10th of June, 2021
/// Purpose: Application's entry point class
#region ========================================================================= USING =====================================================================================
using Autofac;
using System.Windows;
using Leya.ViewModels.Startup;
using Leya.Views.Common.Configuration;
using Leya.ViewModels.Common.ViewFactory;
#endregion

namespace Leya
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Raises the Application.Startup event
        /// </summary>
        /// <param name="e">A StartupEventArgs that contains the event data</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            // configure the dependency injection services
            IContainer container = DIContainerConfig.Configure();
            //DIContainerConfig.AddRepositories(container);
            // begin the application's lifetime score
            using (ILifetimeScope scope = container.BeginLifetimeScope())
            {
                // get a view factory from the DI container and display the startup view from it, as modal dialog
                IViewFactory factory = container.Resolve<IViewFactory>();
                IStartupView main = factory.CreateView<IStartupView>();
                await main.ShowDialog();
            }
        }
        #endregion
    }
}
