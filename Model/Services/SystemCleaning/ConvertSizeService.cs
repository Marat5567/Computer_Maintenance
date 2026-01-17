using Computer_Maintenance.Model.Structs.SystemCleaning;

namespace Computer_Maintenance.Model.Services.SystemCleaning
{
    public static class ConvertSizeService
    {
        public static StorageSize ConvertSize(ulong size)
        {
            StorageSize storageSize = new StorageSize();
            ulong remainingBytes = size;

            // TB (1 TB = 1024 GB = 1024 * 1024 * 1024 * 1024 байт)
            storageSize.TB = (uint)(remainingBytes / (1024UL * 1024 * 1024 * 1024));
            remainingBytes %= (1024UL * 1024 * 1024 * 1024);

            // GB (1 GB = 1024 * 1024 * 1024 байт)
            storageSize.GB = (uint)(remainingBytes / (1024UL * 1024 * 1024));
            remainingBytes %= (1024UL * 1024 * 1024);

            // MB (1 MB = 1024 * 1024 байт)
            storageSize.MB = (uint)(remainingBytes / (1024UL * 1024));
            remainingBytes %= (1024UL * 1024);

            // KB (1 KB = 1024 байта)
            storageSize.KB = (uint)(remainingBytes / 1024UL);

            // Оставшиеся байты
            storageSize.Byte = (uint)(remainingBytes % 1024UL);

            return storageSize;
        }
    }
}
