/// Written by: Yulia Danilova, Sameh Salem
/// Creation Date: 12th of June, 2021
/// Purpose: Concrete implementation for the repositories abstract factory
#region ========================================================================= USING =====================================================================================
using Autofac;
#endregion

namespace Leya.DataAccess.Repositories.Common
{
    public class RepositoryFactory : IRepositoryFactory
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly ILifetimeScope container;
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Overload C-tor
        /// </summary>
        /// <param name="container">The DI container that resolves the requested repositories</param>
        public RepositoryFactory(ILifetimeScope container)
        {
            this.container = container;
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Creates a new repository of type <typeparamref name="TResult"/>
        /// </summary>
        /// <typeparam name="TResult">The type of repository to create</typeparam>
        /// <returns>A repository of type <typeparamref name="TResult"/></returns>
        public TResult CreateRepository<TResult>() 
        {
            return container.Resolve<TResult>();
        }
        #endregion
    }
}
