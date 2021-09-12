/// Written by: Yulia Danilova
/// Creation Date: 09th of September, 2021
/// Purpose: Interface business model for system options
#region ========================================================================= USING =====================================================================================
using System;
using System.Threading.Tasks;
#endregion

namespace Leya.Models.Core.Options
{
    public interface IOptionsSystem
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        event Action<string> PropertyChanged;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        bool UsesDatabaseForStorage { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the system options from the application's configuration
        /// </summary>        
        void GetSystemOptions();

        /// <summary>
        /// Updates the application's configurations for the system options
        /// </summary>
        Task UpdateSystemOptionsAsync();
        #endregion
    }
}
