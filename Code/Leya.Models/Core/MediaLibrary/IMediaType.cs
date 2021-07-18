/// Written by: Yulia Danilova
/// Creation Date: 06th of July, 2021
/// Purpose: Interface business model for media types
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.Models.Common.Models.Media;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public interface IMediaType
    {
        #region ================================================================ PROPERTIES =================================================================================
        MediaTypeEntity[] MediaTypes { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets all the media types from the storage medium
        /// </summary>
        Task GetMediaTypesAsync();
        #endregion
    }
}
