using System.Security.Cryptography;
using System.Text;
using DotNetAPI.Data;
using DotNetAPI.Dtos;
using DotNetAPI.Helpers;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DotNetAPI.Controllers
{
    public class AuthController : ControllerBase
    {

        private readonly DataContextDapper _dapper;
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _config = config;

        }

        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password != userForRegistration.PasswordConfirm)
            {
                return BadRequest("Passwords do not match!");
            }

            string sqlCheckUserExists = "SELECT Email FROM TutorialAppSchema.Auth WHERE Email = '" + userForRegistration.Email + "'";

            IEnumerable<string> existingUser = _dapper.LoadData<string>(sqlCheckUserExists);

            if (existingUser.Count() != 0)
            {
                return BadRequest("user with this email already exists!");
            }

            byte[] passwordSalt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }

            byte[] passwordHash = PasswordHelper.GetPasswordHash(
                userForRegistration.Password,
                _config.GetSection("AppSettings.PasswordKey").Value,
                 passwordSalt
                 );

            string sqlAddAuth = @"
            INSERT INTO TutorialAppSchema.Auth
            ( 
            [Email], [PasswordHash], [PasswordSalt]
            )
            VALUES
            ('" + userForRegistration.Email + "', @PasswordHash, @PasswordSalt)";

            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", System.Data.SqlDbType.VarBinary);
            passwordSaltParameter.Value = passwordSalt;

            SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", System.Data.SqlDbType.VarBinary);
            passwordHashParameter.Value = passwordHash;

            sqlParameters.Add(passwordSaltParameter);
            sqlParameters.Add(passwordHashParameter);

            if (!_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
            {
                throw new Exception("Failed to register the user.");
            }

            return Ok();
        }

        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {

            string sqlForHashAndSalt = @"
            SELECT [Email],
            [PasswordHash],
            [PasswordSalt] FROM TutorialAppSchema.Auth WHERE Email = '" + userForLogin.Email + "'";
            UserForLoginConfirmationDto userForLoginConfirmation = _dapper.LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);

            byte[] passwordHash = PasswordHelper.GetPasswordHash(
                userForLogin.Password,
                _config.GetSection("AppSettings.PasswordKey").Value,
                 userForLoginConfirmation.PasswordSalt
                 );

            for (int i = 0; i < passwordHash.Length; i++)
            {
                try
                {
                    if (passwordHash[i] != userForLoginConfirmation.PasswordHash[i])
                    {
                        return Unauthorized("password or email is wrong.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Unauthorized("password or email is wrong.");
                }


            }
            return Ok();
        }
    }
}