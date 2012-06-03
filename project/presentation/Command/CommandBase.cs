using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace Bobasoft.Presentation
{
    public abstract class CommandBase : ICommand, INotifyPropertyChanged
    {
        //======================================================
        #region _Constructors_

        protected CommandBase(string name, bool isEnable = true)
        {
            Name = name;
            IsEnable = isEnable;
            IsVisible = true;
        }

        #endregion

        //======================================================
        #region _Public properties_

        /// <summary>
        /// Gets command name.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets command text.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    RaisePropertyChanged("Text");
                }
            }
        }

        /// <summary>
        /// Gets or sets enable parameter of command.
        /// </summary>
        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                if (_isEnable != value)
                {
                    _isEnable = value;
                    RaisePropertyChanged("IsEnable");
                    RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    RaisePropertyChanged("IsVisible");
                }
            }
        }

        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        //======================================================
        #region _Public methods_

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// Defines the method to be called when the command is invoked. 
        /// </summary>
        public abstract void Execute(object parameter);

        #endregion

        //======================================================
        #region _Private, protected, internal methods_

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged" /> event.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "The this keyword is used in the Silverlight version")]
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        protected void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        protected bool _isEnable;
        protected bool _isVisible;
        protected string _text;

        #endregion
    }
}