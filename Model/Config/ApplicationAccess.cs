namespace Computer_Maintenance.Model.Config
{
    public static class ApplicationAccess
    {
        public static Access CurrentAccess { get; set; }
        public enum Access
        {
            User,
            Admin,
        }
    }
}
