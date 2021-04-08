﻿using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using GPSNotebook.Resources;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace GPSNotebook.Services.Permissions
{
    public class PermissionsService : IPermissionsService
    {
        #region -- IPermissionsService Implementation --

        public async Task<bool> TryGetPermissionAsync<T>() where T : BasePermission, new()
        {
            var result = false;

            var currentStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<T>();

            if (currentStatus != PermissionStatus.Granted)
            {
                var newStatus = await CrossPermissions.Current.RequestPermissionAsync<T>();

                if (newStatus != PermissionStatus.Granted)
                {
                    ConfirmConfig config = new ConfirmConfig
                    {
                        Message = $"{Resource.PermissionNotGranted}: {typeof(T).Name}.\n{Resource.SuggestVisitingSettings}",
                        CancelText = Resource.Cancel,
                        OkText = Resource.OpenSettings
                    };

                    var shouldOpenSettings = await UserDialogs.Instance.ConfirmAsync(config);

                    if (shouldOpenSettings)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        #endregion
    }
}