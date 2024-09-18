namespace ProxyResource.Authorization
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, User user);
    }
}
