using Contactly.Web.Models;
using Contactly.Web.Services.IServices;

namespace Contactly.Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public void ClearTokne()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.AccessToken);
        }

        public string GetToken()
        {
            try
            {
                bool hasAccessToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.AccessToken, out string accessToken);
                return (hasAccessToken) ? accessToken : null;
            }
            catch
            {
                return null;
            }
        }

        public void SetToken(string AccessToken)
        {
            var cookieOptions = new CookieOptions { Expires = DateTime.UtcNow.AddMinutes(59) };
            _contextAccessor.HttpContext?.Response.Cookies.Append(SD.AccessToken, AccessToken, cookieOptions);
        }
    }
}
