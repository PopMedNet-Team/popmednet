namespace PopMedNet.Dns.Portal.Code
{
    public static class Version
    {
        static Version()
        {
            FileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Version).Assembly.Location).FileVersion ?? string.Empty;
            ApplicationVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Version).Assembly.Location).ProductVersion ?? string.Empty;
        }
        /// <summary>
        /// Gets the file version of the website.
        /// </summary>
        public static string FileVersion { get; private set; }
        /// <summary>
        /// Gets the application version of the website.
        /// </summary>
        public static string ApplicationVersion { get; private set; }
    }
}
