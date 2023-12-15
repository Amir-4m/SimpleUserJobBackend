using DotNetAPI.Models;

namespace DotNetAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private DataContextEf _entiryFramework;
        public UserRepository(IConfiguration config)
        {
            _entiryFramework = new DataContextEf(config);
        }

        public bool SaveChanges()
        {
            return _entiryFramework.SaveChanges() > 0;
        }
        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entiryFramework.Add(entityToAdd);

            }
        }
        public void RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _entiryFramework.Remove(entityToRemove);

            }
        }
        public IEnumerable<User> GetUsers()
        {
            return _entiryFramework.Users.ToList<User>();
        }
        public User GetUser(int userId)
        {
            User? user = _entiryFramework.Users
                .FirstOrDefault(obj => obj.UserId == userId);
            if (user != null)
            {
                return user;
            }

            throw new Exception("Failed to get user");
        }

    }
}