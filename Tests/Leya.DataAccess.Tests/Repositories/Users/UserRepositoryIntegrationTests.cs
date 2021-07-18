/// Written by: Yulia Danilova
/// Creation Date: 15th of June, 2021
/// Purpose: Integration Test for the UserRepository DAL class
#region ========================================================================= USING =====================================================================================
using NUnit.Framework;
using Leya.DataAccess.Repositories.Users;
#endregion

namespace Leya.DataAccess.Tests.Repositories.Users
{
    [TestFixture]
    public class UserRepositoryIntegrationTests
    {
        #region ================================================================= METHODS ===================================================================================
        [Test]
        public void UserRepositoryImplementsIUserRepositoryInterface_InterfaceIsImplemented_ReturnsTrue()
        {
            // ARRANGE
            UserRepository userRepository = new UserRepository(null);
            
            // ACT

            // ASSERT
            Assert.True(userRepository is IUserRepository);
        }
        #endregion
    }
}
