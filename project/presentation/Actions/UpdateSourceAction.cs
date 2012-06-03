using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Bobasoft.Presentation.Actions
{
    public class UpdateSourceAction : TriggerAction<Control>
    {
        //======================================================
        #region _Private, protected, internal methods_

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter">The parameter to the action. If the action does not require a parameter, the parameter may be set to a null reference.</param>
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject != null)
            {
                BindingExpression e = null;

                var o = AssociatedObject as TextBox;
                if (o != null)
                    e = o.GetBindingExpression(TextBox.TextProperty);
                else
                {
                    var o1 = AssociatedObject as PasswordBox;
                    if (o1 != null)
                        e = o1.GetBindingExpression(PasswordBox.PasswordProperty);
                }

                if (e != null)
                    e.UpdateSource();
            }
        }

        #endregion
    }
}