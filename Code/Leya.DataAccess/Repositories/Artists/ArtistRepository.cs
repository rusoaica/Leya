/// Written by: Yulia Danilova
/// Creation Date: 09th of December, 2020
/// Purpose: Artist repository interface for the bridge-through between the generic storage medium and storage medium for Artists
#region ========================================================================= USING =====================================================================================
using System.Threading.Tasks;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Common.Models.Albums;
using Leya.DataAccess.Common.Models.Artists;
#endregion

namespace Leya.DataAccess.Repositories.Artists
{
    internal sealed class ArtistRepository : IArtistRepository
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IDataAccess dataAccess;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="dataAccess">The injected data access to use</param>
        public ArtistRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Deletes all artists from the storage medium
        /// </summary>
        /// <returns>The result of deleting the artists, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteAllAsync()
        {
            // delete all artists and their associated data 
            dataAccess.OpenTransaction();
            ApiResponse deleteArtists = await dataAccess.DeleteAsync(EntityContainers.Artists);
            ApiResponse deleteSongs = await dataAccess.DeleteAsync(EntityContainers.Songs);
            ApiResponse deleteArtistGenres = await dataAccess.DeleteAsync(EntityContainers.ArtistGenres);
            ApiResponse deleteArtistTypes = await dataAccess.DeleteAsync(EntityContainers.ArtistTypes);
            ApiResponse deleteAlbums = await dataAccess.DeleteAsync(EntityContainers.Albums);
            dataAccess.CloseTransaction();
            // check if any of the queries resulted in an error
            if (string.IsNullOrEmpty(deleteArtists.Error) && string.IsNullOrEmpty(deleteSongs.Error) && string.IsNullOrEmpty(deleteArtistGenres.Error)
                && string.IsNullOrEmpty(deleteArtistTypes.Error) && string.IsNullOrEmpty(deleteAlbums.Error))
                return deleteArtists;
            else
                return new ApiResponse() { Error = "Error deleting all artists!" };
        }

        /// <summary>
        /// Deletes an artist identified by <paramref name="id"/> from the storage medium
        /// </summary>
        /// <param name="id">The id of the artist to be deleted</param>
        /// <returns>The result of deleting the artist, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> DeleteByIdAsync(int id)
        {
            // delete all artists and their associated data 
            dataAccess.OpenTransaction();
            ApiResponse deleteArtist = await dataAccess.DeleteAsync(EntityContainers.Artists, new { Id = id });
            ApiResponse deleteSongs = await dataAccess.DeleteAsync(EntityContainers.Songs, new { ArtistId = id });
            ApiResponse deleteArtistGenres = await dataAccess.DeleteAsync(EntityContainers.ArtistGenres, new { ArtistId = id });
            ApiResponse deleteArtistTypes = await dataAccess.DeleteAsync(EntityContainers.ArtistTypes, new { ArtistId = id });
            ApiResponse deleteAlbums = await dataAccess.DeleteAsync(EntityContainers.Albums, new { ArtistId = id });
            dataAccess.CloseTransaction();
            // check if any of the queries resulted in an error
            if (string.IsNullOrEmpty(deleteArtist.Error) && string.IsNullOrEmpty(deleteSongs.Error) && string.IsNullOrEmpty(deleteArtistGenres.Error)
                && string.IsNullOrEmpty(deleteArtistTypes.Error) && string.IsNullOrEmpty(deleteAlbums.Error))
                return deleteArtist;
            else
                return new ApiResponse() { Error = "Error deleting the artist!" };
        }

