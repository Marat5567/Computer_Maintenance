namespace Computer_Maintenance.Jsons
{
    public static class BaseDirectoryPath
    {
        public static readonly string BaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"); //Базовый путь где будет храниться все json файлы
    }
}
