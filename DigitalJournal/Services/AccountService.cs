using DigitalJournal.Services.Interfaces;

namespace DigitalJournal.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly DigitalJournalContext _journalContext;
    public AccountService(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, DigitalJournalContext journalContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _journalContext = journalContext;
    }

    public async Task<IndexWebModel> GetIndexWebModel(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        var roles = await _userManager.GetRolesAsync(user);
        var model = new IndexWebModel
        {
            User = user,
            Profile = await _journalContext.Profiles.SingleOrDefaultAsync(p => p.Id == user.ProfileId),
            UserRoleNames = _roleManager.Roles.Where(r => roles.Contains(r.Name)).Select(r => r.Description).ToArray(),
        };
        return model;
    }
}

