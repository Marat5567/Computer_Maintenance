using System.Text.Json;
namespace Computer_Maintenance.Core.Json
{
    public static class JsonService
    {
        private static readonly string _baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"); //Базовый путь где будет храниться все json файлы

        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            WriteIndented = true, // форматированный вывод
        };

        public static void Save<T>(string fileName, T obj)
        {
            if (!Directory.Exists(_baseDirectory))
            {
                Directory.CreateDirectory(_baseDirectory);
            }

            string fullPath = Path.Combine(_baseDirectory, fileName);
            string json = JsonSerializer.Serialize(obj, _options);

            File.WriteAllText(fullPath, json);
        }
        public static T Load<T>(string fileName)
        {
            string fullPath = Path.Combine(_baseDirectory, fileName);

            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                return JsonSerializer.Deserialize<T>(json, _options);
            }
            return default(T);
        }

    }
}
