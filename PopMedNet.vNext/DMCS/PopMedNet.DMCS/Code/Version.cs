namespace PopMedNet.DMCS.Code
{
    public static class Version
    {
        static Version()
        {
            FileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Version).Assembly.Location).FileVersion;
            ApplicationVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Version).Assembly.Location).ProductVersion;
        }
        public static string FileVersion { get; private set; }
        public static string ApplicationVersion { get; private set; }
    }
}
