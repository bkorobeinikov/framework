using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;

namespace Bobasoft.Presentation.MVVM
{
	public class PageViewModel : ModelBase
	{
		//======================================================
		#region _Public properties_

		public PageBase Page { get; set; }

		public CoreDispatcher Dispatcher
		{
			get { return Page.Dispatcher; }
		}

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

		protected internal virtual void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		protected internal virtual void OnNavigatedFrom(NavigationEventArgs e)
		{
		}

		protected internal virtual void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
		}

		public virtual void DispatchAsync(DispatchedHandler action, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
		{
			Page.Dispatcher.RunAsync(priority, action);
		}

		#endregion
	}
}