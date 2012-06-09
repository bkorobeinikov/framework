using Windows.UI.Xaml;

namespace Bobasoft.Presentation.Interactivity
{
	public class EventTrigger : EventTriggerBase<object>
	{
		//======================================================
		#region _Public properties_

		public string EventName
		{
			get { return (string)GetValue(EventNameProperty); }
			set { SetValue(EventNameProperty, value); }
		}

		#endregion

		//======================================================
		#region _Private, protected, internal methods_

		protected override string GetEventName()
		{
			return EventName;
		}

		#endregion

		//======================================================
		#region _Private, protected, internal fields_
		
		public static readonly DependencyProperty EventNameProperty =
			DependencyProperty.Register("EventName", typeof (string), typeof (EventTrigger), new PropertyMetadata("Loaded", (s, e) => ((EventTrigger)s).OnEventNameChanged((string) e.OldValue, (string) e.NewValue)));

		#endregion
	}
}