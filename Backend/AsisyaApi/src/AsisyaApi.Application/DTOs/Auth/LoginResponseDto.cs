namespace AsisyaApi.Application.DTOs.Auth;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public string Username { get; set; } = string.Empty;

    public LoginResponseDto() { }

    public LoginResponseDto(string token, DateTime expiresAtUtc, string username)
    {
        Token = token;
        ExpiresAtUtc = expiresAtUtc;
        Username = username;
    }
}