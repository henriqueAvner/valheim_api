namespace api_valheim.DTO;

public class LoginDTORequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class LoginDDTOResponse
{
    public string? Token { get; set; }
}