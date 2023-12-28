using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PVUkol.FileHandlers
{
    /// <summary>
    /// YAML serialization/deserialization class
    /// </summary>
    public static class YamlFileHandler
    {
        public static readonly ISerializer Serializer = new SerializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).DisableAliases().Build();

        public static readonly IDeserializer Deserializer = new DeserializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();

        /// <summary>
        /// Serializes object of class argument to file at specified location
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void Serialize(string path, object content)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            ArgumentNullException.ThrowIfNull(content, nameof(content));
            try
            {
                string yaml = Serializer.Serialize(content);
                File.WriteAllText(path, yaml);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serializing YAML to {path}: {ex}");
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
                string yamlContent = File.ReadAllText(path);
                return Deserializer.Deserialize<T>(yamlContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing YAML from {path}: {ex}");
            }

            return default;
        }
    }
}