/// Written by: Yulia Danilova, Sameh Salem
/// Creation Date: 12th of June, 2021
/// Purpose: Interaction boundary with the Data Access Layer
#region ========================================================================= USING =====================================================================================
using System;
using Autofac;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Leya.DataAccess.Repositories.Common;
#endregion

namespace Leya.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly IRepositoryFactory repositoryFactory;
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        internal RepositoryDictionary Repositories { get; private set; } = new RepositoryDictionary();
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public UnitOfWork(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
            AddRepositories();
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Adds all the repositories from the Data Access Layer so that they can be exposed to the Business Layer
        /// </summary>
        internal void AddRepositories()
        {
            // get all the concrete implementations of IRepository<>
            IEnumerable<Type> repositoryClassTypes =
                Assembly.GetExecutingAssembly()
                        .GetTypes()
                        .Where(t => !t.IsInterface && !t.IsAbstract && t.GetInterfaces()
                                                                        .Any(i => i.IsGenericType &&
                                                                                  i.GetGenericTypeDefinition() == typeof(IRepository<>)));
            // store all repositories
            foreach (Type repositoryClassType in repositoryClassTypes)
            {
                // get the interface that implements IRepository<> of the currently iterated repository class
                Type repositoryInterfaceType = repositoryClassType.GetInterfaces()
                                                                  .Where(i => !i.IsGenericType && 
                                                                               i.GetInterfaces()
                                                                                .Any(a => a.GetGenericTypeDefinition() == typeof(IRepository<>)))
                                                                  .First();
                // ask the concrete type for the repository interface type from the repositories factory;
                // because the method for creating a repository is generic and we need to call it with a runtime type, reflection is the only option
                object repositoryClass = typeof(IRepositoryFactory).GetMethod("CreateRepository")
                                                                   .MakeGenericMethod(repositoryInterfaceType)
                                                                   .Invoke(repositoryFactory, null);
                Repositories.Add(repositoryClass);
            }
        }
       
        /// <summary>
        /// Exposes a repository of type <typeparamref name="TRepository"/> to the Business Layer
        /// </summary>
        /// <typeparam name="TRepository">The type of the exposed repository</typeparam>
        /// <returns>A repository of type <typeparamref name="TRepository"/></returns>
        public TRepository GetRepository<TRepository>() 
        {
            // get the repository type based on the type of the provided repository interface
            Type repositoryType = Assembly.GetExecutingAssembly()
                                          .GetTypes()
                                          .Where(t => !t.IsInterface && !t.IsAbstract && typeof(TRepository).IsAssignableFrom(t))
                                          .First();
            return Repositories.Get<TRepository>(repositoryType);
        }

        /// <summary>
        /// Resets the list of repositories
        /// </summary>
        internal void ResetRepositories()
        {
            Repositories.Clear();
        }
        #endregion
    }
}
