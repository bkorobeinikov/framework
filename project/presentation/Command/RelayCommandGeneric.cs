using System;

namespace Bobasoft.Presentation
{
    /// <summary>
    /// A generic command whose sole purpose is to relay its functionality to other
    /// objects by invoking delegates. The default return value for the CanExecute
    /// method is 'true'. This class allows you to accept command parameters in the
    /// Execute and CanExecute callback methods.
    /// </summary>
    /// <typeparam name="T">The type of the command parameter.</typeparam>
    public class RelayCommand<T> : CommandBase
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
        public RelayCommand(string name, Action<T> execute, bool isEnable, bool isVisible, Predicate<T> canExecute = null)
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
        /// <param name="parameter">Data used by the command. If the command does not require data 
        /// to be passed, this object can be set to a null reference</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public override bool CanExecute(object parameter)
        {
            return IsEnable && (_canExecute == null || _canExecute((T) parameter));
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked. 
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data 
        /// to be passed, this object can be set to a null reference</param>
        public override void Execute(object parameter)
        {
            if (IsEnable)
                _execute((T)parameter);
            else
                throw new InvalidOperationException("Cannot execute command in disable state.");
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        #endregion
    }
}