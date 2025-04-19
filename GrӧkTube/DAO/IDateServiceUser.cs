using GrӧkTube.Entities;

namespace GrӧkTube.DAO
{
    public interface IDateServiceUser
    {
        public void SaveUsers(User user);

        public User GetUser(int id);

        public User FindUser(string login, string password);

        public User GetUserByLogin(string login);
        public bool LoginExists(string login);
    }
}
