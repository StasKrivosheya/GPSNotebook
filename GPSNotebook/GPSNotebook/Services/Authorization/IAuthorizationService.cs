namespace GPSNotebook.Services.Authorization
{
    interface IAuthorizationService
    {
        bool IsAuthorized { get; }

        int GetCurrentUserId { get; }

        void Authorize(int id, string mail);

        void UnAuthorized();
    }
}
