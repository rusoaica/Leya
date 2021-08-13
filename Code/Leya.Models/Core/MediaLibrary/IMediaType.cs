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

        /// <summary>
        /// Deletes an entity identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the entity to be deleted</param>
        Task DeleteMediaTypeAsync(int id);

        Task<int> AddMediaTypeAsync(MediaTypeEntity media);
        #endregion
    }
}
