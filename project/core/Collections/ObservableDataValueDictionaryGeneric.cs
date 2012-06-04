using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
#if WinRT
using System.Reflection;
#endif

namespace Bobasoft.Collections
{
    public class ObservableDataValueDictionary<TValue> : IDictionary<string, TValue>, ICollection<KeyValuePair<string, TValue>>, IEnumerable<KeyValuePair<string, TValue>>, IEnumerable,
        INotifyCollectionChanged, INotifyPropertyChanged
    {
        //======================================================
        #region _Constructors_

        public ObservableDataValueDictionary()
        {
            _dictionary = new Dictionary<string, TValue>(StringComparer.OrdinalIgnoreCase);
            _keys = new Collection<string>();
        }

        public ObservableDataValueDictionary(IDictionary<string, TValue> dictionary)
        {
            _dictionary = new Dictionary<string, TValue>(dictionary, StringComparer.OrdinalIgnoreCase);
        }

        public ObservableDataValueDictionary(object values)
        {
            _dictionary = new Dictionary<string, TValue>(StringComparer.OrdinalIgnoreCase);
            AddValues(values);
        }

        #endregion

        //======================================================
        #region _Public properties_

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((IDictionary<string, TValue>)_dictionary).IsReadOnly; }
        }

        public ICollection<string> Keys
        {
            get { return _dictionary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return _dictionary.Values; }
        }

        public TValue this[string key]
        {
            get
            {
                TValue obj;
                TryGetValue(key, out obj);
                return obj;
            }
            set
            {
                _dictionary[key] = value;
                var index = AddKeyForIndex(key);
                RaiseAddNotification(key, value, index);
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        //======================================================
        #region _Public methods_
        
        public void Add(string key, TValue value)
        {
            _dictionary.Add(key, value);
            RaiseAddNotification(key, value, DefaultIndex);
        }

        void ICollection<KeyValuePair<string, TValue>>.Add(KeyValuePair<string, TValue> item)
        {
            ((ICollection<KeyValuePair<string, TValue>>)_dictionary).Add(item);
            RaiseAddNotification(item.Key, item.Value, DefaultIndex);
        }

        public void Clear()
        {
            _dictionary.Clear();
            RaiseResetNotification();
        }

        bool ICollection<KeyValuePair<string, TValue>>.Contains(KeyValuePair<string, TValue> item)
        {
            return ((ICollection<KeyValuePair<string, TValue>>)_dictionary).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return _dictionary.ContainsValue(value);
        }

        void ICollection<KeyValuePair<string, TValue>>.CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, TValue>>)_dictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(string key)
        {
            TValue value;
            if (_dictionary.TryGetValue(key, out value) && _dictionary.Remove(key))
            {
                var index = RemoveKeyForIndex(key);
                RaiseRemoveNotification(key, value, index);
                return true;
            }

            return false;
        }

        bool ICollection<KeyValuePair<string, TValue>>.Remove(KeyValuePair<string, TValue> item)
        {
            var dictionary = ((ICollection<KeyValuePair<string, TValue>>) _dictionary);

            TValue value;
            var key = item.Key;
            if (_dictionary.TryGetValue(key, out value) && dictionary.Remove(item))
            {
                var index = RemoveKeyForIndex(key);
                RaiseRemoveNotification(key, value, index);
                return true;
            }

            return false;
        }

        public bool TryGetValue(string key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, TValue>> IEnumerable<KeyValuePair<string, TValue>>.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public Dictionary<string, TValue>.Enumerator GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        #endregion

        //======================================================
        #region _Private, protected, internal methods_

        private void AddValues(object values)
        {
            if (values != null)
            {
#if WinRT
				foreach (var property in values.GetType().GetRuntimeProperties())
#else
                foreach (var property in values.GetType().GetProperties())
#endif
                {
                    var obj = property.GetValue(values, null);
                    Add(property.Name, (TValue)obj);
                }
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            var handler = CollectionChanged;
            if (handler != null)
                handler(this, args);
        }

        // ------------------------ firing events

        private void RaiseAddNotification(string key, TValue value, int index)
        {
            // fire the relevant PropertyChanged notifications
            RaisePropertyChangedNotifications();

            // fire CollectionChanged notification
            var item = new KeyValuePair<string, TValue>(key, value);
            RaiseCollectionChanged(index == DefaultIndex
                                       ? new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count-1)
                                       : new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, index));
        }

        private void RaiseRemoveNotification(string key, TValue value, int index)
        {
            // fire the relevant PropertyChanged notifications
            RaisePropertyChangedNotifications();

            // fire CollectionChanged notification
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<string, TValue>(key, value), index));
        }

        private void RaisePropertyChangedNotifications()
        {
            if (Count != _countCache)
            {
                _countCache = Count;
                RaisePropertyChanged("Count");
                RaisePropertyChanged("Item[]");
                RaisePropertyChanged("Keys");
                RaisePropertyChanged("Values");
            }
        }

        private void RaiseResetNotification()
        {
            // fire the relevant PropertyChanged notifications
            RaisePropertyChangedNotifications();

            // fire CollectionChanged notification
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        // ------------------------------------- indexing

        private int AddKeyForIndex(string key)
        {
#if WinRT
			var newkey = key.ToLower();
#else
            var newkey = key.ToLower(CultureInfo.InvariantCulture);
#endif
            if (!_keys.Contains(newkey))
            {
                _keys.Add(newkey);
                return DefaultIndex;
            }
            
            return _keys.IndexOf(newkey);
        }

        private int RemoveKeyForIndex(string key)
        {
#if WinRT
			var newkey = key.ToLower();
#else
            var newkey = key.ToLower(CultureInfo.InvariantCulture);
#endif
            if (_keys.Contains(newkey))
            {
                var index = _keys.IndexOf(newkey);
                _keys.Remove(newkey);
                return index;
            }

            return DefaultIndex;
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private readonly Dictionary<string, TValue> _dictionary;
        private readonly Collection<string> _keys;
        private int _countCache;

        internal const int DefaultIndex = -1;

        #endregion
    }
}