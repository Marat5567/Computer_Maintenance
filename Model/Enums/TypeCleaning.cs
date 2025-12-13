namespace Computer_Maintenance.Model.Enums
{
    public enum TypeCleaning
    {
        UserTemp, //Временные файлы пользователя
        UserRecycleBin, //Корзина пользователя
        UserBrowserCache, //Кэши браузеров пользователя
        ThumbnailCache, //Кэши эскизов изображений
        RecentDocuments, //Недавние документы
        CrashDumps, //Дампы ошибок приложений
        D3DSCache, //Временные файлы DirectX
        WindowsErrorReporting, //Отчеты об ошибках Windows
        UWPTempFiles, //хранилище UWP-приложений
        WindowsUpdateCache, //Кэш центра обновления Windows
        WindowsUpdateLogs, //Логи центра обновления Windows
    }
}
