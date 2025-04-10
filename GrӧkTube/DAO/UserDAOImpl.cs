
using GrӧkTube.Entities;
using GrӧkTube.Repository;

namespace GrӧkTube.DAO
{
    public  class UserDAOImpl : IDateServiceUser
    {
        private UsersTable userTable;

       public UserDAOImpl()
        {
            userTable = UsersTable.GetIntanse();
        }


        public void SaveUsers(User user)
        {
            userTable.Users.Add(user);
        }

        public User GetUser(int id)
        {
            return userTable.Users.FirstOrDefault<User>(u => u.Id == id);
        }
    }
}
