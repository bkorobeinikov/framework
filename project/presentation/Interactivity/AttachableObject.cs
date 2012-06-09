using System;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Bobasoft.Presentation.Interactivity
{
	public abstract class AttachableObject : FrameworkElement, IAttachedObject
	{
		//======================================================
		#region _Constructors_

		protected AttachableObject()
		{
		}

		#endregion

		//======================================================
		#region _Public properties_

		public DependencyObject AssociatedObject { get; protected set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is attached
		/// </summary>
		public bool IsHosted { get; set; }

		#endregion

		//======================================================
		#region _Public methods_
		
		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
				throw new ArgumentNullException("dependencyObject");

			if (dependencyObject == AssociatedObject)
				return;

			if (AssociatedObject != null)
				throw new InvalidOperationException("Cannot host trigger action multiple items");

			VerifyAttachObject(dependencyObject);
			
			AssociatedObject = dependencyObject;

			var a = AssociatedObject as FrameworkElement;
			if (a != null)
			{
				if (a.DataContext == null)
					a.Loaded += OnLoaded;
				else
					UpdateDataContextBinding();
			}

			OnAttached();
		}

		public void Detach()
		{
			OnDetaching();

			var a = AssociatedObject as FrameworkElement;
			if (a != null)
				a.Loaded -= OnLoaded;

			AssociatedObject = null;
		}

		#endregion

		//======================================================
		#region _Private, protected, internal methods_

		protected virtual void OnAttached()
		{
		}

		protected virtual void OnDetaching()
		{
		}

		protected virtual void VerifyAttachObject(DependencyObject attachObject)
		{
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			_isLoaded = true;
			if (AssociatedObject != null)
				UpdateDataContextBinding();
		}

		protected void UpdateDataContextBinding()
		{
			if (AssociatedObject != null)
				SetBinding(DataContextProperty, new Binding {Path = new PropertyPath("DataContext"), Source = AssociatedObject});
		}

		#endregion

		//======================================================
		#region _Private, protected, internal fields_

		private bool _isLoaded;		

		#endregion
	}

	public abstract class ConstainedAttachableObject : AttachableObject
	{
		protected ConstainedAttachableObject(Type associatedObjectTypeConstaint)
		{
			AssociatedObjectTypeConstraint = associatedObjectTypeConstaint;
		}

		//======================================================
		#region _Public properties_

		public Type AssociatedObjectTypeConstraint { get; protected set; }

		#endregion

		//======================================================
		#region _Private, protected, internal methods_

		protected override void VerifyAttachObject(DependencyObject attachObject)
		{
			if (AssociatedObjectTypeConstraint == null)
				return;

			if (!AssociatedObjectTypeConstraint.GetTypeInfo().IsAssignableFrom(attachObject.GetType().GetTypeInfo()))
				throw new InvalidOperationException("attach object should be of type '{0}'".With(AssociatedObjectTypeConstraint.Name));
		}

		#endregion
	}
}