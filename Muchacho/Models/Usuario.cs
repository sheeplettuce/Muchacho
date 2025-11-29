
using System.ComponentModel.DataAnnotations.Schema;

    [Table("usuarios")]
    public class Usuario
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }

