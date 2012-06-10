using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Bobasoft.Presentation.Interactivity
{
	[ContentProperty(Name = "Actions")]
	public abstract class Trigger : AttachableObject
	{
		//======================================================
		#region _Constructors_

		protected Trigger()
		{
			var collection = new TriggerActionCollection();
			SetValue(ActionsProperty, collection);
		}

		#endregion

		//======================================================
		#region _Public properties_

		public TriggerActionCollection Actions
		{
			get { return (TriggerActionCollection)GetValue(ActionsProperty); }
		}

		#endregion

		//======================================================
		#region _Public properties_

		public event EventHandler<CancelEventArgs> PreviewInvoke;

		#endregion

		//======================================================
		#region _Private, protected, internal methods_

		protected void InvokeActions(object parameter)
		{
			var e = new CancelEventArgs();
			RaisePreviewInvoke(e);
			if (e.Cancel)
				return;

			foreach (var triggerAction in Actions)
				triggerAction.CallInvoke(parameter);
		}

		protected override void OnAttached()
		{
			Actions.Attach(AssociatedObject);
		}

		protected override void OnDetaching()
		{
			Actions.Detach();
		}

		protected void RaisePreviewInvoke(CancelEventArgs e)
		{
			if (PreviewInvoke != null)
				PreviewInvoke(this, e);
		}

		#endregion

		//======================================================
		#region _Private, protected, internal fields_

		public static readonly DependencyProperty ActionsProperty =
			DependencyProperty.Register("Actions", typeof (TriggerActionCollection), typeof (Trigger), new PropertyMetadata(null));

		#endregion
	}
}