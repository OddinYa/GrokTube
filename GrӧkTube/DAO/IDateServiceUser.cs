using GrӧkTube.Entities;

namespace GrӧkTube.DAO
{
    public interface IDateServiceUser
    {
        public void SaveUsers(User user);

        public User GetUser(int id);
    }
}
