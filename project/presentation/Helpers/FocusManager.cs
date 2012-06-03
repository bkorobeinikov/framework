using System;
using System.Windows;
using System.Windows.Controls;

namespace Bobasoft.Presentation
{
    public class FocusChangedArgs : EventArgs
    {
        //======================================================
        #region _Constructors_

        public FocusChangedArgs(object previousElement, object currentElement)
        {
            PreviousElement = previousElement;
            CurrentElement = currentElement;
        }

        #endregion

        //======================================================
        #region _Public properties_

        public object PreviousElement { get; private set; }
        public object CurrentElement { get; private set; }

        #endregion
    }

    public static class FocusManager
    {
        //======================================================
        #region _Public properties_

        private static UserControl _root;
        public static UserControl Root
        {
            get { return _root ?? (_root = Application.Current.RootVisual as UserControl); }
        }

        public static event EventHandler<FocusChangedArgs> FocusChanged;

        #endregion
        
        //======================================================
        #region _Public methods_

        public static void Initialize()
        {
            Root.GotFocus += OnGlobalGotFocus;
            Root.LostFocus += OnGlobalLostFocus;

            _focusedElement = GetFocusedElement();
        }

        public static object GetFocusedElement()
        {
            _focusedElement = System.Windows.Input.FocusManager.GetFocusedElement();
            return _focusedElement;
        }

        public static bool Unfocus()
        {
            var focusedon = GetFocusedElement();
            if (focusedon != Root)
            {
                Root.Focus();
                //RaiseFocuChanged(new FocusChangedArgs(focusedon, Root));
                return true;
            }

            return false;
        }

        public static void SetFocus(FrameworkElement element)
        {
            if (element is UserControl)
            {
                ((UserControl) element).Focus();
                //RaiseFocuChanged(new FocusChangedArgs(element, Root));
                return;
            }

            throw new NotSupportedException("Cannot set focus on object of type {0}".With(element.GetType()));
        }

        #endregion

        //======================================================
        #region _Private, protected, internal methods_

        private static void OnGlobalGotFocus(object sender, RoutedEventArgs e)
        {
            if (_focusedElement != e.OriginalSource)
            {
                RaiseFocuChanged(new FocusChangedArgs(_focusedElement, e.OriginalSource));
                _focusedElement = e.OriginalSource;
            }
        }

        private static void OnGlobalLostFocus(object sender, RoutedEventArgs e)
        {
            // 
        }

        private static void RaiseFocuChanged(FocusChangedArgs args)
        {
            var handler = FocusChanged;
            if (handler != null)
                handler(null, args);
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private static object _focusedElement;

        #endregion
    }
}