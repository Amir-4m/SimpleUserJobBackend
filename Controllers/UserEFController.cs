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
    IUserRepository _userRepository;
    private IMapper _mapper;

    public UserEfController(IConfiguration config, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserToAddDto, User>();
        }));
    }


    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        return _userRepository.GetUsers();
    }

    [HttpGet("GetUser/{userId:int}")]
    public User GetUser(int userId)
    {
        return _userRepository.GetUser(userId);
    }

    [HttpPut]
    public IActionResult UpdateUser(User user)
    {
        User? userDb = _userRepository.GetUser(user.UserId);
        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            if (_userRepository.SaveChanges())
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
        _userRepository.AddEntity<User>(userDb);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }

        throw new Exception("Failed to create user");
    }

    [HttpDelete("DeleteUser/{userId:int}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _userRepository.GetUser(userId);
        if (userDb != null)
        {
            _userRepository.RemoveEntity<User>(userDb);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to remove user");
        }

        throw new Exception("Failed to remove user");
    }
}