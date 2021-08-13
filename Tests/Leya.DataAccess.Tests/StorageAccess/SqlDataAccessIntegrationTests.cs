/// Written by: Yulia Danilova
/// Creation Date: 14th of June, 2021
/// Purpose: Integration Test for the UnitOfWork DAL class
#region ========================================================================= USING =====================================================================================
using NUnit.Framework;
using Leya.DataAccess.StorageAccess;
#endregion

namespace Leya.DataAccess.Tests.StorageAccess
{
    [TestFixture]
    public class SqlDataAccessIntegrationTests
    {
        #region ================================================================= METHODS ===================================================================================
        [Test]
        public void SqlDataAccessImplementsDataAccessInterface_InterfaceIsImplemented_ReturnsTrue()
        {
            // ARRANGE
            SqlDataAccess sqlDataAccess = new SqlDataAccess(null, null);
            // ACT

            // ASSERT
            Assert.True(sqlDataAccess is IDataAccess);
        }
        #endregion
    }
}
