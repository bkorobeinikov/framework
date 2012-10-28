using System.Collections.Generic;
#if METRO
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
#endif


namespace Bobasoft.Presentation.Extensions
{
    public static class ChildrenFinder
    {
        public static FrameworkElement Find(this FrameworkElement root, string name)
        {
            var queue = new Queue<FrameworkElement>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                for (var i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i) as FrameworkElement;
                    if (child != null)
                    {
                        if (child.Name == name)
                            return child;
                        queue.Enqueue(child);
                    }
                }
            }
            return null;
        }

        public static T FindChildOfType<T>(this DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }

        public static T FindVisualChildren<T>(this DependencyObject depObj, object  dataContext) where T : Control
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = VisualTreeHelper.GetChild(depObj, i) as T;
                    if (child != null && child.DataContext == dataContext)
                        return child;
                }
            }

            return null;
        }

        public static IEnumerable<T> FindTopVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = VisualTreeHelper.GetChild(depObj, i) as T;
                    if (child != null)
                        yield return child;
                }
            }
        }

        public static IEnumerable<T> FindAllVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
	            int count = VisualTreeHelper.GetChildrenCount(depObj);
	            for (int i = 0; i < count; i++)
                {
	                var depObjChild = VisualTreeHelper.GetChild(depObj, i);
	                var child = depObjChild as T;
                    if (child != null)
                        yield return child;

					foreach (var childOfChild in FindAllVisualChildren<T>(depObjChild))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

    }
}