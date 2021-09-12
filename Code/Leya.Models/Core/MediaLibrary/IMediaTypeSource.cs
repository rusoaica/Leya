/// Written by: Yulia Danilova
/// Creation Date: 06th of July, 2021
/// Purpose: Interface business model for media type sources
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.Models.Common.Models.Media;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public interface IMediaTypeSource
    {
        #region ================================================================ PROPERTIES =================================================================================
        MediaTypeSourceEntity[] MediaTypeSources { get; set; }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets all the media type sources from the storage medium
        /// </summary>
        Task GetMediaTypeSourcesAsync();

        /// <summary>
        /// Inserts <paramref name="media"/> into the storage medium
        /// </summary>
        /// <param name="media">The media type source to be inserted</param>
        /// <returns>The id of the inserted media type source</returns>
        Task<int> InsertMediaTypeSource(MediaTypeSourceEntity media);
        #endregion
    }
}
