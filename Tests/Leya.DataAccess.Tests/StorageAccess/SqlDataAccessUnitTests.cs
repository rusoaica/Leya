/// Written by: Yulia Danilova
/// Creation Date: 15th of June, 2021
/// Purpose: Unit Test for the SqlDataAccess DAL class
#region ========================================================================= USING =====================================================================================
using System;
using System.Data;
using System.Linq;
using NUnit.Framework;
using Newtonsoft.Json;
using System.Reflection;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Collections.Generic;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Common.Attributes;
using Leya.DataAccess.Common.Configuration;
using Leya.DataAccess.Common.Models.Episodes;
#endregion

namespace Leya.DataAccess.Tests.StorageAccess
{
    [TestFixture]
    public class SqlDataAccessUnitTests
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private int id;
        #endregion

        #region ================================================================= METHODS ===================================================================================
        [Test, Order(0)]
        public void CanGetConnectionString_ConnectionStringIsTakenReturnsTrue()
        {
            // ARRANGE
            string expected = "Data Source=database.db;Version=3;";
            SqlDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", expected } } });

            // ACT
            string actual = sqlDataAccess.GetConnectionString("SqLite");

            // ASSERT
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(1)]
        public void CanGetDynamicObjectProperties_PropertiesAreTaken_ReturnsTrue()
        {
            // ARRANGE
            SqlDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            EpisodeEntity model = new EpisodeEntity()
            {
                Id = id,
                TvShowId = int.MaxValue,
                NamedSeasonId = int.MaxValue,
                Title = "test title",
                NamedTitle = "test named title",
                Episode = 1,
                Synopsis = "test synopsis",
                Runtime = 2,
                MPAA = "test mpaa",
                LastPlayed = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                ImDbId = "imdb id",
                TvDbId = 3,
                TmDbId = 4,
                Director = "test director",
                Year = "2021",
                Aired = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1),
                IsWatched = false,
                IsFavorite = true
            };

            string expected = string.Join(",\n\t", model.GetType()
                                                        .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                                                        .Where(e => e.GetCustomAttributes<IgnoreOnInsertAttribute>()
                                                        .Count() == 0).Select(e => "`" + e.Name + "`"));

            // ACT 
            string actual = sqlDataAccess.GetColumns(model);

