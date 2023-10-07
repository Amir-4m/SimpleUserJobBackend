namespace DotNetAPI.Dtos;

public class UserJobInfoDto
{
    public int UserId { get; set; }
    public string JobTitle { get; set; }
    public string Department { get; set; }
    public UserJobInfoDto()
    {
        JobTitle ??= "";
        Department ??= "";
    }
}