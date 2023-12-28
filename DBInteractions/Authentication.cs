using Backend.Models.Query_Models;
using Backend.Models.User_Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.DBInteractions
{
    public class Authentication
    {
        private readonly IConfiguration _config;
        public Authentication(IConfiguration config)
        {
            _config = config;
        }
        internal string Decorekey(string Token)
        {
            // Crea el manejador de tokens JWT
            JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();

            // Decodifica el token JWT
            JwtSecurityToken jwtSecurityToken = jwtTokenHandler.ReadJwtToken(Token);

            // Accede a las claims (reclamaciones) del token JWT
            List<Claim> claims = jwtSecurityToken.Claims.ToList();

            Login login = new Login();

            login.ProfessorId = int.Parse(claims[0].Value);
            login.Password = claims[1].Value;

            return Autenticate(login);
        }

        private string Autenticate(Login LoginParameters)
        {
            ExecuteStoreProcedure ESP = new ExecuteStoreProcedure();

            List<UserInfo> UserParameters = ESP.Execute<UserInfo>("ppGetVerificateUsers", LoginParameters, false);

            if (UserParameters == null)
            {
                return "Not found";
            }

            string Token = GenerateKey(LoginParameters, UserParameters.First());

            return Token;

        }
        private string GenerateKey(Login LoginParameters, UserInfo UserParameters)
        {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var Credential = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var Claims = new[]
            {
                new Claim("Id", UserParameters.ProfessorId.ToString()),
                new Claim("Name", UserParameters.ProfessorName),
                new Claim("Role", UserParameters.Role),

            };

            var Token = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                Claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: Credential
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
