using System;
#if WinRT
using Windows.UI.Xaml;
#else
using System.Windows.Threading;
#endif


namespace Bobasoft.Presentation
{
    public class DispatcherDelayTimer : DispatcherTimer
    {
        /// <summary>
        /// uncomment this to see when the DispatcherTimerContainingAction is collected
        /// if you remove  t.Tick -= _onTimeout; line from _onTimeout method
        /// you will see that the timer is never collected
        /// </summary>
        //~DispatcherTimerContainingAction()
        //{
        //    throw new Exception("DispatcherTimerContainingAction is disposed");
        //}

        //======================================================
        #region _Public properties_

        /// <summary>
        /// The callback action.
        /// </summary>
        public Action Action { get; set; }
        
        #endregion

        //======================================================
        #region _Public methods_

        public static DispatcherDelayTimer StartOnTimeout(int milliseconds, Action callback)
        {
            return StartOnTimeout(new TimeSpan(0, 0, 0, 0, milliseconds), callback);
        }

        public static DispatcherDelayTimer StartOnTimeout(TimeSpan internval, Action callback)
        {
            var timer = new DispatcherDelayTimer
                        {
                Interval = internval,
                Action = callback
            };
            timer.Tick += OnTimeout;
            timer.Start();

            return timer;
        }

        #endregion

        //======================================================
        #region _Private, protected, internal methods_

#if WinRT
		private static void OnTimeout(object sender, object args)
#else
        private static void OnTimeout(object sender, EventArgs args)
#endif
        {
            var t = sender as DispatcherDelayTimer;
            if (t != null)
            {
                t.Tick -= OnTimeout;
                t.Stop();
                t.Action();
            }
        }

        #endregion
    }
}