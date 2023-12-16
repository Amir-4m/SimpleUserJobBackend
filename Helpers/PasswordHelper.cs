using System.Buffers.Text;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DotNetAPI.Helpers
{
    public class PasswordHelper
    {
        static private string GetPasswordSaltPlusSecret(string passwordSecret, byte[] passwordSalt)
        {
            return passwordSecret + Convert.ToBase64String(passwordSalt);
        }
        static public byte[] GetPasswordHash(string password, string passwordSecret, byte[] passwordSalt)
        {
            byte[] passwordHash = KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.ASCII.GetBytes(GetPasswordSaltPlusSecret(passwordSecret, passwordSalt)),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
            );

            return passwordHash;
        }


    }
}