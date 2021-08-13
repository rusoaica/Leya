/// Written by: Yulia Danilova
/// Creation Date: 14th of June, 2021
/// Purpose: Unit Test for the UnitOfWork DAL class
#region ========================================================================= USING =====================================================================================
using System;
using NUnit.Framework;
using Leya.DataAccess.Common.IoC;
using Leya.DataAccess.Repositories.Songs;
using Leya.DataAccess.Repositories.Media;
using Leya.DataAccess.Repositories.Users;
using Leya.DataAccess.Repositories.Albums;
using Leya.DataAccess.Repositories.Movies;
using Leya.DataAccess.Repositories.Seasons;
using Leya.DataAccess.Repositories.TvShows;
using Leya.DataAccess.Repositories.Episodes;
#endregion

namespace Leya.DataAccess.Tests
{
    [TestFixture]
    public class UnitOfWorkUnitTests
    {
        #region ================================================================= METHODS ===================================================================================
        [Test]
        public void CanResetRepositories_RepositoriesAreReset_ReturnsTrue()
        {
            // ARRANGE
            UnitOfWork unitOfWork = new UnitOfWork();
            // ACT
            Assert.Greater(unitOfWork.Repositories.Count, 0);
            unitOfWork.ResetRepositories();
            // ASSERT
            Assert.AreEqual(unitOfWork.Repositories.Count, 0);
        }

        [Test]
        public void CanAddRepositories_RepositoriesAreAdded_ReturnsTrue()
        {
            // ARRANGE
            UnitOfWork unitOfWork = new UnitOfWork();

            // ACT

            // ASSERT
            // repositories should already be added ever since unit of work is instantiated
            Assert.Greater(unitOfWork.Repositories.Count, 0);
            // trying to add duplicate repositories should throw an exception
            Assert.Throws<ArgumentException>(new TestDelegate(() => unitOfWork.AddRepositories(DIContainerConfig.Configure())));
            int nrOfDefaultAddedRepositories = unitOfWork.Repositories.Count;
            // reset repositories
            unitOfWork.ResetRepositories();
            // there should be no more repositories registered
            Assert.AreEqual(unitOfWork.Repositories.Count, 0);
            // add repositories
            unitOfWork.AddRepositories(DIContainerConfig.Configure());
            // repositories should be added
            Assert.Greater(unitOfWork.Repositories.Count, 0);
            // number of added repositories should be the same as the number of repositories added at instantiation
            Assert.AreEqual(unitOfWork.Repositories.Count, nrOfDefaultAddedRepositories);
        }

        [Test]
        public void CanGetRepository_RepositoryIsTaken_ReturnsFalse()
        {
            // ARRANGE
            UnitOfWork unitOfWork = new UnitOfWork();
            unitOfWork.ResetRepositories();

            // ACT
          
            // ASSERT
            Assert.Null(unitOfWork.GetRepository<IEpisodeRepository>());
        }

        [Test]
        public void CanGetRepository_RepositoryIsTaken_ReturnsTrue()
        {
            // ARRANGE
            UnitOfWork unitOfWork = new UnitOfWork();

            // ACT
           
            // ASSERT
            // all required repositories should be registered
            Assert.NotNull(unitOfWork.GetRepository<IAlbumRepository>());
            Assert.NotNull(unitOfWork.GetRepository<IAlbumRepository>());
            Assert.NotNull(unitOfWork.GetRepository<IEpisodeRepository>());
            Assert.NotNull(unitOfWork.GetRepository<IMediaTypeRepository>());
            Assert.NotNull(unitOfWork.GetRepository<IMediaTypeSourceRepository>());
            Assert.NotNull(unitOfWork.GetRepository<IMovieRepository>());
            Assert.NotNull(unitOfWork.GetRepository<INamedSeasonRepository>());
            Assert.NotNull(unitOfWork.GetRepository<ISongRepository>());
            Assert.NotNull(unitOfWork.GetRepository<ITvShowRepository>());
            Assert.NotNull(unitOfWork.GetRepository<IUserRepository>());
        }
        #endregion
    }
}