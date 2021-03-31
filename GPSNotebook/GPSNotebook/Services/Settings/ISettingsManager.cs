namespace GPSNotebook.Services.Settings
{
    public interface ISettingsManager
    {
        int RememberedUserId { get; set; }

        string RememberedMail { get; set; }
    }
}
