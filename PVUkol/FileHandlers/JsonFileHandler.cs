using Newtonsoft.Json;

namespace PVUkol.FileHandlers
{
    /// <summary>
    /// JSON serialization/deserialization class
    /// </summary>
    public static class JsonFileHandler
    {
        /// <summary>
        /// Serializes object to file at specified location
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void Serialize(string path, object content)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
            ArgumentNullException.ThrowIfNull(content, nameof(content));
            try
            {
                string json = JsonConvert.SerializeObject(content);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serializing JSON to {path}: {ex}");
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
                return JsonConvert.DeserializeObject<T>(yamlContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing JSON from {path}: {ex}");
            }

            return default;
        }
    }
}
