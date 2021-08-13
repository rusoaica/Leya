/// Written by: Yulia Danilova
/// Creation Date: 06th of July, 2021
/// Purpose: Business model for media type sources
#region ========================================================================= USING =====================================================================================
using System;
using Leya.DataAccess;
using System.Threading.Tasks;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Infrastructure;
using Leya.DataAccess.Repositories.Media;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public class MediaTypeSource : IMediaTypeSource
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IMediaTypeSourceRepository mediaTypeSourceRepository;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public MediaTypeSourceEntity[] MediaTypeSources { get; set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
        public MediaTypeSource(IUnitOfWork unitOfWork)
        {
            mediaTypeSourceRepository = unitOfWork.GetRepository<IMediaTypeSourceRepository>();
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the media type sources from the storage medium
        /// </summary>
        public async Task GetMediaTypeSourcesAsync()
        {
            // get the media type sources from the storage medium
            var result = await mediaTypeSourceRepository.GetAllAsync();
            if (string.IsNullOrEmpty(result.Error))
                MediaTypeSources = Services.AutoMapper.Map<MediaTypeSourceEntity[]>(result.Data);
            else
                throw new InvalidOperationException("Error getting the media type sources from the repository: " + result.Error);
        }

        public async Task<int> InsertMediaTypeSource(MediaTypeSourceEntity media)
        {
            var result = await mediaTypeSourceRepository.InsertAsync(media.ToStorageEntity());
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error inserting the media type source in the repository: " + result.Error);
            else
                return result.Data[0].Id;
        }
        #endregion
    }
}
