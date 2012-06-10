using System;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;

namespace Bobasoft.Presentation.Interactivity
{
	public abstract class EventTriggerBase : Trigger
	{
		//======================================================
		#region _Constructors_

		protected EventTriggerBase()
		{
			_onEventHandler = new Action<object, object>(OnEvent1);
		}

		#endregion

		//======================================================
		#region _Public properties_

		public object SourceObject
		{
			get { return GetValue(SourceObjectProperty); }
			set { SetValue(SourceObjectProperty, value); }
		}

		public object Source
		{
			get
			{
				object obj = AssociatedObject;
				if (SourceObject != null)
					obj = SourceObject;

				return obj;
			}
		}

		#endregion

		//======================================================
		#region _Private, protected, internal methods_

		protected override void OnAttached()
		{
			base.OnAttached();
			if (Source != null)
			{
				var eventName = GetEventName();
				if (string.IsNullOrEmpty(eventName))
					return;

				RegisterEvent(Source, eventName);
			}
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			if (Source != null)
			{
				var eventName = GetEventName();
				if (string.IsNullOrEmpty(eventName))
					return;

				UnregisterEvent(Source, eventName);
			}
		}
		
		protected abstract string GetEventName();

		protected virtual void OnEvent(RoutedEventArgs e)
		{
			InvokeActions(e);
		}

		protected virtual void OnEvent1(object s, object e)
		{
			OnEvent(e as RoutedEventArgs);
		}

		protected virtual void OnSourceObjectChanged(object oldValue, object newValue)
		{
			if (AssociatedObject == null)
				return;

			var eventName = GetEventName();
			if (string.IsNullOrEmpty(eventName))
				return;

			if (oldValue != null)
				UnregisterEvent(oldValue, eventName);
			if (newValue != null)
				RegisterEvent(newValue, eventName);
		}

		protected virtual void RegisterEvent(object obj, string eventName)
		{
			var @event = obj.GetType().GetRuntimeEvent(eventName);

			if (@event == null)
			{
				if (SourceObject == null)
					return;
				throw new ArgumentException("Invalid event name");
			}
			
			if (!IsValidEvent(@event))
			{
				if (SourceObject == null)
					return;

				throw new ArgumentException("Invalid event");
			}

			_onEventHandler = typeof(EventTriggerBase).GetTypeInfo().GetDeclaredMethod("OnEvent1").CreateDelegate(@event.EventHandlerType, this);

			WindowsRuntimeMarshal.AddEventHandler<object>(handler => (EventRegistrationToken)@event.AddMethod.Invoke(obj, new[] { handler }),
														  t => @event.RemoveMethod.Invoke(obj, new[] { (object)t }),
														  _onEventHandler);
		}
		
		private void UnregisterEvent(object obj, string eventName)
		{
			var @event = obj.GetType().GetRuntimeEvent(eventName);
			WindowsRuntimeMarshal.RemoveEventHandler(t => @event.RemoveMethod.Invoke(obj, new[] {(object)t}), _onEventHandler);
		}

		protected virtual bool IsValidEvent(EventInfo eventInfo)
		{
			var handlerType = eventInfo.EventHandlerType;
			if (!typeof(Delegate).IsAssignableFrom(handlerType))
				return false;
			var parameters = handlerType.GetTypeInfo().GetDeclaredMethod("Invoke").GetParameters();
			return parameters.Length == 2 && typeof(object).IsAssignableFrom(parameters[0].ParameterType);
		}

		protected virtual void OnEventNameChanged(string oldValue, string newValue)
		{
			if (AssociatedObject == null)
				return;

			if (!string.IsNullOrEmpty(oldValue))
				UnregisterEvent(Source, oldValue);

			if (!string.IsNullOrEmpty(newValue))
				RegisterEvent(Source, newValue);
		}

		#endregion

		//======================================================
		#region _Private, protected, internal fields_

		public static readonly DependencyProperty SourceObjectProperty =
			DependencyProperty.Register("SourceObject", typeof (object), typeof (EventTriggerBase), new PropertyMetadata(null, (s, e) => ((EventTriggerBase)s).OnSourceObjectChanged(e.OldValue, e.NewValue)));

		private Delegate _onEventHandler;

		#endregion
	}

	public abstract class EventTriggerBase<T> : EventTriggerBase where T : class
	{
		//======================================================
		#region _Public properties_

		public new T Source
		{
			get { return (T) base.Source; }
		}

		#endregion
	}
}