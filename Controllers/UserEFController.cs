using AutoMapper;
using DotNetAPI.Data;
using DotNetAPI.Models;
using DotNetAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEfController : ControllerBase
{
    private DataContextEf _entiryFramework;
    private IMapper _mapper;

    public UserEfController(IConfiguration config)
    {
        _entiryFramework = new DataContextEf(config);
        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserToAddDto, User>();
        }));
    }


    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        return _entiryFramework.Users.ToList<User>();
    }

    [HttpGet("GetUser/{userId:int}")]
    public User GetUser(int userId)
    {
        User? user = _entiryFramework.Users
            .FirstOrDefault(obj => obj.UserId == userId);
        if (user != null)
        {
            return user;
        }

        throw new Exception("Failed to get user");
    }

    [HttpPut]
    public IActionResult UpdateUser(User user)
    {
        User? userDb = _entiryFramework.Users
            .FirstOrDefault(obj => obj.UserId == user.UserId);
        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            if (_entiryFramework.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to update user");
        }

        throw new Exception("Failed to update user");
    }

    [HttpPost]
    public IActionResult CreateUser(UserToAddDto user)
    {
        User userDb = _mapper.Map<User>(user);
        userDb.Active = user.Active;
        userDb.FirstName = user.FirstName;
        userDb.LastName = user.LastName;
        userDb.Email = user.Email;
        userDb.Gender = user.Gender;
        _entiryFramework.Add(userDb);
        if (_entiryFramework.SaveChanges() > 0)
        {
            return Ok();
        }

        throw new Exception("Failed to create user");
    }

    [HttpDelete("DeleteUser/{userId:int}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _entiryFramework.Users
            .FirstOrDefault(obj => obj.UserId == userId);
        if (userDb != null)
        {
            _entiryFramework.Users.Remove(userDb);
            if (_entiryFramework.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to remove user");
        }

        throw new Exception("Failed to remove user");
    }
}