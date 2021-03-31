﻿using GPSNotebook.Services.Settings;

namespace GPSNotebook.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ISettingsManager _settingsManager;

        public AuthorizationService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        #region -- IAuthorizationService Implementation --

        public bool IsAuthorized => _settingsManager.RememberedMail != default;

        public int GetCurrentUserId => IsAuthorized ? _settingsManager.RememberedUserId : -1;

        public void Authorize(int id, string mail)
        {
            _settingsManager.RememberedUserId = id;
            _settingsManager.RememberedMail = mail;
        }

        public void UnAuthorize()
        {
            _settingsManager.RememberedUserId = -1;
            _settingsManager.RememberedMail = default;
        }

        #endregion
    }
}