        /// <summary>
        /// Gets all artists and their associated data from the storage medium
        /// </summary>
        /// <returns>A list of artists and their associated data, wrapped in a generic API container of type <see cref="ApiResponse{ArtistEntity}"/></returns>
        public async Task<ApiResponse<ArtistEntity>> GetAllAsync()
        {
            dataAccess.OpenTransaction();
            // get the artists data
            ApiResponse<ArtistEntity> artists = await dataAccess.SelectAsync<ArtistEntity>(EntityContainers.Artists,
                "Id, MediaTypeSourceId, MediaTypeId, ArtistNamedName, ArtistName, MusicBrainzArtistID, Formed, FormedIn, Biography, VideoClipLink, IsDisbanded, IsListened, Created, IsFavorite");
            if (artists.Data != null)
            {
                await Task.Run(async () =>
                {
                    for (int i = 0; i < artists.Data.Length; i++)
                    {
                        // get the data associated with the artist
                        artists.Data[i].Type = (await dataAccess.SelectAsync<ArtistTypeEntity>(EntityContainers.ArtistTypes, "Type, ArtistId, Id", new { ArtistId = artists.Data[i].Id })).Data;
                        artists.Data[i].Genres = (await dataAccess.SelectAsync<ArtistGenreEntity>(EntityContainers.ArtistGenres, "Genre, ArtistId, Id", new { ArtistId = artists.Data[i].Id })).Data;
                        artists.Data[i].Members = (await dataAccess.SelectAsync<BandMemberEntity>(EntityContainers.BandMembers, "Id, ArtistId, Name, Role, `Order`, Thumb", new { ArtistId = artists.Data[i].Id })).Data;
                        artists.Data[i].Albums = (await dataAccess.SelectAsync<AlbumEntity>(EntityContainers.Albums, "Id, ArtistId, Title, NamedTitle, Year, Description, Rating, IsListened, IsFavorite", new { ArtistId = artists.Data[i].Id })).Data;
                    }
                });
            }
            dataAccess.CloseTransaction();
            return artists;
        }

        /// <summary>
        /// Gets an artist whose media type source's id is identified by <paramref name="id"/> and its associated data from the storage medium
        /// </summary>
        /// <param name="id">The id of the media type source of the artist to get</param>
        /// <returns>The artist whose media type source's id is identified by <paramref name="id"/>, wrapped in a generic API container of type <see cref="ApiResponse{ArtistEntity}"/></returns>
        public async Task<ApiResponse<ArtistEntity>> GetByIdAsync(int id)
        {
            dataAccess.OpenTransaction();
            // get the artist data
            ApiResponse<ArtistEntity> artist = await dataAccess.SelectAsync<ArtistEntity>(EntityContainers.Artists,
                "Id, MediaTypeSourceId, MediaTypeId, ArtistNamedName, ArtistName, MusicBrainzArtistID, Formed, FormedIn, Biography, VideoClipLink, IsDisbanded, IsListened, Created, IsFavorite", new { MediaTypeSourceId = id });
            if (artist.Data != null)
            {
                await Task.Run(async () =>
                {
                    // get the data associated with the artist
                    artist.Data[0].Type = (await dataAccess.SelectAsync<ArtistTypeEntity>(EntityContainers.ArtistTypes, "Type, ArtistId, Id", new { ArtistId = artist.Data[0].Id })).Data;
                    artist.Data[0].Genres = (await dataAccess.SelectAsync<ArtistGenreEntity>(EntityContainers.ArtistGenres, "Genre, ArtistId, Id", new { ArtistId = artist.Data[0].Id })).Data;
                    artist.Data[0].Members = (await dataAccess.SelectAsync<BandMemberEntity>(EntityContainers.BandMembers, "Id, ArtistId, Name, Role, `Order`, Thumb", new { ArtistId = artist.Data[0].Id })).Data;
                    artist.Data[0].Albums = (await dataAccess.SelectAsync<AlbumEntity>(EntityContainers.Albums, "Id, ArtistId, Title, NamedTitle, Year, Description, Rating, IsListened, IsFavorite", new { ArtistId = artist.Data[0].Id })).Data;
                });
            }
            dataAccess.CloseTransaction();
            return artist;
        }

        /// <summary>
        /// Saves an artist and its associated data in the storage medium
        /// </summary>
        /// <param name="entity">The artist to be saved</param>
        /// <returns>The result of saving <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse{ArtistEntity}"/></returns>
        public async Task<ApiResponse<ArtistEntity>> InsertAsync(ArtistEntity entity)
        {
            dataAccess.OpenTransaction();
            // insert the artist
            ApiResponse<ArtistEntity> artist = await dataAccess.InsertAsync(EntityContainers.Artists, entity);
            if (artist.Data != null && artist.Data.Length > 0)
            {
                await Task.Run(async () =>
                {
                    // insert the types
                    foreach (ArtistTypeEntity type in entity.Type)
                    {
                        type.ArtistId = artist.Data[0].Id; // TODO: check if assignment is necessary, might already come assigned!
                        await dataAccess.InsertAsync(EntityContainers.ArtistTypes, type);
                    }
                    // insert the genre
                    foreach (ArtistGenreEntity genre in entity.Genres)
                    {
                        genre.ArtistId = artist.Data[0].Id;
                        await dataAccess.InsertAsync(EntityContainers.ArtistGenres, genre);
                    }
                    // insert the albums
                    foreach (AlbumEntity album in entity.Albums)
                    {
                        album.ArtistId = artist.Data[0].Id;
                        await dataAccess.InsertAsync(EntityContainers.Albums, album);
                    }
                });
            }
            dataAccess.CloseTransaction();
            return artist;
        }

