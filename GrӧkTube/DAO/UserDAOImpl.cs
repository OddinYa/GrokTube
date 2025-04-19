
using GrӧkTube.Entities;
using GrӧkTube.Repository;
using GrӧkTube.Service;

namespace GrӧkTube.DAO
{
    public  class UserDAOImpl : IDateServiceUser
    {
        private UsersTable _userTable;
        private UserService _userService;

       public UserDAOImpl()
        {
            _userTable = UsersTable.GetIntanse();
            _userService = new UserService();
        }


        public void SaveUsers(User user)
        {   
            _userTable.Users.Add(user);
        }

        public User GetUser(int id)
        {
            return _userTable.Users.FirstOrDefault<User>(u => u.Id == id);
        }

        public User FindUser(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return null;

            var user = _userTable.Users.Where<User>(u => u.Login == login
            && u.HashPassword == _userService.GetHashPassword(password)).FirstOrDefault();

            return user;
        }

        public bool LoginExists(string login)
        {
           if(_userTable.Users.Exists(u => u.Login == login))
            {
                return true;
            }
           return false;
        }

        public User GetUserByLogin(string login)
        {
           var user = _userTable.Users.FirstOrDefault(u => u.Login == login);

            return user;
        }
    }
}
