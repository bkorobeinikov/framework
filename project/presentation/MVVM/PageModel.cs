namespace Bobasoft.Presentation.MVVM
{
    public class PageModel : ModelBase
    {
        //======================================================
        #region _Public properties_

        public PageBase Page { get; set; }

        #endregion

        //======================================================
        #region _Public methods_

        public virtual void OnNavigatedTo()
        {
            
        }

        public virtual void OnNavigatingFrom()
        {
            
        }

        #endregion
    }
}