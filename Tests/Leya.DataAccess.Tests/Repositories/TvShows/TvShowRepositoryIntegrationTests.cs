/// Written by: Yulia Danilova
/// Creation Date: 16th of June, 2021
/// Purpose: Integration Test for the TvShowRepository DAL class
#region ========================================================================= USING =====================================================================================
using NUnit.Framework;
using Leya.DataAccess.Repositories.TvShows;
#endregion

namespace Leya.DataAccess.Tests.Repositories.TvShows
{
    [TestFixture]
    public class TvShowRepositoryIntegrationTests
    {
        #region ================================================================= METHODS ===================================================================================
        [Test]
        public void UserRepositoryImplementsIUserRepositoryInterface_InterfaceIsImplemented_ReturnsTrue()
        {
            // ARRANGE
            TvShowRepository userRepository = new TvShowRepository(null);
            
            // ACT

            // ASSERT
            Assert.True(userRepository is ITvShowRepository);
        }
        #endregion
    }
}
