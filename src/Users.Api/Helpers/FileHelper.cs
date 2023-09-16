using Newtonsoft.Json;

namespace Users.Api.Helpers
{
    public static class FileHelper
    {
        public static List<T>? GetListFromJsonFile<T>(string filePath)
        {
            if (!File.Exists(filePath) || !filePath.EndsWith(".json"))
            {
                return null;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                var json = reader.ReadToEnd();

                try
                {
                    return JsonConvert.DeserializeObject<List<T>>(json);
                }
                catch (Exception)
                {
                    return null;
                }

            }
        }
    }
}
