using System.Runtime.InteropServices;

namespace Computer_Maintenance.Core.WinApi
{
    public static class CommandLineParser
    {
        [DllImport("shell32.dll", SetLastError = true)]
        private static extern IntPtr CommandLineToArgvW(
              [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine,
              out int pNumArgs);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LocalFree(IntPtr hMem);

        public static (string?, string[]?) Parse(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) { return (null, Array.Empty<string>()); }

            IntPtr argv = CommandLineToArgvW(value, out int argc);
            if (argv == IntPtr.Zero || argc == 0) { return (null, Array.Empty<string>()); }

            try
            {
                string[] args = new string[argc];
                for (int i = 0; i < argc; i++)
                {
                    IntPtr p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p);
                }

                string? exePath = args.Length > 0 ? args[0] : null;
                string[] arguments = args.Length > 1 ? args[1..] : Array.Empty<string>();

                return (exePath, arguments);
            }
            finally
            {
                LocalFree(argv);
            }
        }
    }
}