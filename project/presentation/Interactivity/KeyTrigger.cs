using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Bobasoft.Presentation.Interactivity
{
	public class KeyTrigger : EventTriggerBase<UIElement>
	{
		//======================================================
		#region _Public properties_

		public int Modifiers
		{
			get { return (int) GetValue(ModifiersProperty); }
			set { SetValue(ModifiersProperty, value); }
		}

		public VirtualKey Key
		{
			get { return (VirtualKey) GetValue(KeyProperty); }
			set { SetValue(KeyProperty, value); }
		}

		public KeyTriggerFiredOn FiredOn
		{
			get { return (KeyTriggerFiredOn) GetValue(FiredOnProperty); }
			set { SetValue(FiredOnProperty, value); }
		}

		public bool ActiveOnFocus
		{
			get { return (bool) GetValue(ActiveOnFocusProperty); }
			set { SetValue(ActiveOnFocusProperty, value); }
		}

		#endregion

		//======================================================
		#region _Private, protected, internal methods_

		protected override string GetEventName()
		{
			return "Loaded";
		}

		protected virtual void OnKeyPress(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key != Key || GetCurrentModifiers() != (VirtualKeyModifiers)Modifiers)
				return;

			//Debug.WriteLine("Key: {0}", e.Key);
			InvokeActions(e);
			e.Handled = true;
		}

		private VirtualKeyModifiers GetCurrentModifiers()
		{
			var modifiers = VirtualKeyModifiers.None;

			var current = CoreWindow.GetForCurrentThread();
			if (current.GetKeyState(VirtualKey.RightControl) != CoreVirtualKeyStates.None || current.GetKeyState(VirtualKey.LeftControl) != CoreVirtualKeyStates.None || current.GetKeyState(VirtualKey.Control) != CoreVirtualKeyStates.None)
				modifiers |= VirtualKeyModifiers.Control;
			if (current.GetKeyState(VirtualKey.RightMenu) != CoreVirtualKeyStates.None || current.GetKeyState(VirtualKey.LeftMenu) != CoreVirtualKeyStates.None)
				modifiers |= VirtualKeyModifiers.Menu;
			if (current.GetKeyState(VirtualKey.RightShift) != CoreVirtualKeyStates.None || current.GetKeyState(VirtualKey.LeftShift) != CoreVirtualKeyStates.None)
				modifiers |= VirtualKeyModifiers.Shift;
			if (current.GetKeyState(VirtualKey.RightWindows) != CoreVirtualKeyStates.None || current.GetKeyState(VirtualKey.LeftWindows) != CoreVirtualKeyStates.None)
				modifiers |= VirtualKeyModifiers.Windows;

			//Debug.WriteLine("Modifiers: {0}", modifiers);

			return modifiers;
		}

		protected override void OnEvent(RoutedEventArgs e)
		{
			_target = !ActiveOnFocus ? GetRoot(Source) : Source;

			if (FiredOn == KeyTriggerFiredOn.KeyDown)
				_target.KeyDown += OnKeyPress;
			else
				_target.KeyUp += OnKeyPress;
		}

		protected override void OnDetaching()
		{
			if (_target != null)
			{
				if (FiredOn == KeyTriggerFiredOn.KeyDown)
					_target.KeyDown -= OnKeyPress;
				else if (FiredOn == KeyTriggerFiredOn.KeyUp)
					_target.KeyUp -= OnKeyPress;
			}

			base.OnDetaching();
		}

		private static UIElement GetRoot(DependencyObject current)
		{
			UIElement el = null;
			for(; current != null; current = VisualTreeHelper.GetParent(current))
				el = current as UIElement;

			return el;
		}

		#endregion

		//======================================================
		#region _Private, protected, internal fields_
		
		public static readonly DependencyProperty KeyProperty =
			DependencyProperty.Register("Key", typeof (VirtualKey), typeof (KeyTrigger), new PropertyMetadata(VirtualKey.Enter));

		public static readonly DependencyProperty ModifiersProperty =
			DependencyProperty.Register("Modifiers", typeof(int), typeof(KeyTrigger), new PropertyMetadata(0));

		public static readonly DependencyProperty ActiveOnFocusProperty =
			DependencyProperty.Register("ActiveOnFocus", typeof (bool), typeof (KeyTrigger), new PropertyMetadata(true));

		public static readonly DependencyProperty FiredOnProperty =
			DependencyProperty.Register("FiredOn", typeof (KeyTriggerFiredOn), typeof (KeyTrigger), new PropertyMetadata(KeyTriggerFiredOn.KeyDown));

		private UIElement _target;

		#endregion
	}
}