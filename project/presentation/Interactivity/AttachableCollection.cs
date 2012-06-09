using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Xaml;

namespace Bobasoft.Presentation.Interactivity
{
	public abstract class AttachableCollection<T> :  ObservableCollection<T>, IAttachedObject where T : IAttachedObject
	{
		//======================================================
		#region _Constructors_

		protected AttachableCollection()
		{
			_snapshot = new List<T>();
		}

		#endregion

		//======================================================
		#region _Public properties_

		public DependencyObject AssociatedObject { get; protected set; }

		#endregion

		//======================================================
		#region _Public methods_

		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject == AssociatedObject)
				return;

			if (AssociatedObject != null)
				throw new InvalidOperationException("Already attached");

			AssociatedObject = dependencyObject;
			OnAttached();
		}

		public void Detach()
		{
			OnDetaching();
			AssociatedObject = null;
		}

		#endregion
		
		//======================================================
		#region _Private, protected, internal methods_

		protected abstract void OnAttached();
		protected abstract void OnDetaching();

		protected abstract void ItemAdded(T item);
		protected abstract void ItemRemoved(T item);

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnCollectionChanged(e);

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					{
						foreach (var item in e.NewItems.OfType<T>())
						{
							if (_snapshot.Contains(item))
								throw new InvalidOperationException("Duplicate item: {0}".With(item.GetType().Name));
							_snapshot.Add(item);
							ItemAdded(item);
						}
					}
					break;
					case NotifyCollectionChangedAction.Remove:
					{
						foreach (var item in e.OldItems.OfType<T>())
						{
							_snapshot.Remove(item);
							ItemRemoved(item);
						}
					}
					break;
					case NotifyCollectionChangedAction.Replace:
					{
						foreach (var item in e.OldItems.OfType<T>())
							ItemRemoved(item);

						foreach (var item in e.NewItems.OfType<T>())
						{
							if (_snapshot.Contains(item))
								throw new InvalidOperationException("Duplicate item: {0}".With(item.GetType().Name));
							_snapshot.Add(item);
							ItemAdded(item);
						}
					}
					break;
					case NotifyCollectionChangedAction.Reset:
					{
						foreach (var item in _snapshot)
							ItemRemoved(item);
						_snapshot.Clear();

						foreach (var item in this)
						{
							if (_snapshot.Contains(item))
								throw new InvalidOperationException("Duplicate item: {0}".With(item.GetType().Name));
							_snapshot.Add(item);
							ItemAdded(item);
						}
					}
					break;

			}
		}

		#endregion

		//======================================================
		#region _Private, protected, internal fields_

		protected List<T> _snapshot;

		#endregion
	}
}