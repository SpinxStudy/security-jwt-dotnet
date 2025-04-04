using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TransactionAPI.Helper;

public static class Authentication
{
    public static string GenerateJwtToken()
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("3ss4-s3r4-4-m1nh4-s3cr3t-k3y-t0-us3-w1th-th1s-4pp"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Role, "payment_processor"),
            new Claim(JwtRegisteredClaimNames.Sub, "transaction-api"),
            new Claim(JwtRegisteredClaimNames.Iss, "financial-system"),
            new Claim(JwtRegisteredClaimNames.Aud, "payment-api")
        };

        var token = new JwtSecurityToken(
            issuer: "financial-system",
            audience: "payment-api",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
