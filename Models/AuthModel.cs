using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using TodoApi.Data;

namespace TodoApi.Models;

public class AuthModel
{
    static public async Task<User> LoginOrCreateUser(TodoContext context, GoogleUserInfo googleInfo)
    {
        User user;
        if (!context.Users.Any(u => u.Email == googleInfo.Email))
        {
            user = new User()
            {
                Email = googleInfo.Email,
                FirstName = googleInfo.GivenName,
                LastName = googleInfo.FamilyName
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
        else
        {
            user = context.Users.Where(u => u.Email == googleInfo.Email).First();
        }

        return user;
    }

    static public async Task<GoogleUserInfo> ValidateGoogleAccessToken(IHttpClientFactory httpClientFactory, string accessToken)
    {
        var client = httpClientFactory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/oauth2/v3/userinfo");
        request.Headers.Add("Authorization", $"Bearer {accessToken}");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var ex = new Exception("Invalid access token.");
            ex.Data.Add("StatusCode", StatusCodes.Status401Unauthorized);

            throw ex;
        }

        var content = await response.Content.ReadAsStringAsync();
        GoogleUserInfo? userInfo = JsonSerializer.Deserialize<GoogleUserInfo>(content);

        if (userInfo == null)
        {
            var ex = new Exception("No user found with access token, or token could be expired.");
            ex.Data.Add("StatusCode", StatusCodes.Status404NotFound);

            throw ex;
        }

        return userInfo;
    }

    static public string GenerateAccessToken(IConfiguration config, User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:AccessTokenSecret"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        IEnumerable<Claim> claims =
        [
            new ("Name", user.FirstName + " " + user.LastName),
            new ("Email", user.Email),
            new ("UserId", user.Id.ToString()),
            new ("TokenType", "AccessToken"),
        ];

        var Sectoken = new JwtSecurityToken(config["JWT:Issuer"],
            config["JWT:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        var token =  new JwtSecurityTokenHandler().WriteToken(Sectoken);

        return token;
    }

    static public string GenerateRefreshToken(IConfiguration config, User user, out DateTime expires)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:AccessTokenSecret"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        expires = DateTime.Now.AddDays(7);
        IEnumerable<Claim> claims =
        [
            new ("Email", user.Email),
            new ("UserId", user.Id.ToString()),
            new ("TokenType", "RefreshToken"),
        ];

        var Sectoken = new JwtSecurityToken(config["JWT:Issuer"],
            config["JWT:Issuer"],
            claims,
            expires: expires,
            signingCredentials: credentials);

        var token =  new JwtSecurityTokenHandler().WriteToken(Sectoken);

        return token;
    }

    static public object RefreshAccessToken(IConfiguration config, User user)
    {
        string newRefreshToken = GenerateRefreshToken(config, user, out var expires);
        string accessToken = GenerateAccessToken(config, user);

        return new { accessToken, refreshToken = newRefreshToken };
    }
}
