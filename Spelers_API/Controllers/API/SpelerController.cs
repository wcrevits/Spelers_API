using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spelers_API.Domain.EntitiesDB;
using Spelers_API.Services;
using Spelers_API.Services.Interfaces;
using Spelers_API.ViewModels;

[Route("api/[controller]")]
[ApiController]
public class SpelerController : ControllerBase
{
    private readonly ISpelerService _spelerService;
    private readonly IMapper _mapper;

    public SpelerController(ISpelerService spelerService, IMapper mapper)
    {
        _spelerService = spelerService;
        _mapper = mapper;
    }


    /// <summary>
    /// Get the list of all Spelers.
    /// </summary>
    /// <returns>The list of Spelers.</returns>

    // could also be [HttpGet, Authorize(Policy = "AdminManager")]
    // GET: api/Speler
    [HttpGet, Authorize]
    public async Task<ActionResult<IEnumerable<SpelerVM>>> Get()
    {
        try
        {
            // ophalen Employees via service
            var spelers = await _spelerService.GetAll();

            // mapping Entity -> ViewModel
            var data = _mapper.Map<IEnumerable<SpelerVM>>(spelers);

            if (data == null || !data.Any())
            {
                // Als er geen gegevens gevonden worden
                return NotFound();
            }

            // succesvolle response
            return Ok(data);
        }
        catch (Exception ex)
        {
            // interne fout
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Creates a Speler.
    /// </summary>
    /// <remarks>
    /// Sample request
    /// POST api/Speler
    /// </remarks>

    // POST: api/Speler
    [HttpPost]
    public async Task<ActionResult<SpelerVM>> Post([FromBody] SpelerPostVM spelerPostVM)
    {
        // MANUAL TEST (Bypass AutoMapper)
        var speler = new Speler
        {
            Naam = spelerPostVM.Naam, // Assigning manually
            Leeftijd = spelerPostVM.Leeftijd,
            PositieId = spelerPostVM.PositieId,
            TeamId = spelerPostVM.TeamId
        };

        await _spelerService.Add(speler);

        var result = _mapper.Map<SpelerVM>(speler);
        return Ok(result);
    }
}