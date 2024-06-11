using System.ComponentModel.DataAnnotations;

namespace api_valheim.models;


public class Player
{
    [Key]
    public int PlayerId { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public DateTime? DateJoined { get; set; }

    public ICollection<Character>? Characters { get; set; } = null;
}