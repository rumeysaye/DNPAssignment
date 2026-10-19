using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;
    
    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        try
        { 
            await VerifyUserNameIsAvaiableAsync(request.UserName);
            User user = new()
            {
                UserName = request.UserName,
                Password = request.Password
            };
            User created = await userRepo.AddAsync(user);
            UserDto dto = new()
            {
                Id = created.Id,
                UserName = created.UserName
            };
            return Created($"/users/{dto.Id}", created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingleUser([FromRoute]int id)
    {
            User user = await userRepo.GetSingleAsync(id);

            UserDto dto = new()
            {
                Id = user.Id,
                UserName = user.UserName
            };

            return Ok(dto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetManyUsers(
    [FromQuery] string? username)
    {
        IQueryable<User> users = userRepo.GetManyAsync();

        List<UserDto> dtos = new();
        foreach (User user in users)
        {
            if (string.IsNullOrEmpty(username) || user.UserName.Contains(username, StringComparison.OrdinalIgnoreCase))
            {
                UserDto dto = new()
                {
                    Id = user.Id,
                    UserName = user.UserName
                };
                dtos.Add(dto);
            }
        }
        return Ok(dtos);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(
        [FromRoute] int id,
        [FromBody] UserDto request)
    {
        
    User existingUser = await userRepo.GetSingleAsync(id);
    existingUser.UserName = request.UserName;
    await userRepo.UpdateAsync(existingUser);
    return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        await userRepo.DeleteAsync(id);

        return NoContent();
    }

    private Task VerifyUserNameIsAvaiableAsync(string username)
    {
        IQueryable<User> users = userRepo.GetManyAsync();
        bool exists = users.Any(u => u.UserName == username);
        if (exists)
        {
            throw new InvalidOperationException("Username already exists!");
        }
        return Task.CompletedTask;
    }
}