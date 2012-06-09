using System;
using Windows.UI.Xaml;

namespace Bobasoft.Presentation.Interactivity
{
	public abstract class TriggerAction : ConstainedAttachableObject
	{
		//======================================================
		#region _Constructors_

		protected TriggerAction(Type associatedObjectTypeConstaint) : base(associatedObjectTypeConstaint)
		{
		}

		#endregion

		//======================================================
		#region _Public properties_

		public bool IsEnabled
		{
			get { return (bool)GetValue(IsEnabledProperty); }
			set { SetValue(IsEnabledProperty, value); }
		}

		#endregion

		//======================================================
		#region _Public methods_

		public void CallInvoke(object parameter)
		{
			if (!IsEnabled)
				return;
			Invoke(parameter);
		}

		#endregion

		//======================================================
		#region _Private, protected, internal methods_

		protected abstract void Invoke(object parameter);
		
		#endregion

		//======================================================
		#region _Private, protected, internal fields_

		public static readonly DependencyProperty IsEnabledProperty =
			DependencyProperty.Register("IsEnabled", typeof (bool), typeof (TriggerAction), new PropertyMetadata(true));

		#endregion
	}

	public abstract class TriggerAction<T> : TriggerAction where T : DependencyObject
	{
		//======================================================
		#region _Constructors_

		protected TriggerAction() 
			: base(typeof(T))
		{
		}

		#endregion

		//======================================================
		#region _Public properties_

		protected new T AssociatedObject
		{
			get { return (T) base.AssociatedObject; }
		}

		#endregion
	}
}