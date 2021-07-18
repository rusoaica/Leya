/// Written by: Yulia Danilova
/// Creation Date: 11th of December, 2020
/// Purpose: Media type repository interface for the bridge-through between the generic storage medium and the storage medium for MediaTypes
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Common.Models.Media;
using Leya.DataAccess.Repositories.Common;
#endregion

namespace Leya.DataAccess.Repositories.Media
{
    public interface IMediaTypeRepository : IRepository<MediaTypeEntity> 
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets a media type by name from the generic storage medium
        /// </summary>
        /// <param name="name">The name of the media type to get</param>
        /// <returns>An <see cref="ApiResponse{MediaTypeEntity}"/> API wrapper around a media type identified by <paramref name="name"/></returns>
        Task<ApiResponse<MediaTypeEntity>> GetByNameAsync(string name);
        #endregion
    }
}
