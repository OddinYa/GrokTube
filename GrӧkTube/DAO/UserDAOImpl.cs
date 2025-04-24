
using GrӧkTube.Entities;
using GrӧkTube.Repository;
using GrӧkTube.Service;

namespace GrӧkTube.DAO
{
    public  class UserDAOImpl : IDateServiceUser
    {
        private readonly GrokRepository _context;
        private UserService _userService;

       public UserDAOImpl(GrokRepository context)
        {
            _context = context;
            _userService = new UserService();
        }


        public void SaveUsers(User user)
        {   
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User GetUser(int id)
        {
            return _context.Users.FirstOrDefault<User>(u => u.Id == id);
        }

        public User FindUser(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.Where<User>(u => u.Login == login
            && u.HashPassword == _userService.GetHashPassword(password)).FirstOrDefault();

            return user;
        }

        public bool LoginExists(string login)
        {
           if(_context.Users.Where(u => u.Login == login).FirstOrDefault()!=null)
            {
                return true;
            }
           return false;
        }

        public User GetUserByLoginAndId(string login,int id)
        {
           var user = _context.Users.FirstOrDefault(u => u.Login == login && u.Id == id);

            return user;
        }

        public User GetUserByLogin(string login)
        {
            throw new NotImplementedException();
        }
    }
}
