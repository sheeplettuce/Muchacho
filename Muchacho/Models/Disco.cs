using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("discos")]
public class Disco
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("artist_id")]
    public int ArtistId { get; set; }

    [Column("year")]
    public int? Year { get; set; }

    [Column("coverurl")]  // Asegúrate que esté en minúsculas
    public string CoverURL { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("name")]
    public string Name { get; set; }
}