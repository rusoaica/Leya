/// Written by: Yulia Danilova
/// Creation Date: 11th of December, 2020
/// Purpose: Media type repository interface for the bridge-through between the generic storage medium and the storage medium for MediaTypes
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Models.Media;
#endregion

namespace Leya.DataAccess.Repositories.Media
{
    internal sealed class MediaTypeRepository : IMediaTypeRepository
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IDataAccess dataAccess;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="dataAccess">The injected data access to use</param>
        public MediaTypeRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Deletes all media types from the storage medium
        /// </summary>
        /// <returns>The result of deleting the media types, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteAllAsync()
        {
            // delete all media types and their associated data 
            dataAccess.OpenTransaction();
            ApiResponse deleteMediaTypes = await dataAccess.DeleteAsync(EntityContainers.MediaTypes);
            ApiResponse deleteMediaTypeSources = await dataAccess.DeleteAsync(EntityContainers.MediaTypeSources);
            dataAccess.CloseTransaction();
            // check is any of the queries resulted in an error
            if (string.IsNullOrEmpty(deleteMediaTypes.Error) && string.IsNullOrEmpty(deleteMediaTypes.Error) && string.IsNullOrEmpty(deleteMediaTypeSources.Error))
                return deleteMediaTypes;
            else
                return new ApiResponse() { Error = "Error deleting all media types!" };
        }

        /// <summary>
        /// Deletes a media type identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the media to be deleted</param>
        /// <returns>The result of deleting the media type, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteByIdAsync(int id)
        {
            dataAccess.OpenTransaction();
            // delete the media type
            ApiResponse mediaTypes = await dataAccess.DeleteAsync(EntityContainers.MediaTypes, new { Id = id });
            // delete the media type sources associated with it
            ApiResponse mediaTypeSources = await dataAccess.DeleteAsync(EntityContainers.MediaTypeSources, new { MediaTypeId = id });
            dataAccess.CloseTransaction();
            // check if any of the queries returned an error
            if (string.IsNullOrEmpty(mediaTypes.Error) && string.IsNullOrEmpty(mediaTypeSources.Error))
                return mediaTypes;
            else
                return new ApiResponse() { Error = "Error deleting media sources!" };
        }

        /// <summary>
        /// Gets the media types from the storage medium
        /// </summary>
        /// <returns>A list of media types, wrapped in a generic API container of type <see cref="ApiResponse{MediaTypeEntity}"/></returns>
        public async Task<ApiResponse<MediaTypeEntity>> GetAllAsync()
        {
            return await dataAccess.SelectAsync<MediaTypeEntity>(EntityContainers.MediaTypes);
        }

        /// <summary>
        /// Gets a media type identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the media type to get</param>
        /// <returns>A media type identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{MediaTypeEntity}"/></returns>
        public async Task<ApiResponse<MediaTypeEntity>> GetByIdAsync(int id)
        {
            return await dataAccess.SelectAsync<MediaTypeEntity>(EntityContainers.MediaTypes, "Id, MediaName, MediaType, IsMedia", new { Id = id });
        }

        /// <summary>
        /// Gets a media type by name from the storage medium
        /// </summary>
        /// <param name="name">The name of the media type to get</param>
        /// <returns>A media type identified by <paramref name="name"/>, wrapped in a generic API container of type <see cref="ApiResponse{MediaTypeEntity}"/></returns>
        public async Task<ApiResponse<MediaTypeEntity>> GetByNameAsync(string name)
        {
            return await dataAccess.SelectAsync<MediaTypeEntity>(EntityContainers.MediaTypes, "Id, MediaName, MediaType, IsMedia", new { MediaName = name });
        }

        /// <summary>
        /// Saves a media type in the storage medium
        /// </summary>
        /// <param name="entity">The entity type to be saved</param>
        /// <returns>The result of saving <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse{MediaTypeEntity}"/></returns>
        public async Task<ApiResponse<MediaTypeEntity>> InsertAsync(MediaTypeEntity entity)
        {
            return await dataAccess.InsertAsync(EntityContainers.MediaTypes, entity);
        }

        /// <summary>
        /// Updates <paramref name="entity"/> in the storage medium
        /// </summary>
        /// <param name="entity">The entity that will be updated</param>
        /// <returns>The result of updating <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateAsync(MediaTypeEntity entity)
        {
            return await dataAccess.UpdateAsync(EntityContainers.MediaTypes, "MediaName = '" + entity.MediaName + "', MediaType = '" + entity.MediaType + "', IsMedia = '" + entity.IsMedia + "'", "Id", "'" + entity.Id + "'");
        }
        #endregion
    }
}
