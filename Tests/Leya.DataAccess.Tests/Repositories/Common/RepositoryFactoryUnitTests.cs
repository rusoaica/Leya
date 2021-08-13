/// Written by: Yulia Danilova
/// Creation Date: 14th of June, 2021
/// Purpose: Unit Test for the RepositoryFactory DAL class
#region ========================================================================= USING =====================================================================================
using Autofac;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Data.SQLite;
using Leya.DataAccess.Common.IoC;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Repositories.Common;
using Leya.DataAccess.Common.Configuration;
using Leya.DataAccess.Repositories.Episodes;
#endregion

namespace Leya.DataAccess.Tests.Repositories.Common
{
    [TestFixture]
    public class RepositoryFactoryUnitTests
    {
        #region ================================================================= METHODS ===================================================================================
        [Test]
        public void CanCreateRepository_RepositoryIsCreated_ReturnsTrue()
        {
            // ARRANGE
            UnitOfWork unitOfWork = new UnitOfWork();

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RepositoryFactory>().As<IRepositoryFactory>().SingleInstance();
            builder.RegisterType<EpisodeRepository>().As<IEpisodeRepository>().InstancePerDependency();
            builder.RegisterType<SqlDataAccess>().FindConstructorsWith(new InternalConstructorFinder()).As<IDataAccess>().InstancePerDependency();
            builder.RegisterType<SQLiteConnection>().As<IDbConnection>().InstancePerLifetimeScope();
            builder.Register(context => JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(Directory.GetCurrentDirectory() + "\\appsettings.json"))).As<IAppConfig>().SingleInstance();

            IContainer container = builder.Build();
            ILifetimeScope scope = container.BeginLifetimeScope();
            IEpisodeRepository expected = unitOfWork.GetRepository<IEpisodeRepository>();
            IRepositoryFactory repositoryFactory = container.Resolve<IRepositoryFactory>();
            scope.Dispose();

            // ACT
            IEpisodeRepository actual = repositoryFactory.CreateRepository<IEpisodeRepository>();

            // ASSERT
            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }
        #endregion
    }
}
