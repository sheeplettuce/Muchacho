namespace Muchacho.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public String Name { get; set; }
        public int Id { get; set; }

        // Lista estática en memoria con usuarios de prueba
        public static List<User> Users = new List<User>
        {
            new User { Email = "santi@gmail.com", Password = "nerv", Name = "Santi", Id= 1 },
            new User { Email = "admin@gmail.com", Password = "1234", Name = "Admin", Id = 2 }
        };
    }
}