/// Written by: Yulia Danilova
/// Creation Date: 11th of November, 2020
/// Purpose: Code behind for the StartupV view
#region ========================================================================= USING =====================================================================================
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using Leya.ViewModels.Startup;
#endregion

namespace Leya.Views.Startup
{
    public partial class StartupV : Window, IStartupView
    {
        #region ================================================================ PROPERTIES =================================================================================
        public static StartupV Instance { get; private set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public StartupV()
        {
            AvaloniaXamlLoader.Load(this);
            Instance = this;
#if DEBUG
            this.AttachDevTools();
#endif            
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Shows the current window as a modal dialog
        /// </summary>
        public async Task<bool?> ShowDialog()
        {
            return await ShowDialog<bool?>(Instance);
        }
        #endregion
    }
}