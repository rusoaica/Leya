/// Written by: Yulia Danilova
/// Creation Date: 13th of June, 2021
/// Purpose: Custom collection for storing repositories
#region ========================================================================= USING =====================================================================================
using System;
using System.Linq;
using System.Collections.Generic;
#endregion

namespace Leya.DataAccess.Repositories.Common
{
    public class RepositoryDictionary
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        private readonly List<KeyValuePair<Type, object>> container = new List<KeyValuePair<Type, object>>();
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        public int Count { get { return container.Count; } }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Resets the collection of repositories
        /// </summary>
        public void Clear()
        {
            container.Clear();
        }

        /// <summary>
        /// Adds a repository of type <typeparamref name="TParam"/> to the collection
        /// </summary>
        /// <typeparam name="TParam">The type of the repository to add to the collection</typeparam>
        /// <param name="value">The repository to add</param>
        public void Add<TParam>(TParam value)
        {
            // enforce adding rules
            if (value == null)
                throw new ArgumentException("Value cannot be null!");
            if (container.Where(e => e.Key == value.GetType()).Count() > 0)
                throw new ArgumentException("Duplicate values are not allowed!");
            // check if the value implements an interface that implements IRepository
            bool implementsRepository = value.GetType()
                                             .GetInterfaces()
                                             .Any(e => e.GetInterfaces()
                                                        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>)));
            if (implementsRepository)
                container.Add(new KeyValuePair<Type, object>(value.GetType(), value));
            else
                throw new ArgumentException("Value must implement IRepository interface!");
        }

        /// <summary>
        /// Gets a repository of type <typeparamref name="TResult"/> from the collection
        /// </summary>
        /// <typeparam name="TResult">The type of the repository interface to get from the collection</typeparam>
        /// <param name="key">The concrete type of the repository interface to get</param>
        /// <returns>A repository implementing <typeparamref name="TResult"/></returns>
        public TResult Get<TResult>(Type key)
        {
            return (TResult)container.Where(e => e.Key == key).FirstOrDefault().Value;
        }
        #endregion
    }
}
