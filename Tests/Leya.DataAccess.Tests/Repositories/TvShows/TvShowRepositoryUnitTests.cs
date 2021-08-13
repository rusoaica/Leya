using Leya.DataAccess.Common.Configuration;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.Common.Models.Episodes;
using Leya.DataAccess.Common.Models.Seasons;
using Leya.DataAccess.Common.Models.TvShows;
using Leya.DataAccess.Repositories.TvShows;
using Leya.DataAccess.StorageAccess;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace Leya.DataAccess.Tests.Repositories.TvShows
{
    [TestFixture]
    public class TvShowRepositoryUnitTests
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public int firstId;
        public int secondId;
        #endregion

        [Test, Order(0)]
        public async Task CanDeleteAllTvShowsAsync_TvShowsAreDeleted_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            ITvShowRepository tvShowRepository = new TvShowRepository(sqlDataAccess);

            // ACT
            ApiResponse delete = await tvShowRepository.DeleteAllAsync();
            ApiResponse<EpisodeEntity> deleteEpisodes = await sqlDataAccess.SelectAsync<EpisodeEntity>(EntityContainers.Episodes);
            ApiResponse<TvShowRatingsEntity> deleteTvShowRatings = await sqlDataAccess.SelectAsync<TvShowRatingsEntity>(EntityContainers.TvShowRatings);
            ApiResponse<TvShowGenreEntity> deleteTvShowGenres = await sqlDataAccess.SelectAsync<TvShowGenreEntity>(EntityContainers.TvShowGenre);
            ApiResponse<TvShowActorsEntity> deleteTvShowActors = await sqlDataAccess.SelectAsync<TvShowActorsEntity>(EntityContainers.TvShowActors);
            ApiResponse<NamedSeasonEntity> deleteNamedSeasons = await sqlDataAccess.SelectAsync<NamedSeasonEntity>(EntityContainers.NamedSeasons);
            ApiResponse<TvShowResumeEntity> deleteTvShowResume = await sqlDataAccess.SelectAsync<TvShowResumeEntity>(EntityContainers.TvShowResume);
            ApiResponse<EpisodeActorsEntity> deleteEpisodeActors = await sqlDataAccess.SelectAsync<EpisodeActorsEntity>(EntityContainers.EpisodeActors);
            ApiResponse<EpisodeCreditEntity> deleteEpisodeCredits = await sqlDataAccess.SelectAsync<EpisodeCreditEntity>(EntityContainers.EpisodeCredits);
            ApiResponse<EpisodeGenreEntity> deleteEpisodeGenres = await sqlDataAccess.SelectAsync<EpisodeGenreEntity>(EntityContainers.EpisodeGenre);
            ApiResponse<EpisodeRatingEntity> deleteEpisodeRatings = await sqlDataAccess.SelectAsync<EpisodeRatingEntity>(EntityContainers.EpisodeRatings);

            ApiResponse<TvShowEntity> select = await tvShowRepository.GetAllAsync();

            // ASSERT
            Assert.Null(select.Data);
            Assert.Null(select.Error);
            Assert.Zero(select.Count);
            Assert.Null(delete.Error);
            Assert.Zero(deleteEpisodeRatings.Count);
            Assert.Null(deleteEpisodeRatings.Error);
            Assert.Null(deleteEpisodeRatings.Data);
            Assert.Zero(deleteEpisodeGenres.Count);
            Assert.Null(deleteEpisodeGenres.Error);
            Assert.Null(deleteEpisodeGenres.Data);
            Assert.Zero(deleteEpisodeCredits.Count);
            Assert.Null(deleteEpisodeCredits.Error);
            Assert.Null(deleteEpisodeCredits.Data);
            Assert.Zero(deleteEpisodeActors.Count);
            Assert.Null(deleteEpisodeActors.Error);
            Assert.Null(deleteEpisodeActors.Data);
            Assert.Zero(deleteTvShowResume.Count);
            Assert.Null(deleteTvShowResume.Error);
            Assert.Null(deleteTvShowResume.Data);
            Assert.Zero(deleteNamedSeasons.Count);
            Assert.Null(deleteNamedSeasons.Error);
            Assert.Null(deleteNamedSeasons.Data);
            Assert.Zero(deleteTvShowActors.Count);
            Assert.Null(deleteTvShowActors.Error);
            Assert.Null(deleteTvShowActors.Data);
            Assert.Zero(deleteTvShowGenres.Count);
            Assert.Null(deleteTvShowGenres.Error);
            Assert.Null(deleteTvShowGenres.Data);
            Assert.Zero(deleteTvShowRatings.Count);
            Assert.Null(deleteTvShowRatings.Error);
            Assert.Null(deleteTvShowRatings.Data);
            Assert.Zero(deleteEpisodes.Count);
            Assert.Null(deleteEpisodes.Error);
            Assert.Null(deleteEpisodes.Data);
        }

        [Test, Order(1)]
        public async Task CanInsertTvShowWithoutDetailsAsync_TvShowIsInserted_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            ITvShowRepository tvShowRepository = new TvShowRepository(sqlDataAccess);

            TvShowEntity firstExpected = new TvShowEntity()
            {
                MediaTypeSourceId = int.MaxValue,
                MediaTypeId = int.MaxValue - 1,
                TvShowTitle = "firstTvShowTitle",
                TvShowNamedTitle = "firstTvShowNamedTitle",
                NumberOfSeasons = int.MaxValue - 2,
                NumberOfEpisodes = int.MaxValue - 3,
                Synopsis = "firstSynopsis",
                TagLine = "firstTagLine",
                Runtime = int.MaxValue - 4,
                MPAA = "firstMPAA",
                LastPlayed = DateTime.Today,
                ImDbId = "firstImDbId",
                TvDbId = int.MaxValue - 5,
                TmDbId = int.MaxValue - 6,
                Aired = DateTime.Today.AddDays(1),
                IsEnded = true,
                Trailer = "firstTrailer",
                Studio = "firstStudio",
                IsWatched = true,
                IsFavorite = true,
            };

            // ACT
            ApiResponse<TvShowEntity> insert = await tvShowRepository.InsertAsync(firstExpected);

            // ASSERT
            Assert.Null(insert.Error);
            Assert.NotNull(insert.Data);
            Assert.NotZero(insert.Count);
            firstId = insert.Data[0].Id;
            ApiResponse<TvShowEntity> actual = await sqlDataAccess.SelectAsync<TvShowEntity>(EntityContainers.TvShows,
                    "Id, MediaTypeSourceId, MediaTypeId, TvShowNamedTitle, TvShowTitle, NumberOfSeasons, NumberOfEpisodes, Synopsis, TagLine, Runtime, MPAA, " +
                "LastPlayed, ImDbId, TvDbId, TmDbId, Aired, IsEnded, Trailer, Studio, IsWatched, IsFavorite, Created", new { Id = firstId });
            Assert.Null(actual.Error);
            Assert.NotNull(actual.Data);
            Assert.NotZero(actual.Count);
            firstExpected.Id = insert.Data[0].Id;
            firstExpected.Created = actual.Data[0].Created;
            firstExpected.Aired = actual.Data[0].Aired;
            firstExpected.LastPlayed = actual.Data[0].LastPlayed;
            Assert.AreEqual(JsonConvert.SerializeObject(firstExpected), JsonConvert.SerializeObject(actual.Data[0]));
        }

        [Test, Order(2)]
        public async Task CanInsertTvShowAsync_TvShowIsInserted_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            ITvShowRepository tvShowRepository = new TvShowRepository(sqlDataAccess);

            TvShowEntity firstExpected = new TvShowEntity()
            {
                MediaTypeSourceId = int.MaxValue - 1,
                MediaTypeId = int.MaxValue - 1,
                TvShowTitle = "secondTvShowTitle",
                TvShowNamedTitle = "firstTvShowNamedTitle",
                NumberOfSeasons = int.MaxValue - 2,
                NumberOfEpisodes = int.MaxValue - 3,
                Synopsis = "firstSynopsis",
                TagLine = "firstTagLine",
                Runtime = int.MaxValue - 4,
                MPAA = "firstMPAA",
                LastPlayed = DateTime.Today,
                ImDbId = "firstImDbId",
                TvDbId = int.MaxValue - 5,
                TmDbId = int.MaxValue - 6,
                Aired = DateTime.Today.AddDays(1),
                IsEnded = true,
                Trailer = "firstTrailer",
                Studio = "firstStudio",
                IsWatched = true,
                IsFavorite = true,
                Actors = new TvShowActorsEntity[]
                {
                    new TvShowActorsEntity() { Name = "firstActorName", Order = 1, Role = "firstActorRole", Thumb = "firstActorThumb" },
                    new TvShowActorsEntity() { Name = "secondActorName", Order = 2, Role = "secondActorRole", Thumb = "secondActorThumb" },
                },
                Genre = new TvShowGenreEntity[]
                {
                    new TvShowGenreEntity() { Genre = "firstGenre" },
                    new TvShowGenreEntity() { Genre = "secondGenre" },
                },
                Ratings = new TvShowRatingsEntity[]
                {
                    new TvShowRatingsEntity() { Max = int.MaxValue, Name = "firstRating", Value = int.MaxValue, Votes = int.MaxValue },
                    new TvShowRatingsEntity() { Max = int.MaxValue - 1, Name = "secondRating", Value = int.MaxValue - 1, Votes = int.MaxValue - 1 },
                },
                NamedSeasons = new NamedSeasonEntity[]
                {
                    new NamedSeasonEntity() { IsFavorite = true, IsWatched = true, Premiered = DateTime.Today,  SeasonName = "firstSeasonName", SeasonNumber = int.MaxValue, Synopsis = "firstSynopsis", Year = DateTime.Now.Year },
                    new NamedSeasonEntity() { IsFavorite = false, IsWatched = false, Premiered = DateTime.Today.AddDays(-1),  SeasonName = "secondSeasonName", SeasonNumber = int.MaxValue - 1, Synopsis = "secondSynopsis", Year = DateTime.Now.Year - 1 },
                },
            };
            TvShowEntity secondExpected = new TvShowEntity()
            {
                MediaTypeSourceId = int.MaxValue - 2,
                MediaTypeId = int.MaxValue - 1,
                TvShowTitle = "thirdTvShowTitle",
                TvShowNamedTitle = "secondTvShowNamedTitle",
                NumberOfSeasons = int.MaxValue - 2,
                NumberOfEpisodes = int.MaxValue - 3,
                Synopsis = "secondSynopsis",
                TagLine = "secondTagLine",
                Runtime = int.MaxValue - 4,
                MPAA = "secondMPAA",
                LastPlayed = DateTime.Today,
                ImDbId = "secondImDbId",
                TvDbId = int.MaxValue - 5,
                TmDbId = int.MaxValue - 6,
                Aired = DateTime.Today.AddDays(1),
                IsEnded = false,
                Trailer = "secondTrailer",
                Studio = "secondStudio",
                IsWatched = false,
                IsFavorite = false,
            };

            // ACT
            ApiResponse<TvShowEntity> insert = await tvShowRepository.InsertAsync(firstExpected);
            ApiResponse<TvShowEntity> tvShow = await tvShowRepository.InsertAsync(secondExpected);

            // ASSERT
            Assert.Null(insert.Error);
            Assert.NotNull(insert.Data);
            Assert.NotZero(insert.Count);
            firstId = insert.Data[0].Id;
            secondId = tvShow.Data[0].Id;
            ApiResponse<TvShowEntity> actual = await tvShowRepository.GetByIdAsync(int.MaxValue - 1);
            Assert.Null(actual.Error);
            Assert.NotNull(actual.Data);
            Assert.NotZero(actual.Count);
            firstExpected.Id = insert.Data[0].Id;
            firstExpected.Created = actual.Data[0].Created;
            firstExpected.Aired = actual.Data[0].Aired;
            firstExpected.LastPlayed = actual.Data[0].LastPlayed;
            firstExpected.Ratings[0].Id = actual.Data[0].Ratings[0].Id;
            firstExpected.Ratings[1].Id = actual.Data[0].Ratings[1].Id;
            firstExpected.Genre[0].Id = actual.Data[0].Genre[0].Id;
            firstExpected.Genre[1].Id = actual.Data[0].Genre[1].Id;
            firstExpected.Actors[0].Id = actual.Data[0].Actors[0].Id;
            firstExpected.Actors[1].Id = actual.Data[0].Actors[1].Id;
            firstExpected.NamedSeasons[0].Id = actual.Data[0].NamedSeasons[0].Id;
            firstExpected.NamedSeasons[1].Id = actual.Data[0].NamedSeasons[1].Id;
            firstExpected.NamedSeasons[0].Premiered = actual.Data[0].NamedSeasons[0].Premiered;
            firstExpected.NamedSeasons[1].Premiered = actual.Data[0].NamedSeasons[1].Premiered;
            Assert.AreEqual(JsonConvert.SerializeObject(firstExpected), JsonConvert.SerializeObject(actual.Data[0]));
        }

        [Test, Order(3)]
        public async Task CanGetAllTvShowsAsync_TvShowsAreTaken_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            ITvShowRepository tvShowRepository = new TvShowRepository(sqlDataAccess);
            TvShowEntity firstExpected = new TvShowEntity()
            {
                MediaTypeSourceId = int.MaxValue - 1,
                MediaTypeId = int.MaxValue - 1,
                TvShowTitle = "secondTvShowTitle",
                TvShowNamedTitle = "firstTvShowNamedTitle",
                NumberOfSeasons = int.MaxValue - 2,
                NumberOfEpisodes = int.MaxValue - 3,
                Synopsis = "firstSynopsis",
                TagLine = "firstTagLine",
                Runtime = int.MaxValue - 4,
                MPAA = "firstMPAA",
                LastPlayed = DateTime.Today,
                ImDbId = "firstImDbId",
                TvDbId = int.MaxValue - 5,
                TmDbId = int.MaxValue - 6,
                Aired = DateTime.Today.AddDays(1),
                IsEnded = true,
                Trailer = "firstTrailer",
                Studio = "firstStudio",
                IsWatched = true,
                IsFavorite = true,
                Actors = new TvShowActorsEntity[]
                {
                    new TvShowActorsEntity() { Name = "firstActorName", Order = 1, Role = "firstActorRole", Thumb = "firstActorThumb" },
                    new TvShowActorsEntity() { Name = "secondActorName", Order = 2, Role = "secondActorRole", Thumb = "secondActorThumb" },
                },
                Genre = new TvShowGenreEntity[]
                {
                    new TvShowGenreEntity() { Genre = "firstGenre" },
                    new TvShowGenreEntity() { Genre = "secondGenre" },
                },
                Ratings = new TvShowRatingsEntity[]
                {
                    new TvShowRatingsEntity() { Max = int.MaxValue, Name = "firstRating", Value = int.MaxValue, Votes = int.MaxValue },
                    new TvShowRatingsEntity() { Max = int.MaxValue - 1, Name = "secondRating", Value = int.MaxValue - 1, Votes = int.MaxValue - 1 },
                },
                NamedSeasons = new NamedSeasonEntity[]
                {
                    new NamedSeasonEntity() { IsFavorite = true, IsWatched = true, Premiered = DateTime.Today,  SeasonName = "firstSeasonName", SeasonNumber = int.MaxValue, Synopsis = "firstSynopsis", Year = DateTime.Now.Year },
                    new NamedSeasonEntity() { IsFavorite = false, IsWatched = false, Premiered = DateTime.Today.AddDays(-1),  SeasonName = "secondSeasonName", SeasonNumber = int.MaxValue - 1, Synopsis = "secondSynopsis", Year = DateTime.Now.Year - 1 },
                },
            };
            TvShowEntity secondExpected = new TvShowEntity()
            {
                MediaTypeSourceId = int.MaxValue - 2,
                MediaTypeId = int.MaxValue - 1,
                TvShowTitle = "thirdTvShowTitle",
                TvShowNamedTitle = "secondTvShowNamedTitle",
                NumberOfSeasons = int.MaxValue - 2,
                NumberOfEpisodes = int.MaxValue - 3,
                Synopsis = "secondSynopsis",
                TagLine = "secondTagLine",
                Runtime = int.MaxValue - 4,
                MPAA = "secondMPAA",
                LastPlayed = DateTime.Today,
                ImDbId = "secondImDbId",
                TvDbId = int.MaxValue - 5,
                TmDbId = int.MaxValue - 6,
                Aired = DateTime.Today.AddDays(1),
                IsEnded = false,
                Trailer = "secondTrailer",
                Studio = "secondStudio",
                IsWatched = false,
                IsFavorite = false,
            };

            // ACT
            ApiResponse<TvShowEntity> tvShows = await tvShowRepository.GetAllAsync();

            // ASSERT
            Assert.NotNull(tvShows.Data);
            Assert.Null(tvShows.Error);
            Assert.IsTrue(tvShows.Count > 1);
            Assert.NotZero(tvShows.Data.Where(u => u.TvShowTitle == firstExpected.TvShowTitle).Count());
            Assert.NotZero(tvShows.Data.Where(u => u.TvShowTitle == secondExpected.TvShowTitle).Count());
            TvShowEntity firstActual = tvShows.Data.Where(u => u.TvShowTitle == firstExpected.TvShowTitle).First();
            TvShowEntity secondActual = tvShows.Data.Where(u => u.TvShowTitle == secondExpected.TvShowTitle).First();
            Assert.AreEqual(firstActual.Id, firstId);
            Assert.AreEqual(secondActual.Id, secondId);

            firstExpected.Id = firstActual.Id;
            firstExpected.Created = firstActual.Created;
            firstExpected.Aired = firstActual.Aired;
            firstExpected.LastPlayed = firstActual.LastPlayed;
            firstExpected.Ratings[0].Id = firstActual.Ratings[0].Id;
            firstExpected.Ratings[0].TvShowId = firstActual.Id;
            firstExpected.Ratings[1].Id = firstActual.Ratings[1].Id;
            firstExpected.Ratings[1].TvShowId = firstActual.Id;
            firstExpected.Genre[0].Id = firstActual.Genre[0].Id;
            firstExpected.Genre[0].TvShowId = firstActual.Id;
            firstExpected.Genre[1].Id = firstActual.Genre[1].Id;
            firstExpected.Genre[1].TvShowId = firstActual.Id;
            firstExpected.Actors[0].Id = firstActual.Actors[0].Id;
            firstExpected.Actors[0].TvShowId = firstActual.Id;
            firstExpected.Actors[1].Id = firstActual.Actors[1].Id;
            firstExpected.Actors[1].TvShowId = firstActual.Id;
            firstExpected.NamedSeasons[0].Id = firstActual.NamedSeasons[0].Id;
            firstExpected.NamedSeasons[0].TvShowId = firstActual.Id;
            firstExpected.NamedSeasons[1].Id = firstActual.NamedSeasons[1].Id;
            firstExpected.NamedSeasons[1].TvShowId = firstActual.Id;
            firstExpected.NamedSeasons[0].Premiered = firstActual.NamedSeasons[0].Premiered;
            firstExpected.NamedSeasons[1].Premiered = firstActual.NamedSeasons[1].Premiered;

            secondExpected.Id = secondActual.Id;
            secondExpected.Created = secondActual.Created;
            secondExpected.Aired = secondActual.Aired;
            secondExpected.LastPlayed = secondActual.LastPlayed;

            Assert.AreEqual(JsonConvert.SerializeObject(firstExpected), JsonConvert.SerializeObject(firstActual));
            Assert.AreEqual(JsonConvert.SerializeObject(secondExpected), JsonConvert.SerializeObject(secondActual));
        }

        [Test, Order(4)]
        public async Task CanGetTvShowByIdAsync_TvShowIsTaken_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            ITvShowRepository tvShowRepository = new TvShowRepository(sqlDataAccess);
            TvShowEntity expected = new TvShowEntity()
            {
                MediaTypeSourceId = int.MaxValue - 1,
                MediaTypeId = int.MaxValue - 1,
                TvShowTitle = "secondTvShowTitle",
                TvShowNamedTitle = "firstTvShowNamedTitle",
                NumberOfSeasons = int.MaxValue - 2,
                NumberOfEpisodes = int.MaxValue - 3,
                Synopsis = "firstSynopsis",
                TagLine = "firstTagLine",
                Runtime = int.MaxValue - 4,
                MPAA = "firstMPAA",
                LastPlayed = DateTime.Today,
                ImDbId = "firstImDbId",
                TvDbId = int.MaxValue - 5,
                TmDbId = int.MaxValue - 6,
                Aired = DateTime.Today.AddDays(1),
                IsEnded = true,
                Trailer = "firstTrailer",
                Studio = "firstStudio",
                IsWatched = true,
                IsFavorite = true,
                Actors = new TvShowActorsEntity[]
                {
                    new TvShowActorsEntity() { Name = "firstActorName", Order = 1, Role = "firstActorRole", Thumb = "firstActorThumb" },
                    new TvShowActorsEntity() { Name = "secondActorName", Order = 2, Role = "secondActorRole", Thumb = "secondActorThumb" },
                },
                Genre = new TvShowGenreEntity[]
                {
                    new TvShowGenreEntity() { Genre = "firstGenre" },
                    new TvShowGenreEntity() { Genre = "secondGenre" },
                },
                Ratings = new TvShowRatingsEntity[]
                {
                    new TvShowRatingsEntity() { Max = int.MaxValue, Name = "firstRating", Value = int.MaxValue, Votes = int.MaxValue },
                    new TvShowRatingsEntity() { Max = int.MaxValue - 1, Name = "secondRating", Value = int.MaxValue - 1, Votes = int.MaxValue - 1 },
                },
                NamedSeasons = new NamedSeasonEntity[]
                {
                    new NamedSeasonEntity() { IsFavorite = true, IsWatched = true, Premiered = DateTime.Today,  SeasonName = "firstSeasonName", SeasonNumber = int.MaxValue, Synopsis = "firstSynopsis", Year = DateTime.Now.Year },
                    new NamedSeasonEntity() { IsFavorite = false, IsWatched = false, Premiered = DateTime.Today.AddDays(-1),  SeasonName = "secondSeasonName", SeasonNumber = int.MaxValue - 1, Synopsis = "secondSynopsis", Year = DateTime.Now.Year - 1 },
                },
            };

            // ACT
            ApiResponse<TvShowEntity> tvShows = await tvShowRepository.GetByIdAsync(int.MaxValue - 1);

            // ASSERT
            Assert.NotNull(tvShows.Data);
            Assert.Null(tvShows.Error);
            Assert.IsTrue(tvShows.Count == 1);
            Assert.NotZero(tvShows.Data.Where(u => u.TvShowTitle == expected.TvShowTitle).Count());
            TvShowEntity firstActual = tvShows.Data.Where(u => u.TvShowTitle == expected.TvShowTitle).First();
            Assert.AreEqual(firstActual.Id, firstId);

            expected.Id = firstActual.Id;
            expected.Created = firstActual.Created;
            expected.Aired = firstActual.Aired;
            expected.LastPlayed = firstActual.LastPlayed;
            expected.Ratings[0].Id = firstActual.Ratings[0].Id;
            expected.Ratings[0].TvShowId = firstActual.Id;
            expected.Ratings[1].Id = firstActual.Ratings[1].Id;
            expected.Ratings[1].TvShowId = firstActual.Id;
            expected.Genre[0].Id = firstActual.Genre[0].Id;
            expected.Genre[0].TvShowId = firstActual.Id;
            expected.Genre[1].Id = firstActual.Genre[1].Id;
            expected.Genre[1].TvShowId = firstActual.Id;
            expected.Actors[0].Id = firstActual.Actors[0].Id;
            expected.Actors[0].TvShowId = firstActual.Id;
            expected.Actors[1].Id = firstActual.Actors[1].Id;
            expected.Actors[1].TvShowId = firstActual.Id;
            expected.NamedSeasons[0].Id = firstActual.NamedSeasons[0].Id;
            expected.NamedSeasons[0].TvShowId = firstActual.Id;
            expected.NamedSeasons[1].Id = firstActual.NamedSeasons[1].Id;
            expected.NamedSeasons[1].TvShowId = firstActual.Id;
            expected.NamedSeasons[0].Premiered = firstActual.NamedSeasons[0].Premiered;
            expected.NamedSeasons[1].Premiered = firstActual.NamedSeasons[1].Premiered;

            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(firstActual));
        }

        [Test, Order(5)]
        public async Task CanUpdateTvShowAsync_TvShowIsUpdated_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            ITvShowRepository tvShowRepository = new TvShowRepository(sqlDataAccess);
            TvShowEntity expected = (await tvShowRepository.GetByIdAsync(int.MaxValue - 1))?.Data[0];

            expected.MediaTypeId = int.MaxValue - 1;
            expected.TvShowTitle = "secondTvShowTitle";
            expected.TvShowNamedTitle = "updatedTvShowNamedTitle";
            expected.NumberOfSeasons = int.MaxValue - 2;
            expected.NumberOfEpisodes = int.MaxValue - 3;
            expected.Synopsis = "updatedSynopsis";
            expected.TagLine = "updatedTagLine";
            expected.Runtime = int.MaxValue - 4;
            expected.MPAA = "updatedMPAA";
            expected.LastPlayed = DateTime.Today;
            expected.ImDbId = "updatedImDbId";
            expected.TvDbId = int.MaxValue - 5;
            expected.TmDbId = int.MaxValue - 6;
            expected.Aired = DateTime.Today.AddDays(1);
            expected.IsEnded = true;
            expected.Trailer = "updatedTrailer";
            expected.Studio = "updatedStudio";
            expected.IsWatched = true;
            expected.IsFavorite = true;
            expected.Actors[0].Name = "updatedActorName";
            expected.Actors[0].Role = "updatedActorRole";
            expected.Genre[0].Genre = "updatedGenre";
            expected.Ratings[0].Name = "updatedRating";
            expected.NamedSeasons[0].SeasonName = "updatedSeasonName";

            // ACT
            ApiResponse update = await tvShowRepository.UpdateAsync(expected);
            ApiResponse<TvShowEntity> actual = await tvShowRepository.GetByIdAsync(int.MaxValue - 1);

            // ASSERT
            Assert.NotNull(actual.Data);
            Assert.Null(actual.Error);
            Assert.IsTrue(actual.Count == 1);
            Assert.Null(update.Error);
            Assert.IsTrue(update.Count == 1);

            expected.Id = actual.Data[0].Id;
            expected.Created = actual.Data[0].Created;
            expected.Aired = actual.Data[0].Aired;
            expected.LastPlayed = actual.Data[0].LastPlayed;
            expected.Ratings[0].Id = actual.Data[0].Ratings[0].Id;
            expected.Ratings[0].TvShowId = actual.Data[0].Id;
            expected.Ratings[1].Id = actual.Data[0].Ratings[1].Id;
            expected.Ratings[1].TvShowId = actual.Data[0].Id;
            expected.Genre[0].Id = actual.Data[0].Genre[0].Id;
            expected.Genre[0].TvShowId = actual.Data[0].Id;
            expected.Genre[1].Id = actual.Data[0].Genre[1].Id;
            expected.Genre[1].TvShowId = actual.Data[0].Id;
            expected.Actors[0].Id = actual.Data[0].Actors[0].Id;
            expected.Actors[0].TvShowId = actual.Data[0].Id;
            expected.Actors[1].Id = actual.Data[0].Actors[1].Id;
            expected.Actors[1].TvShowId = actual.Data[0].Id;
            expected.NamedSeasons[0].Id = actual.Data[0].NamedSeasons[0].Id;
            expected.NamedSeasons[0].TvShowId = actual.Data[0].Id;
            expected.NamedSeasons[1].Id = actual.Data[0].NamedSeasons[1].Id;
            expected.NamedSeasons[1].TvShowId = actual.Data[0].Id;
            expected.NamedSeasons[0].Premiered = actual.Data[0].NamedSeasons[0].Premiered;
            expected.NamedSeasons[1].Premiered = actual.Data[0].NamedSeasons[1].Premiered;
            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual.Data[0]));
        }

        [Test, Order(6)]
        public async Task CanUpdateIsWatchedStatusAsync_StatusIsUpdated_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            ITvShowRepository tvShowRepository = new TvShowRepository(sqlDataAccess);
            int tvShow = (await tvShowRepository.GetByIdAsync(int.MaxValue - 1)).Data[0].Id;

            // ACT
            ApiResponse update = await tvShowRepository.UpdateIsWatchedStatusAsync(tvShow, false);

            // ASSERT
            Assert.Null(update.Error);
            Assert.IsTrue(update.Count == 1);
            bool actual = (await tvShowRepository.GetByIdAsync(int.MaxValue - 1)).Data[0].IsWatched;
            Assert.IsFalse(actual);
        }

        [Test, Order(7)]
        public async Task CanUpdateIsFavoriteStatusAsync_StatusIsUpdated_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            ITvShowRepository tvShowRepository = new TvShowRepository(sqlDataAccess);
            int tvShow = (await tvShowRepository.GetByIdAsync(int.MaxValue - 1)).Data[0].Id;

            // ACT
            ApiResponse update = await tvShowRepository.UpdateIsFavoriteStatusAsync(tvShow, false);

            // ASSERT
            Assert.Null(update.Error);
            Assert.IsTrue(update.Count == 1);
            bool actual = (await tvShowRepository.GetByIdAsync(int.MaxValue - 1)).Data[0].IsFavorite;
            Assert.IsFalse(actual);
        }

        [Test, Order(8)]
        public async Task CanDeleteTvShowById_TvShowIsDeleted_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            ITvShowRepository tvShowRepository = new TvShowRepository(sqlDataAccess);

            // ACT
            ApiResponse delete = await tvShowRepository.DeleteByIdAsync(firstId);
            ApiResponse<EpisodeEntity> deleteEpisodes = await sqlDataAccess.SelectAsync<EpisodeEntity>(EntityContainers.Episodes);
            ApiResponse<TvShowRatingsEntity> deleteTvShowRatings = await sqlDataAccess.SelectAsync<TvShowRatingsEntity>(EntityContainers.TvShowRatings);
            ApiResponse<TvShowGenreEntity> deleteTvShowGenres = await sqlDataAccess.SelectAsync<TvShowGenreEntity>(EntityContainers.TvShowGenre);
            ApiResponse<TvShowActorsEntity> deleteTvShowActors = await sqlDataAccess.SelectAsync<TvShowActorsEntity>(EntityContainers.TvShowActors);
            ApiResponse<NamedSeasonEntity> deleteNamedSeasons = await sqlDataAccess.SelectAsync<NamedSeasonEntity>(EntityContainers.NamedSeasons);
            ApiResponse<TvShowResumeEntity> deleteTvShowResume = await sqlDataAccess.SelectAsync<TvShowResumeEntity>(EntityContainers.TvShowResume);
            ApiResponse<EpisodeActorsEntity> deleteEpisodeActors = await sqlDataAccess.SelectAsync<EpisodeActorsEntity>(EntityContainers.EpisodeActors);
            ApiResponse<EpisodeCreditEntity> deleteEpisodeCredits = await sqlDataAccess.SelectAsync<EpisodeCreditEntity>(EntityContainers.EpisodeCredits);
            ApiResponse<EpisodeGenreEntity> deleteEpisodeGenres = await sqlDataAccess.SelectAsync<EpisodeGenreEntity>(EntityContainers.EpisodeGenre);
            ApiResponse<EpisodeRatingEntity> deleteEpisodeRatings = await sqlDataAccess.SelectAsync<EpisodeRatingEntity>(EntityContainers.EpisodeRatings);

            ApiResponse<TvShowEntity> select = await tvShowRepository.GetByIdAsync(int.MaxValue - 1);

            // ASSERT
            Assert.Null(select.Data);
            Assert.Null(select.Error);
            Assert.Zero(select.Count);
            Assert.Null(delete.Error);
            Assert.Zero(deleteEpisodeRatings.Count);
            Assert.Null(deleteEpisodeRatings.Error);
            Assert.Null(deleteEpisodeRatings.Data);
            Assert.Zero(deleteEpisodeGenres.Count);
            Assert.Null(deleteEpisodeGenres.Error);
            Assert.Null(deleteEpisodeGenres.Data);
            Assert.Zero(deleteEpisodeCredits.Count);
            Assert.Null(deleteEpisodeCredits.Error);
            Assert.Null(deleteEpisodeCredits.Data);
            Assert.Zero(deleteEpisodeActors.Count);
            Assert.Null(deleteEpisodeActors.Error);
            Assert.Null(deleteEpisodeActors.Data);
            Assert.Zero(deleteTvShowResume.Count);
            Assert.Null(deleteTvShowResume.Error);
            Assert.Null(deleteTvShowResume.Data);
            Assert.Zero(deleteNamedSeasons.Count);
            Assert.Null(deleteNamedSeasons.Error);
            Assert.Null(deleteNamedSeasons.Data);
            Assert.Zero(deleteTvShowActors.Count);
            Assert.Null(deleteTvShowActors.Error);
            Assert.Null(deleteTvShowActors.Data);
            Assert.Zero(deleteTvShowGenres.Count);
            Assert.Null(deleteTvShowGenres.Error);
            Assert.Null(deleteTvShowGenres.Data);
            Assert.Zero(deleteTvShowRatings.Count);
            Assert.Null(deleteTvShowRatings.Error);
            Assert.Null(deleteTvShowRatings.Data);
            Assert.Zero(deleteEpisodes.Count);
            Assert.Null(deleteEpisodes.Error);
            Assert.Null(deleteEpisodes.Data);
        }
    }
}
