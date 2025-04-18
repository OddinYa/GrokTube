
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

        public User FindUser(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return null;


            var user = userTable.Users.Where<User>(u => u.Login == login
            && u.HashPassword == GetHashPassword(password)).FirstOrDefault();

            return user;
        }


        private string GetHashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
