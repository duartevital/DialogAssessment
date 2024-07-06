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
    public async Task<ActionResult<IEnumerable<WeightEntryResponse>>> GetWeightEntryByUserAsync([FromRoute] int userId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _dbContext.WeightEntry.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
            var response = _mapper.Map<IEnumerable<WeightEntryResponse>>(result);
            return Ok(response);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("WeightEntry")]
    public async Task<ActionResult<WeightEntryResponse>> PostWeightEntryAsync(WeightEntryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if(!await _dbContext.User.AnyAsync(x => x.Id == request.UserId))
                throw new KeyNotFoundException("User not found!");

            var mapped = _mapper.Map<WeightEntry>(request);

            var addedEntry = _dbContext.WeightEntry.Add(mapped);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var newEntry = await _dbContext.WeightEntry.FirstOrDefaultAsync(x => x.Id == addedEntry.Entity.Id) ??
                throw new KeyNotFoundException("Weight Entry not found!");

            return Ok(_mapper.Map<WeightEntryResponse>(newEntry));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("WeightEntry")]
    public async Task<IActionResult> PutWeightEntryAsync(WeightEntryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == null) throw new Exception("No Id was provided");
            if (!await _dbContext.User.AnyAsync(x => x.Id == request.UserId))
                throw new KeyNotFoundException("User not found!");

            var existing = await _dbContext.WeightEntry.FirstOrDefaultAsync(x => x.Id == request.Id) ??
                throw new KeyNotFoundException("Weight Entry not found!");
            _mapper.Map(request, existing);


            _dbContext.WeightEntry.Update(existing);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("WeightEntry/{id}")]
    public async Task<IActionResult> DeleteWeightEntryByIdAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        try
        {
            var entryToRemove = await _dbContext.WeightEntry.FirstOrDefaultAsync(x => x.Id == id) ??
                throw new KeyNotFoundException("Weight Entry not found!");

            _dbContext.WeightEntry.Remove(entryToRemove);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}

