using System;
using Newtonsoft.Json;

namespace Bobasoft.Serialization
{
    public class SafeJSONConverter : IConverter
    {
        //======================================================
        #region _Public methods_

        public string Serialize(object value)
        {
            return value == null ? null : JsonConvert.SerializeObject(value);
        }

        public object Deserialize(string value)
        {
            return string.IsNullOrEmpty(value) ? null : JsonConvert.DeserializeObject(value);
        }

        public object Deserialize(string value, Type type)
        {
            return string.IsNullOrEmpty(value) ? null : JsonConvert.DeserializeObject(value, type);
        }

        public T Deserialize<T>(string value)
        {
            return string.IsNullOrEmpty(value) ? default(T): JsonConvert.DeserializeObject<T>(value);
        }

        #endregion
    }
}