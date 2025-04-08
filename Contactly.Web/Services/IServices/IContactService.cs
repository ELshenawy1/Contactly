using Contactly.Web.Models;
using Contactly.Web.Models.DTOs;

namespace Contactly.Web.Services.IServices
{
    public interface IContactService
    {
        Task<T> GetAllAsync<T>(ContactSpecParams specParams);
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(ContactCreateDTO dto);
        Task<T> UpdateAsync<T>(ContactUpdateDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
