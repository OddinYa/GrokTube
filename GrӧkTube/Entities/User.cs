using System.Numerics;

namespace GrӧkTube.Entities
{
    public class User
    {
       
        public int Id { get; set; }
        public string Name { get; set; }

        public string Login { get; set; }
        public string HashPassword { get; set; }

        public string PhoneNumber { get; set; } 
        public string Race { get; set; }

        public string AvatarUrl { get; set; } = "https://zefirka.club/uploads/posts/2023-01/1673278260_2-zefirka-club-p-serii-chelovek-na-avu-2.png";

    }
}

    
