namespace GPSNotebook.Services.Settings
{
    interface ISettingsManager
    {
        int RememberedUserId { get; set; }

        string RememberedMail { get; set; }
    }
}
