using System.Xml.Serialization;

namespace PVUkol.FileHandlers
{
    /// <summary>
    /// XML serialization/deserialization class
    /// </summary>
    public static class XmlFileHandler
    {
        /// <summary>
        /// Serializes object of class argument to file at specified location
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void Serialize<T>(string path, object content)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            ArgumentNullException.ThrowIfNull(content, nameof(content));
            try
            {
                XmlSerializer Serializer = new XmlSerializer(typeof(T));
                using (TextWriter writer = new StreamWriter(path))
                {
                    Serializer.Serialize(writer, content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serializing XML to {path}: {ex}");
            }
        }

        /// <summary>
        /// Deserializes a specified class to a object with properties from the specified file in argument
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static T? Deserialize<T>(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            if (!File.Exists(path)) throw new FileNotFoundException(nameof(path));

            try
            {
                XmlSerializer Serializer = new XmlSerializer(typeof(T));
                using (TextReader reader = new StreamReader(path))
                {
                    return (T?)Serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing XML from {path}: {ex}");
            }

            return default;
        }
    }
}
