using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Bobasoft.Presentation.Actions
{
    public class SetFocusAction : TargetedTriggerAction<Control>
    {
        //======================================================
        #region _Public methods_

        protected override void Invoke(object parameter)
        {
            var target = Target;
            if (target != null)
                target.Focus();
        }

        #endregion
    }
}