            // ASSERT
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(2)]
        public void CanGetDynamicObjectPropertiesValues_PropertiesValuesAreTaken_ReturnsTrue()
        {
            // ARRANGE
            SqlDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            EpisodeEntity model = new EpisodeEntity()
            {
                Id = id,
                TvShowId = int.MaxValue,
                NamedSeasonId = int.MaxValue,
                Title = "test title",
                NamedTitle = "test named title",
                Episode = 1,
                Synopsis = "test synopsis",
                Runtime = 2,
                MPAA = "test mpaa",
                LastPlayed = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                ImDbId = "imdb id",
                TvDbId = 3,
                TmDbId = 4,
                Director = "test director",
                Year = "2021",
                Aired = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1),
                IsWatched = false,
                IsFavorite = true
            };

            string expected = string.Join(",\n\t", model.GetType()
                                                        .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                                                        .Where(e => e.GetCustomAttributes<IgnoreOnInsertAttribute>()
                                                        .Count() == 0).Select(e => "@" + e.Name));

            // ACT 
            string actual = sqlDataAccess.GetParameters(model);

            // ASSERT
            Assert.AreEqual(expected, actual);
        }

        [Test, Order(3)]
        public async Task CanInsertAsync_InsertIsPerformed_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            EpisodeEntity expected = new EpisodeEntity()
            {
                TvShowId = int.MaxValue,
                NamedSeasonId = int.MaxValue,
                Title = "test title",
                NamedTitle = "test named title",
                Episode = 1,
                Synopsis = "test synopsis",
                Runtime = 2,
                MPAA = "test mpaa",
                LastPlayed = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                ImDbId = "imdb id",
                TvDbId = 3,
                TmDbId = 4,
                Director = "test director",
                Year = "2021",
                Aired = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1),
                IsWatched = true,
                IsFavorite = true
            };

            // ACT
            ApiResponse<EpisodeEntity> result = await sqlDataAccess.InsertAsync(EntityContainers.Episodes, expected);

            // ASSERT
            Assert.NotNull(result.Data);
            Assert.Null(result.Error);
            Assert.AreEqual(result.Count, 1);
            Assert.NotZero(result.Data[0].Id);
            id = result.Data[0].Id;
        }

        [Test, Order(4)]
        public async Task CanSelectAsync_SelectIsPerformed_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            EpisodeEntity expected = new EpisodeEntity()
            {
                Id = id,
                TvShowId = int.MaxValue,
                NamedSeasonId = int.MaxValue,
                Title = "test title",
                NamedTitle = "test named title",
                Episode = 1,
                Synopsis = "test synopsis",
                Runtime = 2,
                MPAA = "test mpaa",
                LastPlayed = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                ImDbId = "imdb id",
                TvDbId = 3,
                TmDbId = 4,
                Director = "test director",
                Year = "2021",
                Aired = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1),
                IsWatched = true,
                IsFavorite = true
            };

            // ACT
            ApiResponse<EpisodeEntity> actual = await sqlDataAccess.SelectAsync<EpisodeEntity>(EntityContainers.Episodes,
               "Id, TvShowId, Title, NamedSeasonId, NamedTitle, Episode, Synopsis, Runtime, MPAA, LastPlayed, ImDbId, TvDbId, TmDbId, Director, Year, Aired, IsWatched, IsFavorite", new { Id = id });

            // ASSERT
            Assert.NotNull(actual.Data);
            Assert.Null(actual.Error);
            Assert.AreEqual(actual.Count, 1);
            Assert.NotZero(actual.Data[0].Id);
            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual.Data[0]));
        }

        [Test, Order(5)]
        public async Task CanUpdateAsync_UpdateIsPerformed_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            EpisodeEntity expected = new EpisodeEntity()
            {
                Id = id,
                TvShowId = int.MaxValue,
                NamedSeasonId = int.MaxValue,
                Title = "test title",
                NamedTitle = "test named title",
                Episode = 1,
                Synopsis = "test synopsis",
                Runtime = 2,
                MPAA = "test mpaa",
                LastPlayed = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                ImDbId = "imdb id",
                TvDbId = 3,
                TmDbId = 4,
                Director = "test director",
                Year = "2021",
                Aired = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1),
                IsWatched = false,
                IsFavorite = true
            };

            // ACT
            ApiResponse update = await sqlDataAccess.UpdateAsync(EntityContainers.Episodes, "IsWatched = 'false', IsFavorite = 'true'", "Id", "'" + id + "'");
            ApiResponse<EpisodeEntity> actual = await sqlDataAccess.SelectAsync<EpisodeEntity>(EntityContainers.Episodes,
            "Id, TvShowId, Title, NamedSeasonId, NamedTitle, Episode, Synopsis, Runtime, MPAA, LastPlayed, ImDbId, TvDbId, TmDbId, Director, Year, Aired, IsWatched, IsFavorite", new { Id = id });

            // ASSERT
            Assert.Null(update.Error);
            Assert.Null(actual.Error);
            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual.Data[0]));
        }

        [Test, Order(6)]
        public async Task CanDeleteAsync_DeleteIsPerformed_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });

            // ACT
            ApiResponse delete = await sqlDataAccess.DeleteAsync(EntityContainers.Episodes, new { Id = id });
            ApiResponse<EpisodeEntity> actual = await sqlDataAccess.SelectAsync<EpisodeEntity>(EntityContainers.Episodes,
                    "Id, TvShowId, Title, NamedSeasonId, NamedTitle, Episode, Synopsis, Runtime, MPAA, LastPlayed, ImDbId, TvDbId, TmDbId, Director, Year, Aired, IsWatched, IsFavorite", new { Id = id });

            // ASSERT
            Assert.Null(delete.Error);
            Assert.Null(actual.Error);
            Assert.Null(actual.Data);
            Assert.Zero(actual.Count);
        }

        [Test]
        public void CanBeginTransaction_TransactionIsCreated_ReturnsTrue()
        {
            // ARRANGE
            SQLiteConnection sqLiteConnection = new SQLiteConnection();
            SqlDataAccess sqlDataAccess = new SqlDataAccess(sqLiteConnection, new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });

            // ACT
            sqlDataAccess.OpenTransaction();

            // ASSERT
            Assert.NotNull(sqlDataAccess.ConnectionStringName);
            Assert.IsTrue(sqLiteConnection.State == ConnectionState.Open);
            sqlDataAccess.CloseTransaction();
        }

        [Test]
        public async Task CanExecuteTransaction_TransactionIsExecuted_ReturnsFalse()
        {
            // ARRANGE
            SqlDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });

            // ACT
            sqlDataAccess.OpenTransaction();

            EpisodeEntity expected = new EpisodeEntity()
            {
                TvShowId = int.MaxValue,
                NamedSeasonId = int.MaxValue,
                Title = "test title",
                NamedTitle = "test named title",
                Episode = 1,
                Synopsis = "test synopsis",
                Runtime = 2,
                MPAA = "test mpaa",
                LastPlayed = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                ImDbId = "imdb id",
                TvDbId = 3,
                TmDbId = 4,
                Director = "test director",
                Year = "2021",
                Aired = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1),
                IsWatched = true,
                IsFavorite = true
            };
            ApiResponse<EpisodeEntity> result = await sqlDataAccess.InsertAsync(EntityContainers.Episodes, expected);
            ApiResponse update = await sqlDataAccess.UpdateAsync(EntityContainers.Episodes, "IsWatched = '''", "Id", "'" + result.Data[0].Id + "'");
            ApiResponse delete = await sqlDataAccess.DeleteAsync(EntityContainers.Episodes, new { Id = result.Data[0].Id });

            sqlDataAccess.CloseTransaction();

            // ASSERT
            ApiResponse<EpisodeEntity> actual = await sqlDataAccess.SelectAsync<EpisodeEntity>(EntityContainers.Episodes,
                    "Id, TvShowId, Title, NamedSeasonId, NamedTitle, Episode, Synopsis, Runtime, MPAA, LastPlayed, ImDbId, TvDbId, TmDbId, Director, Year, Aired, IsWatched, IsFavorite", new { Id = result.Data[0].Id });
            Assert.Null(actual.Data);
            Assert.NotNull(update.Error);
            Assert.NotNull(result.Data);
            Assert.NotZero(delete.Count);
        }

        [Test]
        public async Task CanExecuteTransaction_TransactionIsExecuted_ReturnsTrue()
        {
            // ARRANGE
            SqlDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });

            // ACT
            sqlDataAccess.OpenTransaction();

            EpisodeEntity expected = new EpisodeEntity()
            {
                TvShowId = int.MaxValue,
                NamedSeasonId = int.MaxValue,
                Title = "test title",
                NamedTitle = "test named title",
                Episode = 1,
                Synopsis = "test synopsis",
                Runtime = 2,
                MPAA = "test mpaa",
                LastPlayed = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                ImDbId = "imdb id",
                TvDbId = 3,
                TmDbId = 4,
                Director = "test director",
                Year = "2021",
                Aired = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1),
                IsWatched = true,
                IsFavorite = true
            };
            ApiResponse<EpisodeEntity> result = await sqlDataAccess.InsertAsync(EntityContainers.Episodes, expected);
            ApiResponse update = await sqlDataAccess.UpdateAsync(EntityContainers.Episodes, "IsWatched = 'true'", "Id", "'" + result.Data[0].Id + "'");

            sqlDataAccess.CloseTransaction();

            // ASSERT
            ApiResponse<EpisodeEntity> actual = await sqlDataAccess.SelectAsync<EpisodeEntity>(EntityContainers.Episodes,
                    "Id, TvShowId, Title, NamedSeasonId, NamedTitle, Episode, Synopsis, Runtime, MPAA, LastPlayed, ImDbId, TvDbId, TmDbId, Director, Year, Aired, IsWatched, IsFavorite", new { Id = result.Data[0].Id });
            ApiResponse delete = await sqlDataAccess.DeleteAsync(EntityContainers.Episodes, new { Id = result.Data[0].Id });
            Assert.NotNull(actual.Data);
            Assert.NotZero(actual.Count);
            expected.Id = actual.Data[0].Id;
            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual.Data[0]));
            Assert.Null(update.Error);
            Assert.NotNull(result.Data);
            Assert.NotZero(delete.Count);
        }
        #endregion
    }
}
