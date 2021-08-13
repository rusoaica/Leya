/// Written by: Avalonia
/// Creation Date: 10th of June, 2021
/// Purpose: Application's entry point class
#region ========================================================================= USING =====================================================================================
using Avalonia;
#endregion

namespace Leya.ViewsAvalonia
{
    internal class Program
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Initialization code. Don't use any Avalonia, third-party APIs or any SynchronizationContext-reliant code before AppMain is called: things aren't initialized yet and stuff might break.
        /// </summary>
        public static void Main(string[] args)
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        /// <summary>
        /// Avalonia configuration, don't remove; also used by visual designer.
        /// </summary>
        /// <returns>Platform-specific services for an Avalonia.Application</returns>
        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                             .UsePlatformDetect()
                             .UseSkia()
                             .LogToTrace();
        }
        #endregion
    }
}
