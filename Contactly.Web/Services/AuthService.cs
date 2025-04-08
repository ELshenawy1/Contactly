using Contactly.Web.Models;
using Contactly.Web.Models.DTOs;
using Contactly.Web.Services.IServices;

namespace Contactly.Web.Services
{
    public class AuthService : BaseService,IAuthService
    {
        private string contactUrl;

        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration, ITokenProvider _tokenProvider)
             : base(clientFactory, _tokenProvider)
        {
            contactUrl = configuration.GetValue<string>("SerivceUrls:ContactlyAPI");
        }

        public async Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = contactUrl + $"/api/AuthApi/Login"
            });
        }
    }
}
