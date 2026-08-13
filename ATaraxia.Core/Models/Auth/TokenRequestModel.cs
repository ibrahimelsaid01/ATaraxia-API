namespace ATaraxia.Core.Models;

public class TokenRequestModel
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
