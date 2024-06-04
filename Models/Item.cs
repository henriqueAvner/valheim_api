using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_valheim.models;

public class Item
{
    [Key]
    public int ItemId { get; set; }
    public string? ItemName { get; set; }

    public string? ItemDescription { get; set; }

    public string? ItemType { get; set; }

    [ForeignKey("CharacterId")]
    public int CharacterId { get; set; }

    public Character? Character { get; set; }
}