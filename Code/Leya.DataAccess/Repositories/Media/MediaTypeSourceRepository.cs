/// Written by: Yulia Danilova
/// Creation Date: 17th of November, 2020
/// Purpose: Media type sources repository for the bridge-through between the generic storage medium and storage medium for MediaTypeSources
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Models.Media;
#endregion

namespace Leya.DataAccess.Repositories.Media
{
    internal sealed class MediaTypeSourceRepository : IMediaTypeSourceRepository
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IDataAccess dataAccess;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="dataAccess">The injected data access to use</param>
        public MediaTypeSourceRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Deletes all media type sources from the storage medium
        /// </summary>
        /// <returns>The result of deleting the media type sources, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteAllAsync()
        {
            // delete all media type sources
            ApiResponse deleteMediaTypeSources = await dataAccess.DeleteAsync(EntityContainers.MediaTypeSources);
            dataAccess.CloseTransaction();
            // check if the query resulted in an error
            if (string.IsNullOrEmpty(deleteMediaTypeSources.Error))
                return deleteMediaTypeSources;
            else
                return new ApiResponse() { Error = "Error deleting all media type sources!" };
        }

        /// <summary>
        /// Deletes a media type source identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the media to be deleted</param>
        /// <returns>The result of deleting the media type, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteByIdAsync(int id)
        {
            // delete the media type sources
            ApiResponse mediaTypeSources = await dataAccess.DeleteAsync(EntityContainers.MediaTypeSources, new { Id = id });
            // check if any of the query returned an error
            if (string.IsNullOrEmpty(mediaTypeSources.Error))
                return mediaTypeSources;
            else
                return new ApiResponse() { Error = "Error deleting the media type source!" };
        }

        /// <summary>
        /// Gets all media type sources from the storage medium
        /// </summary>
        /// <returns>A list of media type sources, wrapped in a generic API container of type <see cref="ApiResponse{MediaTypeSourceEntity}"/></returns>
        public async Task<ApiResponse<MediaTypeSourceEntity>> GetAllAsync()
        {
            return await dataAccess.SelectAsync<MediaTypeSourceEntity>(EntityContainers.MediaTypeSources, "Id, MediaTypeId, MediaSourcePath");
        }

        /// <summary>
        /// Gets the sources of the media type identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The Id of the media type for which to get the sources</param>
        /// <returns>A list of media type sources of the media type identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{MediaTypeSourceEntity}"/></returns>
        public async Task<ApiResponse<MediaTypeSourceEntity>> GetByIdAsync(int id)
        {
            return await dataAccess.SelectAsync<MediaTypeSourceEntity>(EntityContainers.MediaTypeSources, "Id, MediaTypeId, MediaSourcePath", new { MediaTypeId = id });
        }

        /// <summary>
        /// Saves a media type source in the storage medium
        /// </summary>
        /// <param name="entity">The media type source to be saved</param>
        /// <returns>The result of saving <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse{MediaTypeSourceEntity}"/></returns>
        public async Task<ApiResponse<MediaTypeSourceEntity>> InsertAsync(MediaTypeSourceEntity entity)
        {
            return await dataAccess.InsertAsync(EntityContainers.MediaTypeSources, entity);
        }

        /// <summary>
        /// Updates <paramref name="entity"/> in the storage medium
        /// </summary>
        /// <param name="entity">The entity that will be updated</param>
        /// <returns>The result of updating <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateAsync(MediaTypeSourceEntity entity)
        {
            return await dataAccess.UpdateAsync(EntityContainers.MediaTypeSources, "MediaTypeId = '" + entity.MediaTypeId + "', MediaSourcePath = '" + entity.MediaSourcePath + "'", "Id", "'" + entity.Id + "'");
        }
        #endregion
    }
}
