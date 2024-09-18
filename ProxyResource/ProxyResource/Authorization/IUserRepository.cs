namespace ProxyResource.Authorization
{
    public interface IUserRepository
    {
        User GetUser(User userModel);
    }
}
