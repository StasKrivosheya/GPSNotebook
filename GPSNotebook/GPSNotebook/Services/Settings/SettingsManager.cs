using Xamarin.Essentials;

namespace GPSNotebook.Services.Settings
{
    public class SettingsManager : ISettingsManager
    {
        #region -- ISettingsManager Implementation --

        public int RememberedUserId
        {
            get => Preferences.Get(nameof(RememberedUserId), default(int));
            set => Preferences.Set(nameof(RememberedUserId), value);
        }

        public string RememberedMail
        {
            get => Preferences.Get(nameof(RememberedMail), default(string));
            set => Preferences.Set(nameof(RememberedMail), value);
        }

        #endregion
    }
}
