using System;

namespace GPSNotebook
{
    public abstract class Constants
    {
        public const string DATABASE_NAME = "gps_notebook.db";
        public static string DatabasePath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public const string DEFAULT_IMAGE_PLACEHOLDER = "pic_placeholder.png";

        public const double DEFAULT_CAMERA_ZOOM = 15d;
    }
}
