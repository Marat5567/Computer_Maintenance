namespace Computer_Maintenance.Globals
{
    public static class Format
    {
        public static string FormatBytes(long bytes)
        {
            double kb = bytes / 1024.0;
            double mb = kb / 1024.0;
            double gb = mb / 1024.0;

            if (gb >= 1) return $"{gb:F2} ГБ";
            if (mb >= 1) return $"{mb:F2} МБ";
            if (kb >= 1) return $"{kb:F2} КБ";
            return $"{bytes} Б";
        }
    }
}
