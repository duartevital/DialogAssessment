using Microsoft.AspNetCore.Mvc;
using WeightManager.Models;
using Microsoft.EntityFrameworkCore;
using WeightManager.Model.Models;
using WeightManager.Model;
using AutoMapper;
using WeightManager.Model.DTOs;

namespace WeightManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeightController : ControllerBase
{
    private readonly WeightManagerContext _dbContext;
    private readonly IMapper _mapper;

    public WeightController(WeightManagerContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet("WeightEntry/{userId}")]
    public async Task<ActionResult<WeightEntry>> GetWeightEntryByUser([FromRoute] int userId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dbContext.WeightEntry.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            var response = _mapper.Map<WeightEntryResponse>(result);
            return Ok(result);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("WeightEntry")]
    public async Task<ActionResult<WeightEntry>> PostWeightEntry(WeightEntryRequest entry, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == entry.UserId) ??
                throw new Exception("User not found!");

            var mapped = _mapper.Map<WeightEntry>(entry);

            _dbContext.WeightEntry.Add(mapped);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(entry);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}

