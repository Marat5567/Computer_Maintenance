namespace Computer_Maintenance.Model.Enums.SystemCleaning
{
    public enum TypeCleaning
    {
        // Пользовательские
        UserTemp, // Временные файлы пользователя
        UserRecycleBin, // Корзина пользователя
        UserBrowserCache, // общий тип для кэшей браузеров (fallback)

        // Аггрегированные типы для браузеров (выбор всей секции)
        Browser_All_GoogleChrome,
        Browser_All_Yandex,
        Browser_All_MicrosoftEdge,
        Browser_All_MozillaFirefox,

        // Браузерные кэши — Firefox (детализированные подпункты)
        Firefox_Cache2,
        Firefox_Cache2_Entries,
        Firefox_Cache2_Doomed,
        Firefox_Thumbnails,
        Firefox_JumpListCache,
        Firefox_StartupCache,

        // Браузерные кэши — общие для Chromium-браузеров (Chrome / Edge / Yandex и т.п.)
        BrowserCache_Cache,
        BrowserCache_CodeCache,
        BrowserCache_GPUCache,
        BrowserCache_ServiceWorker_CacheStorage,
        BrowserCache_Sessions,
        BrowserCache_StorageExt,

        // Кэш и временные файлы
        ThumbnailCache, // Кэши эскизов изображений (общий)
        D3DSCache, // Временные файлы DirectX
        WindowsTempFiles, // Временные файлы Windows
        UWPTempFiles, // Хранилище UWP-приложений
        WindowsUpdateCache, // Кэш центра обновления Windows

        // Отчёты и дампы
        CrashDumps, // Дампы ошибок приложений

        // Отчёты об ошибках (локальные)
        WindowsErrorReporting, // Общая секция WER (локальная)
        WindowsErrorReporting_ReportArchive,
        WindowsErrorReporting_ReportQueue,
        WindowsErrorReporting_Temp,

        // Отчёты об ошибках (системные / CommonApplicationData)
        SystemWindowsErrorReporting, // Общая системная секция WER
        SystemWindowsErrorReporting_ReportArchive,
        SystemWindowsErrorReporting_ReportQueue,
        SystemWindowsErrorReporting_Temp,

        // Логи обновлений и системы
        WindowsUpdateLogs, // Логи Windows Update (DataStore и пр.)
        SystemLogs, // Общая секция системных логов
        CBSLogs, // Логи CBS
        DISMLogs, // Логи DISM
        DPXLogs, // Логи DPX

        // Prefetch и ускорители
        Prefetch, // Файлы для ускорения запуска программ

        // Дополнительные (резерв)
        RecentDocuments
    }
}