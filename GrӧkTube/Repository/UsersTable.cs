using GrӧkTube.Entities;

namespace GrӧkTube.Repository
{
    public class UsersTable
    {
        private static UsersTable _instanse;

        public List<User> Users {  get; set; } = new List<User>();
        private UsersTable()
        {
            Users = new List<User>();
        }

        public static UsersTable GetIntanse()
        {
            return _instanse == null ? _instanse = new UsersTable() : _instanse;
        }
    }
}
