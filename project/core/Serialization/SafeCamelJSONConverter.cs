using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bobasoft.Serialization
{
    public class SafeCamelJSONConverter : IConverter
    {
        //======================================================
        #region _Public methods_

        public string Serialize(object value)
        {
            return value == null
                       ? null
                       : JsonConvert.SerializeObject(
                           value,
                           Formatting.Indented,
                           new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});
        }

        public object Deserialize(string value)
        {
            return string.IsNullOrEmpty(value)
                       ? null
                       : JsonConvert.DeserializeObject(
                           value, new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});
        }

        public object Deserialize(string value, Type type)
        {
            return string.IsNullOrEmpty(value)
                       ? null
                       : JsonConvert.DeserializeObject(
                           value, type, new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});
        }

        public T Deserialize<T>(string value)
        {
            return string.IsNullOrEmpty(value)
                       ? default(T)
                       : JsonConvert.DeserializeObject<T>(
                           value, new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});
        }

        #endregion
    }
}