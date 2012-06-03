using System.ComponentModel;

namespace Bobasoft
{
	public class ObservableObject : INotifyPropertyChanged
	{
		//======================================================
		#region _Public properties_

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		//======================================================
		#region _Public methods_

		protected bool SetProperty<T>(ref T prop, T value, string propertyName)
		{
			if (Equals(prop, value)) return false;

			prop = value;
			RaisePropertyChanged(propertyName);
			return true;
		}

		protected void RaisePropertyChanged(string propertyName)
		{
			var eventHandler = PropertyChanged;
			if (eventHandler != null)
				eventHandler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}