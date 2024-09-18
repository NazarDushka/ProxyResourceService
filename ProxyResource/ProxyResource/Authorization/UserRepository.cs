namespace ProxyResource.Authorization
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public UserRepository() 
        {
            _users = new List<User>
            {
                new User("Nazar","1234")
            };
        }

        public User GetUser(User userModel) =>
            _users.FirstOrDefault(u =>
                string.Equals(u.UserName, userModel.UserName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(u.Password, userModel.Password)) ??
                throw new Exception("User not found");
    }
}
