using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Gestao.Config
{
    public class TokenConfig
    {
        public static SymmetricSecurityKey Key { get; private set; }

        public static void GenerateKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var keyBytes = new byte[32]; // 256 bits
                rng.GetBytes(keyBytes);
                Key = new SymmetricSecurityKey(keyBytes);
            }
        }
    }
}