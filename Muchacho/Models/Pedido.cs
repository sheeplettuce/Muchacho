using System.ComponentModel.DataAnnotations.Schema;

[Table("pedidos")]
public class Pedido
{
    [Column("id")]
    public int Id { get; set; }

    [Column("idusuario")]
    public int IdUsuario { get; set; }

    [Column("fecha")]
    public DateTime? Fecha { get; set; }
}