        /// <summary>
        /// Updates <paramref name="entity"/> and its additional info in the storage medium
        /// </summary>
        /// <param name="entity">The entity that will be updated</param>
        /// <returns>The result of updating <paramref name="entity"/>, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateAsync(ArtistEntity entity)
        {
            dataAccess.OpenTransaction();
            // update the artist
            ApiResponse artist = await dataAccess.UpdateAsync(EntityContainers.Artists,
                "MediaTypeSourceId = '" + entity.MediaTypeSourceId +
                "', MediaTypeId = '" + entity.MediaTypeId +
                "', ArtistName = '" + entity.ArtistName +
                "', ArtistNamedName = '" + entity.ArtistNamedName +
                "', MusicBrainzArtistID = '" + entity.MusicBrainzArtistID +
                "', Formed = '" + entity.Formed +
                "', FormedIn = '" + entity.FormedIn +
                "', Biography = '" + entity.Biography +
                "', VideoClipLink = '" + entity.VideoClipLink +
                "', IsDisbanded = '" + entity.IsDisbanded +
                "', IsListened = '" + entity.IsListened +
                "', IsFavorite = '" + entity.IsFavorite + "'",
                "Id", "'" + entity.Id + "'");
            // update the data associated with the artist
            await Task.Run(async () =>
            {
                foreach (ArtistTypeEntity type in entity.Type)
                    await dataAccess.UpdateAsync(EntityContainers.ArtistTypes, "Type = '" + type.Type + "'", "ArtistId", "'" + entity.Id + "'");
                foreach (ArtistGenreEntity genre in entity.Genres)
                    await dataAccess.UpdateAsync(EntityContainers.ArtistGenres, "Genre = '" + genre.Genre + "'", "ArtistId", "'" + entity.Id + "'");
                foreach (BandMemberEntity member in entity.Members)
                {
                    await dataAccess.UpdateAsync(EntityContainers.BandMembers,
                    "Name = '" + member.Name +
                    "', Role = '" + member.Role +
                    "', Order = '" + member.Order +
                    "', Thumb = '" + member.Thumb + "'",
                    "ArtistId", "'" + entity.Id + "'");
                }
                foreach (AlbumEntity album in entity.Albums)
                {
                    await dataAccess.UpdateAsync(EntityContainers.Albums,
                    "Title = '" + album.Title +
                    "', NamedTitle = '" + album.NamedTitle +
                    "', Year = '" + album.Year +
                    "', Description = '" + album.Description +
                    "', Rating = '" + album.Rating +
                    "', IsListened = '" + album.IsListened +
                    "', IsFavorite = '" + album.IsFavorite + "'",
                    "ArtistId", "'" + entity.Id + "'");
                }
            });
            dataAccess.CloseTransaction();
            return artist;
        }

        /// <summary>
        /// Updates the IsListened status of an artist identified by <paramref name="artistId"/> in the storage medium
        /// </summary>
        /// <param name="artistId">The id of the artist whose status will be updated</param>
        /// <param name="isListened">The IsListened status to be set</param>
        /// <returns>The result of updating the IsListened status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateIsListenedStatusAsync(int artistId, bool? isListened)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Artists, "IsListened = '" + (isListened != null ? isListened.ToString() : "Null") + "'", "Id", "'" + artistId + "'");
        }

        /// <summary>
        /// Updates the IsFavorite status of an artist identified by <paramref name="artistId"/> in the storage medium
        /// </summary>
        /// <param name="artistId">The id of the artist whose status will be updated</param>
        /// <param name="isFavorite">The IsFavorite status to be set</param>
        /// <returns>The result of updating the IsFavorite status, wrapped in a generic API container of type <see cref="ApiResponse"/></returns>
        public async Task<ApiResponse> UpdateIsFavoriteStatusAsync(int artistId, bool isFavorite)
        {
            return await dataAccess.UpdateAsync(EntityContainers.Artists, "IsFavorite = '" + isFavorite + "'", "Id", "'" + artistId + "'");
        }
        #endregion
    }
}
