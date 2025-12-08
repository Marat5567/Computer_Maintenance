using Computer_Maintenance.Model.Config;

namespace Computer_Maintenance.Model.Structs
{
    public struct StorageSize
    {
        public uint TB { get; set; }
        public uint GB { get; set; }
        public uint MB { get; set; }
        public uint KB { get; set; }
        public uint Byte { get; set; }

        public enum SizeType { Byte, KB, MB, GB, TB }

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
        public string GetSizeByType(SizeType type, byte n1 = 0, byte n2 = 0, byte n3 = 0)
        {
            switch (type)
            {
                case SizeType.TB:
                    if (n1 != 0)
                    {
                        if (GB.ToString().Length >= 1)
                        {
                            return $"{TB}, {GB.ToString()[0]} ТБ";
                        }
                    }
                    if (n2 != 0)
                    {
                        if (GB.ToString().Length >= 2)
                        {
                            return $"{TB}, {GB.ToString()[1]} ТБ";
                        }
                    }
                    if (n3 != 0)
                    {
                        if (GB.ToString().Length >= 3)
                        {
                            return $"{TB}, {GB.ToString()[2]} ТБ";
                        }
                    }
                    break;
                case SizeType.GB:
                    if (n1 != 0)
                    {
                        if (MB.ToString().Length >= 1)
                        {
                            return $"{GB}, {MB.ToString()[0]} ГБ";
                        }
                    }
                    if (n2 != 0)
                    {
                        if (MB.ToString().Length >= 2)
                        {
                            return $"{GB}, {MB.ToString()[1]} ГБ";
                        }
                    }
                    if (n3 != 0)
                    {
                        if (MB.ToString().Length >= 3)
                        {
                            return $"{GB}, {MB.ToString()[2]} ГБ";
                        }
                    }
                    break;
                case SizeType.MB:
                    if (n1 != 0)
                    {
                        if (KB.ToString().Length >= 1)
                        {
                            return $"{MB}, {KB.ToString()[0]} МБ";
                        }
                    }
                    if (n2 != 0)
                    {
                        if (KB.ToString().Length >= 2)
                        {
                            return $"{MB}, {KB.ToString()[1]} МБ";
                        }
                    }
                    if (n3 != 0)
                    {
                        if (KB.ToString().Length >= 3)
                        {
                            return $"{MB}, {KB.ToString()[2]} МБ";
                        }
                    }
                    break;
                case SizeType.KB:
                    if (n1 != 0)
                    {
                        if (Byte.ToString().Length >= 1)
                        {
                            return $"{KB}, {Byte.ToString()[0]} КБ";
                        }
                    }
                    if (n2 != 0)
                    {
                        if (Byte.ToString().Length >= 2)
                        {
                            return $"{KB}, {Byte.ToString()[1]} КБ";
                        }
                    }
                    if (n3 != 0)
                    {
                        if (Byte.ToString().Length >= 3)
                        {
                            return $"{KB}, {Byte.ToString()[2]} КБ";
                        }
                    }
                    break;
            }
            return $"{Byte} Б";

        }
    }
}
