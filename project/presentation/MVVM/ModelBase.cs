using System;
using Bobasoft.Collections;

namespace Bobasoft.Presentation.MVVM
{
    public class ModelBase : ObservableObject
    {
        public class DataValue : ObservableObject
        {
            public DataValue(object value)
            {
                _value = value;
            }

            private object _value;
            public object Value
            {
                get { return _value; }
                set
                {
                    if (_value != value)
                    {
                        _value = value;
                        RaisePropertyChanged("Value");
                    }
                }
            }
        }

        //======================================================
        #region _Constructors_
        
        public ModelBase()
        {
            Commands = new ObservableDataValueDictionary<CommandBase>();
            Events = new ObservableDataValueDictionary<CommandBase>();
            Values = new ObservableDataValueDictionary<DataValue>();
        }

        #endregion

        //======================================================
        #region _Public properties_

        public virtual bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                value = (value ? ++_isBusyCount : (_isBusyCount > 0 ? --_isBusyCount : 0)) > 0;
#if WinRT
            	SetProperty(ref _isBusy, value, "IsBusy");
#else
				SmartDispatcher.DispatchAsync(() => SetProperty(ref _isBusy, value, "IsBusy"));
#endif
            }
        }

        public ObservableDataValueDictionary<CommandBase> Commands { get; protected set; }
        public ObservableDataValueDictionary<CommandBase> Events { get; protected set; }
        public ObservableDataValueDictionary<DataValue> Values { get; protected set; }

        #endregion

        //======================================================
        #region _Publi methods_

        public void InitializeInternal()
        {
            if (!_isInitialized)
            {
                Initialize();
                _isInitialized = true;
            }
        }

        #endregion

        //======================================================
        #region _Private, protected, internal methods_

        protected virtual void Initialize()
        {
        }

        // ------------------------------------------------- Commands ------------------------------------------------------- //

        protected virtual CommandBase AddCommand(string name, Action command)
        {
            return AddCommand(name, command, true, true);
        }

        protected virtual CommandBase AddCommand<T>(string name, Action<T> command)
        {
            return AddCommand(name, command, true, true);
        }

        protected virtual CommandBase AddCommand(string name, Action command, bool enable, bool visible)
        {
            var cmd = new RelayCommand(name, command, enable, visible);
            Commands.Add(name, cmd);
            return cmd;
        }

        protected virtual CommandBase AddCommand<T>(string name, Action<T> command, bool enable, bool visible)
        {
            var cmd = new RelayCommand<T>(name, command, enable, visible);
            Commands.Add(name, cmd);
            return cmd;
        }

        /// <summary>
        /// Uses SmartDispatcher.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="enable"></param>
        /// <param name="visible"></param>
        protected virtual void ChangeCommandState(string name, bool? enable = null, bool? visible = null)
        {
            if ((enable.HasValue || visible.HasValue) && Commands.ContainsKey(name))
            {
                var command = Commands[name];
#if WinRT
					if (enable.HasValue)
						command.IsEnable = enable.Value;
					if (visible.HasValue)
						command.IsVisible = visible.Value;
#else
                SmartDispatcher.DispatchAsync(() =>
                                              {
                                                  if (enable.HasValue)
                                                      command.IsEnable = enable.Value;
                                                  if (visible.HasValue)
                                                      command.IsVisible = visible.Value;
                                              });
#endif
            }
        }

        protected virtual void RemoveCommand(string name)
        {
            Commands.Remove(name);
        }

        // --------------------------------------------------- events ------------------------------------------------------- //

        protected virtual CommandBase AddEvent(string name, Action handler)
        {
            var h = new RelayCommand(name, handler, true, true);
            Events.Add(name, h);
            return h;
        }

        protected virtual CommandBase AddEvent<T>(string name, Action<T> handler)
        {
            var h = new RelayCommand<T>(name, handler, true, true);
            Events.Add(name, h);
            return h;
        }

        protected virtual bool RemoveEvent(string name)
        {
            return Events.Remove(name);
        }

        // --------------------------------------------------- values ------------------------------------------------------- //

        protected virtual void NewOrClearValue(string key)
        {
            SetValue(key, (object)null);
        }

        /// <summary>
        /// Uses SmartDispatcher.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
		protected virtual void SetValue<T>(string key, T value)
        {
#if WinRT
        	DataValue v;
        	if (!Values.TryGetValue(key, out v))
        		Values[key] = new DataValue(value);
        	else
        		v.Value = value;
#else
            SmartDispatcher.DispatchAsync(() =>
                            {
                                DataValue v;
                                if (!Values.TryGetValue(key, out v))
                                    Values[key] = new DataValue(value);
                                else
                                    v.Value = value;
                            });
#endif
        }

    	protected virtual T GetValue<T>(string key)
        {
            return (T) Values[key].Value;
        }

        protected virtual DataValue GetValue(string key)
        {
            return Values[key];
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private bool _isBusy;
        private int _isBusyCount;
        private bool _isInitialized;

        #endregion
    }
}