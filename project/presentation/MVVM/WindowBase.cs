using System.Windows;

namespace Bobasoft.Presentation.MVVM
{
    public class WindowBase : Window
    {
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

        #endregion
    }
}