using System;
using System.Collections;
using System.Collections.Generic;

namespace Bobasoft.Collections
{
    /// <summary>
    /// Key - string
    /// Value - object
    /// </summary>
    public class DataValueDictionary : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
    {
        //======================================================
        #region _Constructors_

        public DataValueDictionary()
        {
            _dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        public DataValueDictionary(IDictionary<string, object> dictionary)
        {
            _dictionary = new Dictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase);
        }

        public DataValueDictionary(object values)
        {
            _dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
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
            get { return ((IDictionary<string, object>)_dictionary).IsReadOnly; }
        }

        public ICollection<string> Keys
        {
            get { return _dictionary.Keys; }
        }

        public ICollection<object> Values
        {
            get { return _dictionary.Values; }
        }

        public object this[string key]
        {
            get
            {
                object obj;
                TryGetValue(key, out obj);
                return obj;
            }
            set { _dictionary[key] = value; }
        }

        #endregion

        //======================================================
        #region _Public methods_
        
        public void Add(string key, object value)
        {
            _dictionary.Add(key, value);
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            ((ICollection<KeyValuePair<string, object>>)_dictionary).Add(item);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return ((ICollection<KeyValuePair<string, object>>)_dictionary).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool ContainsValue(object value)
        {
            return _dictionary.ContainsValue(value);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, object>>)_dictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            return ((ICollection<KeyValuePair<string, object>>) _dictionary).Remove(item);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public Dictionary<string, object>.Enumerator GetEnumerator()
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
                foreach (var property in values.GetType().GetProperties())
                {
                    var obj = property.GetValue(values, null);
                    Add(property.Name, obj);
                }
            }
        }

        #endregion

        //======================================================
        #region _Private, protected, internal fields_

        private readonly Dictionary<string, object> _dictionary;

        #endregion
    }
}