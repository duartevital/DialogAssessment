using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeightManager.Model.DTOs;
using WeightManager.Model.Models;
using WeightManager.Models;

namespace WeightManager.Controllers;

public class UserController : ControllerBase
{
    private readonly WeightManagerContext _dbContext;
    private readonly IMapper _mapper;

    public UserController(WeightManagerContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet("User/{id}")]
    public async Task<ActionResult<UserResponse>> GetUser([FromRoute] int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dbContext.User.Include(x => x.WeightEntry).FirstAsync(x => x.Id == id, cancellationToken);
            var response = _mapper.Map<UserResponse>(result);

            var latestWeightEntry = result.WeightEntry.Where(x => x.UserId == id).OrderByDescending(x => x.CreationDate).FirstOrDefault();
            if (latestWeightEntry != null && result.TargetWeight != null)
                response.WeightLeft = Math.Abs(latestWeightEntry.Weight - result.TargetWeight.Value);

            return Ok(response);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("User")]
    public async Task<ActionResult<User>> PostUser(UserRequest user, CancellationToken cancellationToken)
    {

        var mapped = _mapper.Map<User>(user);

        try
        {
            _dbContext.User.Add(mapped);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok(user);
    }

    [HttpPut("User")]
    public async Task<IActionResult> PutUser(UserRequest user, CancellationToken cancellationToken)
    {
        try
        {
            var existing = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == user.Id) ??
                throw new Exception("User not found!");
            _mapper.Map(user, existing);


            _dbContext.User.Update(existing);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
