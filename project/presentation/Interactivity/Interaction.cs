using System;
using Windows.UI.Xaml;

namespace Bobasoft.Presentation.Interactivity
{
	public static class Interaction
	{
		//======================================================
		#region _Public methods_
		
		public static TriggerCollection GetTriggers(UIElement element)
		{
			var collection = (TriggerCollection)element.GetValue(TriggersProperty);

			if (collection == null)
			{
				collection = new TriggerCollection();
				element.SetValue(TriggersProperty, collection);
			}

			return collection;
		}

		#endregion

		//======================================================
		#region _Private, protected, internal methods_

		private static void OnTriggersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var oldValue = (TriggerCollection)e.OldValue;
			var newValue = (TriggerCollection)e.NewValue;

			if (oldValue == newValue)
				return;

			if (oldValue != null && oldValue.AssociatedObject != null)
				oldValue.Detach();

			if (newValue == null || d == null)
				return;

			if (newValue.AssociatedObject != null)
				throw new InvalidOperationException("Cannot host trigger collection multiple times");

			newValue.Attach(d);
		}

		#endregion

		//======================================================
		#region _Private, protected, internal fields_
		
		public static readonly DependencyProperty TriggersProperty =
			DependencyProperty.RegisterAttached("ShadowTriggers", typeof (TriggerCollection), typeof (Interaction), new PropertyMetadata(default(TriggerCollection), OnTriggersChanged));

		#endregion
	}
}