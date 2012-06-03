using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Bobasoft.Presentation.Actions
{
    public class SelectAllAction : TriggerAction<Control>
    {
        //======================================================
        #region _Private, protected, internal methods_

        protected override void Invoke(object parameter)
        {
            if (AssociatedObject != null)
            {
                var o = AssociatedObject as TextBox;
                if (o != null)
                    o.SelectAll();
                else
                {
                    var o1 = AssociatedObject as PasswordBox;
                    if (o1 != null)
                        o1.SelectAll();     
                }
            }
        }

        #endregion
    }
}