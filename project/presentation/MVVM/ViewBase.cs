using System.Windows.Controls;

namespace Bobasoft.Presentation.MVVM
{
    public class ViewBase : UserControl
    {
        //======================================================
        #region _Public properties_

        public ViewModel Model
        {
            get { return (ViewModel)DataContext; }
            set
            {
                if (DataContext != value)
                {
                    if (value != null)
                    {
                        value.View = this;
                        value.InitializeInternal();
                    }
                    DataContext = value;
                }
            }
        }

        #endregion
    }
}