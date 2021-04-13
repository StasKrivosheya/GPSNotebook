using System.Threading.Tasks;
using Plugin.Permissions;

namespace GPSNotebook.Services.Permissions
{
    public interface IPermissionsService
    {
        Task<bool> TryGetPermissionAsync<T>() where T: BasePermission, new();
    }
}
