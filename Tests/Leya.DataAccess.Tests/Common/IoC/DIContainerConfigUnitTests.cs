/// Written by: Yulia Danilova
/// Creation Date: 14th of June, 2021
/// Purpose: Unit Test for the DIContainerConfig DAL class
#region ========================================================================= USING =====================================================================================
using System;
using Autofac;
using System.IO;
using System.Linq;
using System.Data;
using Autofac.Core;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Reflection;
using System.Data.SQLite;
using System.Collections.Generic;
using Leya.DataAccess.Common.IoC;
using Leya.DataAccess.StorageAccess;
using Leya.DataAccess.Repositories.Common;
using Leya.DataAccess.Common.Configuration;
#endregion

namespace Leya.DataAccess.Tests.Common.IoC
{
    [TestFixture]
    public class DIContainerConfigUnitTests
    {
        #region ================================================================= METHODS ===================================================================================
        [Test]
        public void CanConfigure_ConfigurationIsPerformed_ReturnsTrue()
        {
            // ARRANGE
            ContainerBuilder builder = new ContainerBuilder();
            if (File.Exists(Path.GetDirectoryName(Assembly.Load("Leya.DataAccess").Location) + "\\appsettings.json"))
                builder.Register(context => JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(Directory.GetCurrentDirectory() + "\\appsettings.json"))).As<IAppConfig>().SingleInstance();
            else
                Assert.Fail("Configuration file not found!\nPath: " + Path.GetDirectoryName(Assembly.Load("Leya.DataAccess").Location) + "\\appsettings.json");
            builder.RegisterType<SQLiteConnection>().As<IDbConnection>().InstancePerLifetimeScope();
            builder.RegisterType<SqlDataAccess>().FindConstructorsWith(new InternalConstructorFinder()).As<IDataAccess>().InstancePerDependency();
            builder.RegisterType<RepositoryFactory>().As<IRepositoryFactory>().SingleInstance();
            // get all classes implementing IRepository (all repository classes) and register them as their corresponding repository interface
            IEnumerable<Type> repositoryTypes = Assembly.Load("Leya.DataAccess")
                                                        .GetTypes()
                                                        .Where(t => !t.IsInterface &&
                                                                     t.GetInterfaces()
                                                                      .Any(i => i.IsGenericType &&
                                                                                i.GetGenericTypeDefinition() == typeof(IRepository<>)));
            foreach (Type type in repositoryTypes)
            {
                builder.RegisterType(type)
                       .As(type.GetInterfaces()
                               .Where(i => !i.IsGenericType &&
                                            i.GetInterfaces()
                                             .Any(j => j.GetGenericTypeDefinition() == typeof(IRepository<>)))
                               .First())
                       .InstancePerDependency();
            }
            IComponentRegistration[] expected = builder.Build().ComponentRegistry.Registrations.ToArray();

            // ACT
            IComponentRegistration[] actual = DIContainerConfig.Configure().ComponentRegistry.Registrations.ToArray();

            // ASSERT
            // expected and actual should have the same registered services 
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i].GetType(), actual[i].GetType());
        }
        #endregion
    }
}
