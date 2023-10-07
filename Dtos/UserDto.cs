namespace DotNetAPI.Dtos;

public class UserToAddDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public bool Active { get; set; }

    public UserToAddDto()
    {
        FirstName ??= "";
        LastName ??= "";
        Email ??= "";
        Gender ??= "";
    }
}

public class UserDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public bool Active { get; set; }

    public UserDto()
    {
        FirstName ??= "";
        LastName ??= "";
        Email ??= "";
        Gender ??= "";
    }
}