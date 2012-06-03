using System;
using System.Diagnostics;

namespace Bobasoft.Presentation
{
    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other
    /// objects by invoking delegates. The default return value for the CanExecute
    /// method is 'true'.  This class does not allow you to accept command parameters in the
    /// Execute and CanExecute callback methods.
    /// </summary>
    public class RelayCommand : CommandBase
    {
        //======================================================
        #region _Constructors_

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="execute">The execution logic.</param>
        /// <param name="isEnable">The enabled command status.</param>
        /// <param name="isVisible">The visible command status.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <exception cref="ArgumentNullException">If the execute argument is null.</exception>
        public RelayCommand(string name, Action execute, bool isEnable, bool isVisible, Func<bool> canExecute = null)
            : base(name, isEnable)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _isVisible = isVisible;
            _canExecute = canExecute;
        }

        #endregion

        //======================================================
        #region _Public methods_

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        [DebuggerStepThrough]
        public override bool CanExecute(object parameter)
        {
            return IsEnable && (_canExecute == null || _canExecute());
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked. 
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        public override void Execute(object parameter)
        {
            if (IsEnable)
                _execute();
            else
                throw new InvalidOperationException("Cannot execute command in disable state.");
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        protected readonly Action _execute;
        protected readonly Func<bool> _canExecute;

        #endregion
    }
}