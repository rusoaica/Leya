/// Written by: Yulia Danilova
/// Creation Date: 11th of June, 2021
/// Purpose: Interaction boundary with the Data Access Layer

namespace Leya.DataAccess
{
    public interface IUnitOfWork 
    {
        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Exposes a repository of type <typeparamref name="TRepository"/> to the Business Layer
        /// </summary>
        /// <typeparam name="TRepository">The type of the exposed repository</typeparam>
        /// <returns>A repository of type <typeparamref name="TRepository"/></returns>
        TRepository GetRepository<TRepository>();
        #endregion
    }
}
