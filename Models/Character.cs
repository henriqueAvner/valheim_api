using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_valheim.models;


public class Character
{
    [Key]
    public int CharacterId { get; set; }
    public string? Name { get; set; }

    public int Level { get; set; }

    [ForeignKey("PlayerId")]
    public int PlayerId { get; set; }

    public Player? Player { get; set; }

    public ICollection<Item>? Items { get; set; } = null;


}