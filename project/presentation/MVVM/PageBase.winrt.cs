using Windows.UI.Xaml.Controls;

namespace Bobasoft.Presentation.MVVM
{
	public class PageBase : Page
	{
		//======================================================
		#region _Public properties_

		public PageViewModel Model
		{
			get { return (PageViewModel)DataContext; }
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

		//======================================================
		#region _Private, protected, internal methods_

		protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			Model.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			Model.OnNavigatedFrom(e);
		}

		protected override void OnNavigatingFrom(Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
		{
			base.OnNavigatingFrom(e);
			Model.OnNavigatingFrom(e);
		}

		#endregion
	}
}