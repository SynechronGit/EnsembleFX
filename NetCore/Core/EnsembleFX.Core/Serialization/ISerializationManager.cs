namespace EnsembleFX.Core.Serialization
{
    /// <summary>
    /// Interface that handles Serialization and Deserialization of object instances
    /// </summary>
    public interface ISerializationManager
    {
        /// <summary>
        /// Deserializes the string content to object of type T
        /// </summary>
        /// <typeparam name="T">Generic type T</typeparam>
        /// <param name="content">Content to deserialize</param>
        /// <returns>Deserialized object of generic type T</returns>
        T DeserializeObject<T>(string content);

        /// <summary>
        /// Serializes the object instance of T and returns the serialized object back 
        /// </summary>
        /// <typeparam name="T">Generic type T</typeparam>
        /// <param name="instance">Instance of generic type T to serialize</param>
        /// <returns>Serialized object of generic type T</returns>
        string Serialize<T>(T instance);
    }
}
