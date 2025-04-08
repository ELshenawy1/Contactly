namespace Contactly.Web.Services.IServices
{
    public interface ITokenProvider
    {
        void SetToken(string AccessToken);
        string? GetToken();
        void ClearTokne();
    }
}
