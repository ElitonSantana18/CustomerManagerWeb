using CustomerManagerWeb.Services.Interfaces;

namespace CustomerManagerWeb.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetToken()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");
        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext?.Session.SetString("AuthToken", token);
        }

        public void ClearSession()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
        }
    }
}
