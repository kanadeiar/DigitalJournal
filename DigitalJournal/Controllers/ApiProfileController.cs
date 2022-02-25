namespace DigitalJournal.Controllers;

[ApiController, Microsoft.AspNetCore.Mvc.Route("/api/profile"), Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
public class ApiProfileController : ControllerBase
{
    private readonly DigitalJournalContext _journalContext;
    public ApiProfileController(DigitalJournalContext journalContext)
    {
        _journalContext = journalContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var profiles = await _journalContext.Profiles.ToListAsync();
        return Ok(profiles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var profile = await _journalContext.Profiles.FindAsync(id);
        if (profile is { })
            return Ok(profile);
        return NotFound($"not found profile with id = {id}");
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Profile profile)
    {
        if (profile is null)
            throw new ArgumentNullException(nameof(profile));
        profile.Id = default;
        try
        {
            _journalContext.Profiles.Add(profile);
            var id = await _journalContext.SaveChangesAsync();
            return Ok(id);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] Profile profile)
    {
        if (profile is null)
            throw new ArgumentNullException(nameof(profile));
        try
        {
            if (_journalContext.Profiles.Local.Any(x => x == profile) == false)
            {
                var origin = await _journalContext.Profiles.FindAsync(profile.Id);
                if (origin is null)
                    return NotFound($"not edit profile with id = {profile.Id}");
                origin.SurName = profile.SurName;
                origin.FirstName = profile.FirstName;
                origin.Patronymic = profile.Patronymic;
                origin.Birthday = profile.Birthday;
                origin.UserId = profile.UserId;
                _journalContext.Update(origin);
            }
            else
                _journalContext.Update(profile);
            var id = await _journalContext.SaveChangesAsync();
            return Ok(id);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        if (await _journalContext.Profiles.FindAsync(id) is not { } profile)
            return NotFound(false);
        try
        {
            _journalContext.Remove(profile);
            await _journalContext.SaveChangesAsync();
            return Ok(true);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
