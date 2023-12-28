using System.Xml.Serialization;

namespace PVUkol.FileHandlers
{
    public static class XmlFileHandler
    {
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
