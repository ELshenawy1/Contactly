using Contactly.Web.Models;
using Contactly.Web.Models.DTOs;
using Contactly.Web.Services.IServices;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Contactly.Web.Services
{
    public class ContactService : BaseService, IContactService
    {
        private string contactUrl;
        public ContactService(IHttpClientFactory clientFactory,IConfiguration configuration, ITokenProvider _tokenProvider) 
            : base(clientFactory, _tokenProvider)
        {
            contactUrl = configuration.GetValue<string>("SerivceUrls:ContactlyAPI");
        }
        public async Task<T> CreateAsync<T>(ContactCreateDTO dto)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = contactUrl + $"/api/ContactApi",
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = contactUrl + $"/api/ContactApi/" + id,
            });
        }

        public async Task<T> GetAllAsync<T>(ContactSpecParams specParams)
        {
            StringBuilder url = new StringBuilder($"{contactUrl}/api/ContactApi?");
            if (!string.IsNullOrEmpty(specParams.Search))
                url.Append($"Search={specParams.Search}");
            if (specParams.PageIndex > 0)
                url.Append($"&PageIndex={specParams.PageIndex}"); 
            if (specParams.PageSize > 0)
                url.Append($"&PageSize={specParams.PageSize}");
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = url.ToString()
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = contactUrl + $"/api/ContactApi/"+id,
            });
        }

        public async Task<T> UpdateAsync<T>(ContactUpdateDTO dto)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = contactUrl + $"/api/ContactApi/" + dto.ID,
            });
        }
    }
}
