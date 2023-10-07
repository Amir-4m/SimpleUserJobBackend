using DotNetAPI.Data;
using DotNetAPI.Models;
using DotNetAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private DataContextDapper _dapper;

    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }


    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        return _dapper.LoadData<User>(@"SELECT * FROM TutorialAppSchema.Users");
    }

    [HttpGet("GetUser/{userId:int}")]
    public User GetUser(int userId)
    {
        return _dapper.LoadDataSingle<User>(
            $@"SELECT * FROM TutorialAppSchema.Users WHERE UserId='{userId}'"
        );
    }

    [HttpPut]
    public IActionResult UpdateUser(User user)
    {
        int executedSql = _dapper.ExecuteSql($@"
        UPDATE TutorialAppSchema.Users
            SET [FirstName] = '{user.FirstName}',
                [LastName] = '{user.LastName}',
                [Email] = '{user.Email}',
                [Gender] = '{user.Gender}',
                [Active] = '{user.Active}'
            WHERE UserId='{user.UserId}' 
        ");
        if (executedSql > 0)
        {
            return Ok();
        }

        throw new Exception("Failed to Update User");
    }

    [HttpPost]
    public IActionResult CreateUser(UserDto user)
    {
        int executedSql = _dapper.ExecuteSql($@"
        INSERT INTO TutorialAppSchema.Users(
            [FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active]
        ) VALUES (
            '{user.FirstName}',
            '{user.LastName}',
            '{user.Email}',
            '{user.Gender}',
            '{user.Active}'
        )
        ");
        if (executedSql > 0)
        {
            return Ok();
        }

        throw new Exception("Failed to create User");
    }

    [HttpDelete("DeleteUser/{userId:int}")]
    public IActionResult DeleteUser(int userId)
    {
        int executedSql = _dapper.ExecuteSql($@"
        DELETE FROM TutorialAppSchema.Users WHERE UserId='{userId}'
        ");
        if (executedSql > 0)
        {
            return Ok();
        }

        throw new Exception("Failed to Delete User");
    }

    [HttpGet("{userId:int}/UserJobInfos")]
    public IEnumerable<UserJobInfo> GetUserJobInfos(int userId)
    {
        return _dapper.LoadData<UserJobInfo>($@"SELECT * FROM TutorialAppSchema.UserJobInfo WHERE userId='{userId}'");
    }
    [HttpGet("{userId:int}/UserSalary")]
    public UserSalary GetUserSalary(int userId)
    {
        return _dapper.LoadDataSingle<UserSalary>($@"SELECT * FROM TutorialAppSchema.UserSalary WHERE userId='{userId}'");
    }
}