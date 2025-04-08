using Contactly.Web.Models;

namespace Contactly.Web.Services.IServices
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest apiRequest,bool withBearer);
    }
}
