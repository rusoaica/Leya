/// Written by: Yulia Danilova
/// Creation Date: 14th of June, 2021
/// Purpose: Integration Test for the UnitOfWork DAL class
#region ========================================================================= USING =====================================================================================
using NUnit.Framework;
#endregion

namespace Leya.DataAccess.Tests
{
    [TestFixture]
    public class UnitOfWorkIntegrationTests
    {
        #region ================================================================= METHODS ===================================================================================
        [Test]
        public void UnitOfWorkImplementsUnitOfWorkInterface_InterfaceIsImplemented_ReturnsTrue()
        {
            // ARRANGE
            UnitOfWork unitOfWork = new UnitOfWork();
            // ACT

            // ASSERT
            Assert.True(unitOfWork is IUnitOfWork);
        }
        #endregion
    }
}
