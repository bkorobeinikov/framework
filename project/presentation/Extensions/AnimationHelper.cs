using System.Windows;
using System.Windows.Media.Animation;

namespace Bobasoft.Presentation.Extensions
{
    public static class AnimationHelper
    {
        //======================================================
        #region _Public methods_

        public static void BeginAnimation(this FrameworkElement obj, DependencyProperty property, DoubleAnimation animation)
        {
            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(storyboard, obj);
            Storyboard.SetTargetProperty(storyboard, new PropertyPath(property));
            storyboard.Begin();
        }

        #endregion
    }
}