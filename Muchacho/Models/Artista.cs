using System.ComponentModel.DataAnnotations.Schema;

[Table("artistas")]
public class Artista
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }
}
