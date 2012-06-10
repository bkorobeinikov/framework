using System.Collections.Generic;

namespace Bobasoft.Presentation.Cssx
{
	public static partial class Cssx
	{
		public class StyleEntry
		{
			//======================================================
			#region _Constructors_

			public StyleEntry()
				: this(null)
			{
			}

			public StyleEntry(StyleEntry parentEntry)
			{
				ParentEntry = parentEntry;
				Items = new List<StyleItem>();
			}

			#endregion

			//======================================================
			#region _Public properties_

			public Selector Selector { get; set; }

			public List<StyleItem> Items { get; set; }

			public StyleEntry ParentEntry { get; set; }
			public StyleEntry ChildEntry { get; set; }

			#endregion
		}
	}
}