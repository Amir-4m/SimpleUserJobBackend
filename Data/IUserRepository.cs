using DotNetAPI.Models;

namespace DotNetAPI.Data
{
    public interface IUserRepository
    {

        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToAdd);
        public IEnumerable<User> GetUsers();
        public User GetUser(int userId);


    }
}