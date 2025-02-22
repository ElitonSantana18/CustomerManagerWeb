using CustomerManagerWeb.Models;

namespace CustomerManagerWeb.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginViewModel request);
    }
}
