/// Written by: Yulia Danilova
/// Creation Date: 27th of November, 2019
/// Purpose: Explicit implementation of abstract Dispatcher interface, used in UI environments
#region ========================================================================= USING =====================================================================================
using System;
using System.Windows;
using System.Windows.Threading;
using Leya.ViewModels.Common.Dispatcher;
#endregion

namespace Leya.Views.Common.Dispatcher
{
    public class ApplicationDispatcher : IDispatcher
    {
        #region =============================================================== PROPERTIES ==================================================================================
        System.Windows.Threading.Dispatcher UnderlyingDispatcher
        {
            get
            {
                if (Application.Current == null)
                    throw new InvalidOperationException("You must call this method from within a running WPF application!");
                if (Application.Current.Dispatcher == null)
                    throw new InvalidOperationException("You must call this method from within a running WPF application with an active dispatcher!");
                return Application.Current.Dispatcher;
            }
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        /// <summary>
        /// Executes the specified delegate with the specified argument, synchronously
        /// </summary>
        /// <param name="method">A delegate to a method that takes one argument, which is pushed onto the System.Windows.Threading.Dispatcher event queue.</param>
        /// <param name="args">An object to pass as an argument to the given method.</param>
        public void Dispatch(Delegate method, params object[] args)
        {
            UnderlyingDispatcher.Invoke(DispatcherPriority.Background, method, args);
        }

        /// <summary>
        /// Executes the specified delegate synchronously
        /// </summary>
        /// <typeparam name="TResult">The type of result returned by <paramref name="callback"/></typeparam>
        /// <param name="callback">A func returning a result of type <typeparamref name="TResult"/></param>
        /// <returns>A Func callback of type <typeparamref name="TResult"/></returns>
        public TResult Dispatch<TResult>(Func<TResult> callback)
        {
            return UnderlyingDispatcher.Invoke(callback);
        }
        #endregion
    }
}
