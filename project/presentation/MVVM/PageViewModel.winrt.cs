using System;

namespace Bobasoft.Presentation.MVVM
{
	public class PageViewModel : ModelBase
	{
		//======================================================
		#region _Public properties_

		public PageBase Page { get; set; }

		#endregion

		//======================================================
		#region _Private, protected, internal methods_

		protected virtual void GoHome()
		{
			if (Page.Frame != null)
				while (Page.Frame.CanGoBack) 
					Page.Frame.GoBack();
		}

		protected virtual void GoBack()
		{
			if (Page.Frame != null && Page.Frame.CanGoBack)
				Page.Frame.GoBack();
		}

		protected virtual void GoForward()
		{
			if (Page.Frame != null && Page.Frame.CanGoForward)
				Page.Frame.GoForward();
		}

		protected virtual void Navigate(Type pageType)
		{
			if (Page.Frame != null)
				Page.Frame.Navigate(pageType);
		}

		protected virtual void Navigate(Uri uri)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}