namespace Bobasoft.Presentation.Interactivity
{
	public class TriggerCollection : AttachableCollection<Trigger>
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

		protected override void ItemAdded(Trigger item)
		{
			if (AssociatedObject == null)
				return;
			item.Attach(AssociatedObject);
		}

		protected override void ItemRemoved(Trigger item)
		{
			if (item.AssociatedObject == null)
				return;
			item.Detach();
		}

		#endregion
	}
}