using System;
using System.Collections;
using System.Collections.Generic;
#if WinRT
using System.Reflection;
#endif

namespace Bobasoft.Collections
{
    public class DataValueDictionary<T> : IDictionary<string, T>, ICollection<KeyValuePair<string, T>>, IEnumerable<KeyValuePair<string, T>>, IEnumerable
    {
        //======================================================
        #region _Constructors_

        public DataValueDictionary()
        {
            _dictionary = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        }

        public DataValueDictionary(IDictionary<string, T> dictionary)
        {
            _dictionary = new Dictionary<string, T>(dictionary, StringComparer.OrdinalIgnoreCase);
        }

        public DataValueDictionary(object values)
        {
            _dictionary = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
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
            get { return ((IDictionary<string, T>)_dictionary).IsReadOnly; }
        }

        public ICollection<string> Keys
        {
            get { return _dictionary.Keys; }
        }

        public ICollection<T> Values
        {
            get { return _dictionary.Values; }
        }

        public T this[string key]
        {
            get
            {
                T obj;
                TryGetValue(key, out obj);
                return obj;
            }
            set { _dictionary[key] = value; }
        }

        #endregion

        //======================================================
        #region _Public methods_
        
        public void Add(string key, T value)
        {
            _dictionary.Add(key, value);
        }

        void ICollection<KeyValuePair<string, T>>.Add(KeyValuePair<string, T> item)
        {
            ((ICollection<KeyValuePair<string, T>>)_dictionary).Add(item);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        bool ICollection<KeyValuePair<string, T>>.Contains(KeyValuePair<string, T> item)
        {
            return ((ICollection<KeyValuePair<string, T>>)_dictionary).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool ContainsValue(T value)
        {
            return _dictionary.ContainsValue(value);
        }

        void ICollection<KeyValuePair<string, T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, T>>)_dictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }

        bool ICollection<KeyValuePair<string, T>>.Remove(KeyValuePair<string, T> item)
        {
            return ((ICollection<KeyValuePair<string, T>>) _dictionary).Remove(item);
        }

        public bool TryGetValue(string key, out T value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, T>> IEnumerable<KeyValuePair<string, T>>.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public Dictionary<string, T>.Enumerator GetEnumerator()
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
                    Add(property.Name, (T)obj);
                }
            }
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private readonly Dictionary<string, T> _dictionary;

        #endregion
    }
}