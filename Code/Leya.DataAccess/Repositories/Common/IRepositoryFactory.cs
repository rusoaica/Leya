/// Written by: Yulia Danilova, Sameh Salem
/// Creation Date: 12th of June, 2021
/// Purpose: Repositories abstract factory

namespace Leya.DataAccess.Repositories.Common
{
    public interface IRepositoryFactory
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Creates a new repository of type <typeparamref name="TResult"/>
        /// </summary>
        /// <typeparam name="TResult">The type of repository to create</typeparam>
        /// <returns>A repository of type <typeparamref name="TResult"/></returns>
        TResult CreateRepository<TResult>();
        #endregion
    }
}
