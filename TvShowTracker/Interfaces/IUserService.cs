namespace TvShowTracker.Interfaces
{
    public interface IUserService
    {
        void RegisterConsent(int userId);
        bool DeleteUserData(int userId);
    }
}
