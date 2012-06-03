using System;

namespace Bobasoft.Serialization
{
    /// <summary>
    /// Converter interface (if value is null - throws exception)
    /// </summary>
    public interface IConverter
    {
        //======================================================
        #region _Methods_

        /// <summary>
        /// Serializes <paramref name="value">object</paramref> to string.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>String with serialized object.</returns>
        string Serialize(object value);

        /*/// <summary>
        /// Tries to serialize value to string.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="str">The string with serialized object.</param>
        /// <returns>True if serialized successfully; otherwise false.</returns>
        bool TrySerialize(object value, out string str);*/

        /// <summary>
        /// Deserializes <paramref name="value">string</paramref> to object.
        /// </summary>
        /// <param name="value">The string to deserialize.</param>
        /// <returns>Deserialized object from <paramref name="value">string</paramref>.</returns>
        object Deserialize(string value);

        /// <summary>
        /// Deserializes <paramref name="value">string</paramref> to object.
        /// </summary>
        /// <param name="value">The string to deserialize.</param>
        /// <param name="type">The type of object to deserialize.</param>
        /// <returns>Deserialized object from <paramref name="value">string</paramref>.</returns>
        object Deserialize(string value, Type type);

        //bool TryDeserialize(string )

        /// <summary>
        /// Deserializes <paramref name="value">string</paramref> to object.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="value">The string to deserialize.</param>
        /// <returns>Deserialized object from <paramref name="value">string</paramref>.</returns>
        T Deserialize<T>(string value);

        #endregion
    }
}