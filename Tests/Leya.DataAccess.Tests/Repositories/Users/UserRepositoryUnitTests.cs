/// Written by: Yulia Danilova
/// Creation Date: 16th of June, 2021
/// Purpose: Unit Test for the UserRepository DAL class
#region ========================================================================= USING =====================================================================================
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Collections.Generic;
using Leya.DataAccess.Common.Enums;
using Leya.DataAccess.Common.Models;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Repositories.Users;
using Leya.DataAccess.Common.Models.Users;
using Leya.DataAccess.Common.Configuration;
#endregion

namespace Leya.DataAccess.Tests.Repositories.Users
{
    [TestFixture]
    public class UserRepositoryUnitTests
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        public int firstId;
        public int secondId;
        #endregion

        #region ================================================================= METHODS ===================================================================================
        [Test, Order(0)]
        public async Task CanDeleteAllUsersAsync_UsersAreDeleted_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            IUserRepository userRepository = new UserRepository(sqlDataAccess);

            // ACT
            ApiResponse delete = await userRepository.DeleteAllAsync();
            ApiResponse<UserEntity> select = await userRepository.GetAllAsync();

            // ASSERT
            Assert.Null(select.Data);
            Assert.Null(select.Error);
            Assert.Zero(select.Count);
            Assert.Null(delete.Error);
            Assert.NotZero(delete.Count);
        }

        [Test, Order(1)]
        public async Task CanInsertUserAsync_UserIsInserted_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            IUserRepository userRepository = new UserRepository(sqlDataAccess);

            UserEntity firstExpected = new UserEntity()
            {
                Username = "firstUsername",
                Password = "firstPassword",
                SecurityQuestion = "firstSecurityQuestion",
                SecurityAnswer = "firstSecurityAnswer"
            };
            UserEntity secondExpected = new UserEntity()
            {
                Username = "secondUsername",
                Password = "secondPassword",
                SecurityQuestion = "secondSecurityQuestion",
                SecurityAnswer = "secondSecurityAnswer"
            };

            // ACT
            ApiResponse<UserEntity> insert = await userRepository.InsertAsync(firstExpected);
            ApiResponse<UserEntity> user = await userRepository.InsertAsync(secondExpected);

            // ASSERT
            Assert.Null(insert.Error);
            Assert.NotNull(insert.Data);
            Assert.NotZero(insert.Count);
            firstId = insert.Data[0].Id;
            secondId = user.Data[0].Id;
            ApiResponse<UserEntity> actual = await sqlDataAccess.SelectAsync<UserEntity>(EntityContainers.Users,
                    "Id, Username, Password, SecurityQuestion, SecurityAnswer, Created", new { Id = firstId });
            await sqlDataAccess.SelectAsync<UserEntity>(EntityContainers.Users, "Id, Username, Password, SecurityQuestion, SecurityAnswer, Created", new { Id = secondId });
            Assert.Null(actual.Error);
            Assert.NotNull(actual.Data);
            Assert.NotZero(actual.Count);
            firstExpected.Id = insert.Data[0].Id;
            firstExpected.Created = actual.Data[0].Created;
            Assert.AreEqual(JsonConvert.SerializeObject(firstExpected), JsonConvert.SerializeObject(actual.Data[0]));
        }

        [Test, Order(2)]
        public async Task CanGetAllUsersAsync_UsersAreTaken_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            IUserRepository userRepository = new UserRepository(sqlDataAccess);
            UserEntity firstExpected = new UserEntity()
            {
                Username = "firstUsername",
                Password = "firstPassword",
                SecurityQuestion = "firstSecurityQuestion",
                SecurityAnswer = "firstSecurityAnswer"
            };
            UserEntity secondExpected = new UserEntity()
            {
                Username = "secondUsername",
                Password = "secondPassword",
                SecurityQuestion = "secondSecurityQuestion",
                SecurityAnswer = "secondSecurityAnswer"
            };

            // ACT
            ApiResponse<UserEntity> select = await userRepository.GetAllAsync();

            // ASSERT
            Assert.NotNull(select.Data);
            Assert.Null(select.Error);
            Assert.IsTrue(select.Count > 1);
            Assert.NotZero(select.Data.Where(u => u.Username == firstExpected.Username).Count());
            Assert.NotZero(select.Data.Where(u => u.Username == secondExpected.Username).Count());
            UserEntity firstActual = select.Data.Where(u => u.Username == firstExpected.Username).First();
            UserEntity secondActual = select.Data.Where(u => u.Username == secondExpected.Username).First();
            Assert.AreEqual(firstActual.Id, firstId);
            Assert.AreEqual(secondActual.Id, secondId);
            firstExpected.Id = firstActual.Id;
            firstExpected.Created = firstActual.Created;
            secondExpected.Id = secondActual.Id;
            secondExpected.Created = secondActual.Created;
            Assert.AreEqual(JsonConvert.SerializeObject(firstExpected), JsonConvert.SerializeObject(firstActual));
            Assert.AreEqual(JsonConvert.SerializeObject(secondExpected), JsonConvert.SerializeObject(secondActual));
        }

        [Test, Order(3)]
        public async Task CanSelectUserByIdAsync_UserIsSelected_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            IUserRepository userRepository = new UserRepository(sqlDataAccess);
            UserEntity expected = new UserEntity()
            {
                Username = "firstUsername",
                Password = "firstPassword",
                SecurityQuestion = "firstSecurityQuestion",
                SecurityAnswer = "firstSecurityAnswer"
            };

            // ACT
            ApiResponse<UserEntity> select = await userRepository.GetByIdAsync(firstId);

            // ASSERT
            Assert.NotNull(select.Data);
            Assert.Null(select.Error);
            Assert.IsTrue(select.Count == 1);
            Assert.NotZero(select.Data.Where(u => u.Username == expected.Username).Count());
            UserEntity actual = select.Data.Where(u => u.Username == expected.Username).First();
            Assert.AreEqual(actual.Id, firstId);
            expected.Id = actual.Id;
            expected.Created = actual.Created;
            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Test, Order(4)]
        public async Task CanSelectUserByUsernameAsync_UserIsSelected_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            IUserRepository userRepository = new UserRepository(sqlDataAccess);
            UserEntity expected = new UserEntity()
            {
                Username = "firstUsername",
                Password = "firstPassword",
                SecurityQuestion = "firstSecurityQuestion",
                SecurityAnswer = "firstSecurityAnswer"
            };

            // ACT
            ApiResponse<UserEntity> select = await userRepository.GetByUsernameAsync("firstUsername");

            // ASSERT
            Assert.NotNull(select.Data);
            Assert.Null(select.Error);
            Assert.IsTrue(select.Count == 1);
            Assert.NotZero(select.Data.Where(u => u.Username == expected.Username).Count());
            UserEntity actual = select.Data.Where(u => u.Username == expected.Username).First();
            Assert.AreEqual(actual.Id, firstId);
            expected.Id = actual.Id;
            expected.Created = actual.Created;
            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Test, Order(5)]
        public async Task CanUpdateUsernameAsync_UsernameIsUpdated_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            IUserRepository userRepository = new UserRepository(sqlDataAccess);
            UserEntity expected = new UserEntity()
            {
                Id = secondId,
                Username = "firstUsername",
                Password = "firstPassword",
                SecurityQuestion = "firstSecurityQuestion",
                SecurityAnswer = "firstSecurityAnswer"
            };

            // ACT
            ApiResponse update = await userRepository.UpdateAsync(expected);
            ApiResponse<UserEntity> actual = await userRepository.GetByIdAsync(secondId);


            // ASSERT
            Assert.NotNull(actual.Data);
            Assert.Null(actual.Error);
            Assert.IsTrue(actual.Count == 1);
            Assert.Null(update.Error);
            Assert.IsTrue(update.Count == 1);           
            expected.Created = actual.Data[0].Created;
            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual.Data[0]));
        }

        [Test, Order(6)]
        public async Task CanDeleteUsernameByIdAsync_UsernameIsDeleted_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            IUserRepository userRepository = new UserRepository(sqlDataAccess);            

            // ACT
            ApiResponse delete = await userRepository.DeleteByIdAsync(secondId);
            ApiResponse<UserEntity> actual = await userRepository.GetByIdAsync(secondId);


            // ASSERT
            Assert.Null(actual.Data);
            Assert.Null(actual.Error);
            Assert.IsTrue(actual.Count == 0);
            Assert.Null(delete.Error);
            Assert.IsTrue(delete.Count == 1);
        }

        [Test, Order(7)]
        public async Task CanChangeUsernamePasswordAsync_PasswordIsChanged_ReturnsTrue()
        {
            // ARRANGE
            IDataAccess sqlDataAccess = new SqlDataAccess(new SQLiteConnection(), new AppConfig() { ConnectionStrings = new Dictionary<string, string>() { { "SqLite", "Data Source=database.db;Version=3;" } } });
            IUserRepository userRepository = new UserRepository(sqlDataAccess);
            UserEntity expected = new UserEntity()
            {
                Id = firstId,
                Username = "firstUsername",
                Password = "changedPassword",
                SecurityQuestion = "firstSecurityQuestion",
                SecurityAnswer = "firstSecurityAnswer"
            };

            // ACT
            ApiResponse update = await userRepository.ChangePasswordAsync(expected);
            ApiResponse<UserEntity> actual = await userRepository.GetByIdAsync(firstId);


            // ASSERT
            Assert.NotNull(actual.Data);
            Assert.Null(actual.Error);
            Assert.NotZero(actual.Count);

            Assert.Null(update.Error);
            Assert.IsTrue(update.Count == 1);
            expected.Created = actual.Data[0].Created;
            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual.Data[0]));
        }
        #endregion
    }
}
