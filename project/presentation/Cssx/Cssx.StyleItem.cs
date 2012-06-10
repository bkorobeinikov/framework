using Windows.UI.Xaml;

namespace Bobasoft.Presentation.Cssx
{
	public static partial class Cssx
	{
		public class StyleItem
		{
			//======================================================
			#region _Constructors_
			
			public StyleItem(StyleEntry parentEntry, DependencyProperty property, object value)
			{
				ParentEntry = parentEntry;
				Property = property;
				Value = value;
			}

			#endregion

			//======================================================
			#region _Public properties_

			public DependencyProperty Property { get; set; }
			public object Value { get; set; }

			public StyleEntry ParentEntry { get; set; }

			#endregion
		}
	}
}