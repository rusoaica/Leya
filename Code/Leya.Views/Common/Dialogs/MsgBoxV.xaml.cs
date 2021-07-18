/// Written by: Yulia Danilova
/// Creation Date: 09th of November, 2020
/// Purpose: Code behind for the MsgBoxV window
#region ========================================================================= USING =====================================================================================
using System.Windows;
using Leya.Views.Common.Dialogs.MessageBox;
using Leya.ViewModels.Common.Dialogs.MessageBox;
#endregion

namespace Leya.Views.Common.Dialogs
{
    /// <summary>
    /// Interaction logic for MsgBoxV.xaml
    /// </summary>
    public partial class MsgBoxV : Window, IMsgBoxView
    {
        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="param">The ViewModel to inject</param>
        public MsgBoxV(MsgBoxVM param)
        {
            InitializeComponent();
            DataContext = param;
        }

        /// <summary>
        /// Default c-tor
        /// </summary>
        public MsgBoxV()
        {
            InitializeComponent();
        }
        #endregion
    }
}
