using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Bobasoft.Presentation.Cssx
{
	public static partial class Cssx
	{
		//======================================================
		#region _Public methods_

		public static void SetFile(UIElement element, string value)
		{
			element.SetValue(FileProperty, value);
		}

		public static string GetFile(UIElement element)
		{
			return (string)element.GetValue(FileProperty);
		}

		#endregion

		//======================================================
		#region _Private, protected, internal methods_

		private static void OnFileChanged(DependencyObject d, string oldValue, string newValue)
		{
			if (!string.IsNullOrEmpty(newValue))
			{
				var styleEntry = Parse(newValue);
				Apply(styleEntry, d);
			}
		}

		private static void Apply(StyleEntry entry, DependencyObject d)
		{
			if (d.GetType().Name == entry.Selector.Tag)
			{
				foreach (var item in entry.Items)
					d.SetValue(item.Property, item.Value);
			}

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
			{
				var child = VisualTreeHelper.GetChild(d, i);
				if (child != null)
				{
					Apply(entry, child);		
				}
			}

			if (entry.ChildEntry == null)
				return;
			
			Apply(entry.ChildEntry, d);
		}

		private static StyleEntry Parse(string str)
		{
			var entry = new StyleEntry();

			var selector = new Selector {Tag = "TextBlock"};
			var item = new StyleItem(entry, TextBlock.ForegroundProperty, Colors.Red);

			entry.Selector = selector;
			entry.Items.Add(item);

			return entry;
		}

		#endregion

		//======================================================
		#region _Private, protected, internal fields_
		
		public static readonly DependencyProperty FileProperty =
			DependencyProperty.RegisterAttached("File", typeof (string), typeof (Cssx), new PropertyMetadata(null, (o, args) => OnFileChanged(o, (string) args.OldValue, (string) args.NewValue)));

		#endregion
	}
}