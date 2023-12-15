namespace DotNetAPI.Dtos
{
    public partial class UserForLoginConfirmationDto
    {

        public byte[] PasswordHash {get; set;}
        public byte[] PasswordSalt {get; set;}

        public UserForLoginConfirmationDto()
        {
            PasswordHash ??= new byte[0];
            PasswordSalt ??= new byte[0];
        }

    }
}