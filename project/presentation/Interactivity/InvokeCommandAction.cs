using System.Windows.Input;
using Windows.UI.Xaml;

namespace Bobasoft.Presentation.Interactivity
{
	public class InvokeCommandAction : TriggerAction<DependencyObject>
	{
		//======================================================
		#region _Public properties_

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		#endregion

		//======================================================
		#region _Private, protected, internal methods_

		protected override void Invoke(object parameter)
		{
			if (AssociatedObject == null)
				return;

			var cmd = ResolveCommand();

			if (cmd == null || !cmd.CanExecute(CommandParameter))
				return;

			cmd.Execute(CommandParameter);
		}

		protected virtual ICommand ResolveCommand()
		{
			return Command;
		}

		#endregion

		//======================================================
		#region _Private, protected, internal fields_
		
		public static readonly DependencyProperty CommandParameterProperty =
			DependencyProperty.Register("CommandParameter", typeof (object), typeof (InvokeCommandAction), new PropertyMetadata(default(object)));

		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register("Command", typeof (ICommand), typeof (InvokeCommandAction), new PropertyMetadata(default(ICommand)));
		
		#endregion
	}
}