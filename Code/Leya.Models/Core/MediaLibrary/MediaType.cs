/// Written by: Yulia Danilova
/// Creation Date: 06th of July, 2021
/// Purpose: Business model for media types
#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using Leya.DataAccess;
using System.Threading.Tasks;
using System.Collections.Generic;
using Leya.Models.Common.Models.Media;
using Leya.Models.Common.Infrastructure;
using Leya.DataAccess.Repositories.Media;
#endregion

namespace Leya.Models.Core.MediaLibrary
{
    public class MediaType : IMediaType
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IMediaTypeSource mediaTypeSources;
        private readonly IMediaTypeRepository mediaTypeRepository;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public MediaTypeEntity[] MediaTypes { get; set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
        /// <param name="mediaTypeSources">Injected media type source business model</param>
        public MediaType(IUnitOfWork unitOfWork, IMediaTypeSource mediaTypeSources)
        {
            mediaTypeRepository = unitOfWork.GetRepository<IMediaTypeRepository>();
            this.mediaTypeSources = mediaTypeSources;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the media types from the storage medium
        /// </summary>
        public async Task GetMediaTypesAsync()
        {
            await Task.Run(async () =>
            {
                // get all media types
                var result = await mediaTypeRepository.GetAllAsync();
                // get all media type sources
                await mediaTypeSources.GetMediaTypeSourcesAsync();
                if (string.IsNullOrEmpty(result.Error))
                {
                    // if the media types count is 0, this is the first program execution - create the default media types that are always present
                    if (result.Count == 0)
                        MediaTypes = (await InsertDefaultMediaItemsAsync()).OrderByDescending(e => e.IsMedia).ToArray();
                    else
                    {
                        MediaTypes = Services.AutoMapper.Map<MediaTypeEntity[]>(result.Data).OrderByDescending(e => e.IsMedia).ToArray();
                        // for each media type, assign its media type sources
                        foreach (MediaTypeEntity mediaType in MediaTypes)
                            mediaType.MediaTypeSources = mediaTypeSources.MediaTypeSources.Where(e => e.MediaTypeId == mediaType.Id).ToArray();
                    }
                }
                else
                    throw new InvalidOperationException("Error getting the media types from the repository: " + result.Error);
            });
        }

        /// <summary>
        /// Creates the default media types that are always present
        /// </summary>
        /// <returns>A list of media types that are always present</returns>
        private async Task<List<MediaTypeEntity>> InsertDefaultMediaItemsAsync()
        {
            await AddMediaType(new MediaTypeEntity() { MediaName = "SEARCH" });
            await AddMediaType(new MediaTypeEntity() { MediaName = "FAVORITES" });
            await AddMediaType(new MediaTypeEntity() { MediaName = "SYSTEM" });
            return new List<MediaTypeEntity>()
            {
                new MediaTypeEntity() { MediaName = "SEARCH", Id = 1 },
                new MediaTypeEntity() { MediaName = "FAVORITES", Id = 2 },
                new MediaTypeEntity() { MediaName = "SYSTEM", Id = 0 },
            };
        }

        /// <summary>
        /// Saves <paramref name="media"/> in the storage medium
        /// </summary>
        /// <param name="media">The media to be saved</param>
        private async Task AddMediaType(MediaTypeEntity media)
        {
            var result = await mediaTypeRepository.InsertAsync(media.ToStorageEntity());
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error inserting the media type in the repository: " + result.Error);
        }
        #endregion
    }
}
