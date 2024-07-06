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
    public async Task<ActionResult<UserResponse>> GetUserAsync([FromRoute] int id, CancellationToken cancellationToken)
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
    public async Task<ActionResult<User>> PostUserAsync(UserRequest request, CancellationToken cancellationToken)
    {

        var mapped = _mapper.Map<User>(request);

        try
        {
            var addedUser = _dbContext.User.Add(mapped);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var newUser = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == addedUser.Entity.Id) ??
                throw new KeyNotFoundException("User not found!");

            return Ok(_mapper.Map<UserResponse>(newUser));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("User")]
    public async Task<IActionResult> PutUserAsync(UserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var existing = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == request.Id) ??
                throw new KeyNotFoundException("User not found!");
            _mapper.Map(request, existing);


            _dbContext.User.Update(existing);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }
}
