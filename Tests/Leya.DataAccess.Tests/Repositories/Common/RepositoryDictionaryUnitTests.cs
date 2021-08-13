/// Written by: Yulia Danilova
/// Creation Date: 14th of June, 2021
/// Purpose: Unit Test for the RepositoryDictionary DAL class
#region ========================================================================= USING =====================================================================================
using NUnit.Framework;
using Leya.DataAccess.Repositories.Common;
using Leya.DataAccess.Repositories.Episodes;
#endregion

namespace Leya.DataAccess.Tests.Repositories.Common
{
    [TestFixture]
    public class RepositoryDictionaryUnitTests
    {
        #region ================================================================= METHODS ===================================================================================
        [Test]
        public void CanAddRepositoryItem_RepositoryItemIsAdded_ReturnsTrue()
        {
            // ARRANGE
            EpisodeRepository episodeRepository = new EpisodeRepository(null);
            RepositoryDictionary repositoryDictionary = new RepositoryDictionary();
            Assert.Zero(repositoryDictionary.Count);

            // ACT
            repositoryDictionary.Add<IEpisodeRepository>(episodeRepository);

            // ASSERT
            Assert.NotZero(repositoryDictionary.Count);
        }

        [Test]
        public void CanClearRepositoryList_RepositoryListIsCleared_ReturnsTrue()
        {
            // ARRANGE
            EpisodeRepository episodeRepository = new EpisodeRepository(null);
            RepositoryDictionary repositoryDictionary = new RepositoryDictionary();
            repositoryDictionary.Add<IEpisodeRepository>(episodeRepository);

            // ACT
            repositoryDictionary.Clear();

            // ASSERT
            Assert.Zero(repositoryDictionary.Count);
        }

        [Test]
        public void CanGetRepository_RepositoryIsTaken_ReturnsTrue()
        {
            // ARRANGE
            IEpisodeRepository expected = new EpisodeRepository(null);
            RepositoryDictionary repositoryDictionary = new RepositoryDictionary();
            repositoryDictionary.Add<IEpisodeRepository>(expected);

            // ACT
            IEpisodeRepository actual = repositoryDictionary.Get<IEpisodeRepository>(typeof(EpisodeRepository));

            // ASSERT
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
