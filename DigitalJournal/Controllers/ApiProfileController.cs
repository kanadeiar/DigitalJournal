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
    public async Task<IEnumerable<Profile>> GetAll()
    {
        var profiles = await _journalContext.Profiles.ToListAsync();
        return profiles;
    }

    [HttpGet("{id}")]
    public async Task<Profile> Get(int id)
    {
        var profile = await _journalContext.Profiles.FirstAsync(p => p.Id == id);
        return profile;
    }

    [HttpPost]
    public async Task Add([FromBody] Profile profile)
    {
        await _journalContext.Profiles.AddAsync(profile);
        await _journalContext.SaveChangesAsync();
    }

    [HttpPut]
    public async Task Edit([FromBody] Profile profile)
    {
        _journalContext.Update(profile);
        await _journalContext.SaveChangesAsync();
    }

    [HttpDelete]
    public async Task Delete(int id)
    {
        _journalContext.Remove( new Profile { Id = id });
        await _journalContext.SaveChangesAsync();
    }
}
