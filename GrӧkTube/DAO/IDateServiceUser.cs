using GrӧkTube.Entities;

namespace GrӧkTube.DAO
{
    public interface IDateServiceUser
    {
        public void SaveUsers(User user);

        public User GetUser(int id);

        public User FindUser(string login, string password);

        public bool LoginExists(string login);

        public User GetUserByLoginAndId(string login, int id);
    }
}
