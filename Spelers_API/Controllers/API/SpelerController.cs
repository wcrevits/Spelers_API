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
    [HttpPost, Authorize]
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

    /// <summary>
    /// Updates an existing Speler.
    /// </summary>
    /// <param name="id">The ID of the speler to update.</param>
    /// <param name="spelerUpdateVM">The updated speler data.</param>
    /// <returns>NoContent if successful.</returns>
    // PUT: api/Speler/5
    [HttpPut("{id}"), Authorize]
    public async Task<IActionResult> Put(int id, [FromBody] SpelerPostVM spelerUpdateVM)
    {
        try
        {
            // 1. Ophalen van de bestaande speler uit de database
            var speler = await _spelerService.GetById(id);

            if (speler == null)
            {
                return NotFound(new { message = $"Speler met ID {id} niet gevonden." });
            }

            speler.Naam = spelerUpdateVM.Naam;
            speler.Leeftijd = spelerUpdateVM.Leeftijd;
            speler.PositieId = spelerUpdateVM.PositieId;
            speler.TeamId = spelerUpdateVM.TeamId;

            await _spelerService.Update(speler);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Verwijder een specifieke speler op basis van ID.
    /// </summary>
    /// <param name="id">Het unieke ID van de speler.</param>
    /// <returns>NoContent bij succes.</returns>
    // DELETE: api/Speler/5
    [HttpDelete("{id}"), Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            // 1. Controleer of de speler bestaat
            var speler = await _spelerService.GetById(id);

            if (speler == null)
            {
                // Geef 404 als de speler al niet meer bestaat
                return NotFound(new { message = $"Speler met ID {id} niet gevonden." });
            }

            // 2. Verwijder de speler via de service
            await _spelerService.Delete(id);

            // 3. Retourneer 204 No Content (succesvolle verwijdering zonder body)
            return NoContent();
        }
        catch (Exception ex)
        {
            // Foutafhandeling (bijv. database constraints of serverfouten)
            return StatusCode(500, new { error = ex.Message });
        }
    }
}