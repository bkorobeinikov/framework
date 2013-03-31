using System;
using System.Windows;

namespace Bobasoft.Presentation.MVVM
{
    public class WindowBase : Window
    {
        //======================================================
        #region _Constructors_

        public WindowBase()
        {
            base.Activated += (sender, args) =>
                {
                    var newEventArgs = new RoutedEventArgs(ActivatedEvent);
                    RaiseEvent(newEventArgs);
                };

            base.Deactivated += (sender, args) =>
                {
                    var newEventArgs = new RoutedEventArgs(DeactivatedEvent);
                    RaiseEvent(newEventArgs);
                };
        }

        #endregion

        //======================================================
        #region _Public properties_

        public WindowModel Model
        {
            get { return (WindowModel)DataContext; }
            set
            {
                if (DataContext != value)
                {
                    if (value != null)
                    {
                        value.Window = this;
                        value.InitializeInternal();
                    }
                    DataContext = value;
                }
            }
        }

        // Provide CLR accessors for the event 
        public new event RoutedEventHandler Activated
        {
            add { AddHandler(ActivatedEvent, value); }
            remove { RemoveHandler(ActivatedEvent, value); }
        }

        public new event RoutedEventHandler Deactivated
        {
            add { AddHandler(DeactivatedEvent, value); }
            remove { RemoveHandler(DeactivatedEvent, value); }
        }

        #endregion

        //======================================================
        #region _Private, protected, internal methods_


        #endregion

        //======================================================
        #region _Fields_

        public static readonly RoutedEvent ActivatedEvent = EventManager.RegisterRoutedEvent("Activated", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WindowBase));
        public static readonly RoutedEvent DeactivatedEvent = EventManager.RegisterRoutedEvent("Deactivated", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WindowBase));

        #endregion
    }
}