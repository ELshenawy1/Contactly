using Contactly.Web.Models.DTOs;

namespace Contactly.Web.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
    }
}
