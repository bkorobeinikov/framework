using System.ComponentModel;
using System.Windows;

namespace Bobasoft.Presentation
{
    public static class ClientEnvironment
    {
        //======================================================
        #region _Public properties_

        /// <summary>
        /// Gets whether current execution preforms in design mode.
        /// </summary>
        public static bool IsInDesignMode
        {
#if SILVERLIGHT
            get { return DesignerProperties.IsInDesignTool; }
#elif WinRT
			get { return Windows.ApplicationModel.DesignMode.DesignModeEnabled; }
#else
            get { return (bool) (DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof (DependencyObject)).DefaultValue); }
#endif
        }

        #endregion
    }
}