using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LotteryServices.Models;
using System.IdentityModel.Tokens.Jwt;

namespace LotteryServices.Utilitys
{
    public class Utilidades
    {
        private IConfiguration _configuration;

        public Utilidades(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string EncriptarSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create()) {

                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++) { 
                    sb.Append(bytes[i].ToString("x2"));
                }

                return sb.ToString();

            }

            
        }

        public string GenerarJwt(Usuario modelo)
        {
            var userClaimns = new[] {
                new Claim(ClaimTypes.NameIdentifier, modelo.UsuarioId.ToString()),
                new Claim(ClaimTypes.Email, modelo.Email!),
                new Claim(ClaimTypes.GivenName, modelo.NombreUsuario!),
                new Claim(ClaimTypes.Name, modelo.Nombre!)
            };

            var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                claims: userClaimns,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
                
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }

    }
}
