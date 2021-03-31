namespace GPSNotebook.Services.Authorization
{
    public interface IAuthorizationService
    {
        bool IsAuthorized { get; }

        int GetCurrentUserId { get; }

        void Authorize(int id, string mail);

        void UnAuthorized();
    }
}
