namespace Computer_Maintenance.Model.Structs.SystemCleaning
{
    public class StorageSize
    {
        public uint TB { get; set; }
        public uint GB { get; set; }
        public uint MB { get; set; }
        public uint KB { get; set; }
        public uint Byte { get; set; }

        public enum SizeType { Byte, KB, MB, GB, TB }

        public void AddSize(StorageSize newSize)
        {
            TB += newSize.TB;
            GB += newSize.GB;
            MB += newSize.MB;
            KB += newSize.KB;
            Byte += newSize.Byte;
        }

        public Color GetColorBySize()
        {
            if (TB >= 1)
            {
                return Color.Red;
            }

            if (GB >= 1)
            {
                return Color.Red;
            }

            if (MB >= 500)
            {
                return Color.OrangeRed;
            }

            if (MB >= 100)
            {
                return Color.Orange;
            }

            if (MB >= 1)
            {
                return Color.Green;
            }

            if (KB >= 1)
            {
                if (KB <= 999)
                {
                    return Color.LightGreen;
                }
            }

            if (Byte > 0)
            {
                return Color.YellowGreen;
            }

            return Color.Gray;
        }

        public SizeType GetMaxSizeType()
        {
            if (TB != 0)
            {
                return SizeType.TB;
            }
            if (GB != 0)
            {
                return SizeType.GB;
            }
            if (MB != 0)
            {
                return SizeType.MB;
            }
            if (KB != 0)
            {
                return SizeType.KB;
            }
            if (Byte != 0)
            {
                return SizeType.Byte;
            }
            return 0;
        }
        public string GetSizeByType(SizeType type)
        {
            switch (type)
            {
                case SizeType.TB:
                    double totalTB = TB + (GB / 1024.0) + (MB / (1024.0 * 1024.0)) + (KB / (1024.0 * 1024.0 * 1024.0)) + (Byte / (1024.0 * 1024.0 * 1024.0 * 1024.0));
                    return $"{totalTB:F2} ТБ";

                case SizeType.GB:
                    double totalGB = GB + (MB / 1024.0) + (KB / (1024.0 * 1024.0)) + (Byte / (1024.0 * 1024.0 * 1024.0));
                    return $"{totalGB:F2} ГБ";

                case SizeType.MB:
                    double totalMB = MB + (KB / 1024.0) + (Byte / (1024.0 * 1024.0));
                    return $"{totalMB:F2} МБ";

                case SizeType.KB:
                    double totalKB = KB + (Byte / 1024.0);
                    return $"{totalKB:F2} КБ";

                case SizeType.Byte:
                    return $"{Byte} Б";
            }

            return "0 Б";
        }
    }
}
