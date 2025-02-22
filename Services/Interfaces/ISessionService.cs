namespace CustomerManagerWeb.Services.Interfaces
{
    public interface ISessionService
    {
        string GetToken();
        void SetToken(string token);
        void ClearSession();
    }
}
