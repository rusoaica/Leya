/// Written by: Yulia Danilova
/// Creation Date: 07th of July, 2021
/// Purpose: Business model for artists
#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using Leya.DataAccess;
using System.Threading.Tasks;
using Leya.Models.Common.Infrastructure;
using Leya.Models.Common.Models.Artists;
using Leya.DataAccess.Repositories.Artists;
using Leya.Models.Common.Models.Media;
using System.IO;
using Newtonsoft.Json;
#endregion

namespace Leya.Models.Core.Artists
{
    public class Artist : IArtist
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IAlbum albums;
        private readonly IArtistRepository artistRepository;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public ArtistEntity[] Artists { get; set; }
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// <param name="unitOfWork">Injected unit of work for interacting with the data access layer repositories</param>
        /// <param name="albums">Injected album business model</param>
        public Artist(IUnitOfWork unitOfWork, IAlbum albums)
        {
            this.albums = albums;
            artistRepository = unitOfWork.GetRepository<IArtistRepository>();
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Gets the artists from the storage medium
        /// </summary>
        public async Task GetAllAsync()
        {
            await Task.Run(async () =>
            {
                // get all artists
                var result = await artistRepository.GetAllAsync();
                // get all seasons
                await albums.GetAlbumsAsync();
                if (string.IsNullOrEmpty(result.Error))
                {
                    Artists = Services.AutoMapper.Map<ArtistEntity[]>(result.Data);
                    // for each tv show, assign its seasons
                    foreach (ArtistEntity artist in Artists)
                        artist.Albums = albums.Albums.Where(s => s.ArtistId == artist.Id).ToArray();
                }
                else
                    throw new InvalidOperationException("Error getting the artists from the repository: " + result.Error);
            });
        }

        /// <summary>
        /// Deletes the artists from the storage medium
        /// </summary>
        public async Task DeleteAllAsync()
        {
            var result = await artistRepository.DeleteAllAsync();
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error deleting the artists from the repository: " + result.Error);
        }

        /// <summary>
        /// Saves an artist in the storage medium
        /// </summary>
        /// <param name="mediaTypeSource">The media type source of the TV show</param>
        /// <param name="mediaTypeId">The media type id of the TV show</param>
        public async Task SaveAsync(MediaTypeSourceEntity mediaTypeSource, int mediaTypeId)
        {
            // read the tv show details, if any
            if (File.Exists(mediaTypeSource.MediaSourcePath + Path.DirectorySeparatorChar + "artist.nfo"))
            {
                using (StreamReader artistStream = new StreamReader(mediaTypeSource.MediaSourcePath + Path.DirectorySeparatorChar + "artist.nfo"))
                {
                    // deserialize the artist info json
                    ArtistEntity artistEntity = JsonConvert.DeserializeObject<ArtistEntity>(await artistStream.ReadToEndAsync());
                    // assign the media type source 
                    artistEntity.MediaTypeSourceId = mediaTypeSource.Id;
                    artistEntity.MediaTypeId = mediaTypeId;
                    // store the artist storage entity for later, it contains the ids of its albums, no reason to ask the storage for them yet again
                    var artistStorageEntity = artistEntity.ToStorageEntity();
                    // save the artist
                    var result = await artistRepository.InsertAsync(artistStorageEntity);
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new InvalidOperationException("Error inserting the artist in the repository: " + result.Error);
                    else
                    {
                        // iterate all the albums and save their songs by reading their info json files
                        foreach (AlbumEntity album in artistEntity.Albums)
                        {
                            foreach (string songPath in Directory.EnumerateFiles(mediaTypeSource.MediaSourcePath + Path.DirectorySeparatorChar + album.NamedTitle).Where(f => f.EndsWith(".nfo")))
                            {
                                using (StreamReader songStream = new StreamReader(songPath))
                                {
                                    // deserialize the song info json
                                    SongEntity songEntity = JsonConvert.DeserializeObject<SongEntity>(songStream.ReadToEnd());
                                    // assign the media type source 
                                    songEntity.ArtistId = result.Data[0].Id;
                                    songEntity.AlbumId = artistStorageEntity.Albums.Where(s => s.NamedTitle == album.NamedTitle).First().Id;
                                    // save the song
                                    await albums.SaveSongAsync(songEntity);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the IsListened status of an artist identified by <paramref name="artistId"/> in the storage medium
        /// </summary>
        /// <param name="artistId">The id of the artist whose status will be updated</param>
        /// <param name="isListened">The IsListened status to be set</param>
        public async Task UpdateIsListenedStatusAsync(int artistId, bool? isListened)
        {
            var result = await artistRepository.UpdateIsListenedStatusAsync(artistId, isListened);
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error updating the IsListened status of the artist: " + result.Error);
        }

        /// <summary>
        /// Updates the IsFavorite status of an artist identified by <paramref name="artistId"/> in the storage medium
        /// </summary>
        /// <param name="artistId">The id of the artist whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        public async Task UpdateIsFavoriteStatusAsync(int artistId, bool isFavorite)
        {
            var result = await artistRepository.UpdateIsFavoriteStatusAsync(artistId, isFavorite);
            if (!string.IsNullOrEmpty(result.Error))
                throw new InvalidOperationException("Error updating the IsFavorite status of the artist: " + result.Error);
        }
        #endregion
    }
}
