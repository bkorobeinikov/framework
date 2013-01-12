using System.Windows.Controls;

namespace Bobasoft.Presentation.MVVM
{
    public class PageBase : Page
    {
        //======================================================
        #region _Public properties_

        public PageModel Model
        {
            get { return (PageModel)DataContext; }
            set
            {
                if (DataContext != value)
                {
                    if (value != null)
                    {
                        value.Page = this;
                        value.InitializeInternal();
                    }
                    DataContext = value;
                }
            }
        }

        #endregion
    }
}