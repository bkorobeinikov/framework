using System;
#if WinRT
using Windows.UI.Xaml;
#else
using System.Windows;
using System.Windows.Threading;
#endif

namespace Bobasoft.Presentation
{
    public static class SmartDispatcher
    {
#if !WinRT
        public static Dispatcher DefaultDispatcher
        {
            get
            {
#if SILVERLIGHT
                return Deployment.Current.Dispatcher;
#else
                return Application.Current.Dispatcher;
#endif
            }
        }

        /// <summary>
        /// Checks for CheckAccess(), if false, dispatch throught .BeginInvoke method; otherwise just call action.
        /// </summary>
        /// <param name="action">The action to dispatch.</param>
        public static void DispatchAsync(Action action)
        {
            var dispatcher = DefaultDispatcher;
            if (!dispatcher.CheckAccess())
                dispatcher.BeginInvoke(action);
            else
                action();
        }

#if !SILVERLIGHT
        /// <summary>
        /// Checks for CheckAccess(), if false, dispatch throught .Invoke method; otherwise just call action.
        /// </summary>
        /// <param name="action">The action to dispatch.</param>
        public static void Dispatch(Action action)
        {
            var dispatcher = DefaultDispatcher;
            if (!dispatcher.CheckAccess())
                dispatcher.Invoke(action);
            else
                action();      
        }
#endif

        /// <summary>
        /// Call .BeginInvoke action anyway. 
        /// </summary>
        /// <param name="action">The action to dispatch.</param>
        public static void DispatchAsyncAnyway(Action action)
        {
            DefaultDispatcher.BeginInvoke(action);
        }

#if !SILVERLIGHT
        /// <summary>
        /// Call .Invoke action anyway. 
        /// </summary>
        /// <param name="action">The action to dispatch.</param>
        public static void DispatchAnyway(Action action)
        {
            DefaultDispatcher.Invoke(action);
        }
#endif
#endif
    }
}