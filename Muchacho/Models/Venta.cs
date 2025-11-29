using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("ventas")]
public class Venta
{
    [Key]
    [Column("idventa")]
    public int IdVenta { get; set; }

    [Column("iddisco")]
    public int IdDisco { get; set; }

    [Column("idusuario")]
    public int IdUsuario { get; set; }
}
