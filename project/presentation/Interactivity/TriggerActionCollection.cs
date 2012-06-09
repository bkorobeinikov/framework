using System;

namespace Bobasoft.Presentation.Interactivity
{
	public class TriggerActionCollection : AttachableCollection<TriggerAction>
	{
		//======================================================
		#region _Private, protected, internal methods_

		protected override void OnAttached()
		{
			foreach (var trigger in this)
				trigger.Attach(AssociatedObject);
		}

		protected override void OnDetaching()
		{
			foreach (var trigger in this)
				trigger.Detach();
		}

		protected override void ItemAdded(TriggerAction item)
		{
			if (item.IsHosted)
				throw new InvalidOperationException("Cannot host trigger action multiple times");

			if (AssociatedObject != null)
				item.Attach(AssociatedObject);
			item.IsHosted = true;
		}

		protected override void ItemRemoved(TriggerAction item)
		{
			if (AssociatedObject != null)
				item.Detach();
			item.IsHosted = false;
		}

		#endregion
	}
}