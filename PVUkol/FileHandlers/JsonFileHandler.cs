using Newtonsoft.Json;

namespace PVUkol.FileHandlers
{
    public static class JsonFileHandler
    {
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
