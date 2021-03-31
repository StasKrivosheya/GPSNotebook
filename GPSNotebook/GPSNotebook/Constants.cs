using System;

namespace GPSNotebook
{
    public abstract class Constants
    {
        public const string DATABASE_NAME = "gps_notebook.db";
        public static string DatabasePath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }
}
