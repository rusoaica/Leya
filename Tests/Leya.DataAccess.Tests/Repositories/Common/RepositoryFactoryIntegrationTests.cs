/// Written by: Yulia Danilova
/// Creation Date: 14th of June, 2021
/// Purpose: Integration Test for the RepositoryFactory DAL class
#region ========================================================================= USING =====================================================================================
using NUnit.Framework;
using Leya.DataAccess.Repositories.Common;
#endregion

namespace Leya.DataAccess.Tests.Repositories.Common
{
    [TestFixture]
    public class RepositoryFactoryIntegrationTests
    {
        #region ================================================================= METHODS ===================================================================================
        [Test]
        public void RepositoryFactoryImplementsRepositoryFactoryInterface_InterfaceIsImplemented_ReturnsTrue()
        {
            // ARRANGE
            RepositoryFactory repositoryFactory = new RepositoryFactory(null);
            // ACT

            // ASSERT
            Assert.True(repositoryFactory is IRepositoryFactory);
        }
        #endregion
    }
}
