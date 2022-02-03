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

    public async Task<UserIndexWebModel> GetIndexWebModel(string? userName)
    {
        if (string.IsNullOrEmpty(userName))
            return new UserIndexWebModel
            {
                User = new User(),
                Profile = new Profile(),
                UserRoleNames = Array.Empty<string>(),
            };
        var user = await _userManager.FindByNameAsync(userName);
        var roles = await _userManager.GetRolesAsync(user);
        var model = new UserIndexWebModel
        {
            User = user,
            Profile = await _journalContext.Profiles.SingleOrDefaultAsync(p => p.Id == user.ProfileId),
            UserRoleNames = _roleManager.Roles.Where(r => roles.Contains(r.Name)).Select(r => r.Description).ToArray(),
        };
        return model;
    }

    public async Task<(bool success, string[] errors)> RequestRegisterUser(UserRegisterWebModel model)
    {
        var user = new User
        {
            UserName = model.UserName,
            Email = model.Email,
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "users");
            await _signInManager.SignInAsync(user, false);
            var profile = new Profile
            {
                SurName = model.SurName,
                FirstName = model.FirstName,
                Patronymic = model.Patronymic,
                Birthday = model.Birthday,
                UserId = user.Id,
            };
            _journalContext.Profiles.Add(profile);
            await _journalContext.SaveChangesAsync();
            user.ProfileId = profile.Id;
            await _userManager.UpdateAsync(user);
            return (true, Array.Empty<string>());
        }
        var errors = result.Errors.Select(e => IdentityErrorCodes.GetDescription(e.Code)).ToArray();
        return (false, errors);      
    }

    public async Task<bool> LoginPasswordSignIn(UserLoginWebModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
        return result.Succeeded;
    }

    public async Task<(bool founded, UserEditWebModel? model)> GetEditModelByName(string? username)
    {
        if (username is null)
            return (false, null);
        if (await _userManager.FindByNameAsync(username) is User user)
        {
            if (await _journalContext.Profiles.SingleOrDefaultAsync(p => p.Id == user.ProfileId) is Profile profile)
            {
                var model = new UserEditWebModel
                {
                    SurName = profile.SurName,
                    FirstName = profile.FirstName,
                    Patronymic = profile.Patronymic,
                    Email = user.Email,
                    Birthday = profile.Birthday,
                };
                return (true, model);
            }
        }
        return (false, null);
    }

    public async Task<(bool success, string[] errors)> RequestUpdateUserProfile(string? username, UserEditWebModel model)
    {
        if (username is null)
            return (false, new string[] { "Должно быть указано имя пользователя" });
        if (await _userManager.FindByNameAsync(username) is User user)
        {
            if (await _journalContext.Profiles.SingleOrDefaultAsync(p => p.Id == user.ProfileId) is Profile profile)
            {
                profile.SurName = model.SurName;
                profile.FirstName = model.FirstName;
                profile.Patronymic = model.Patronymic;
                user.Email = model.Email;
                profile.Birthday = model.Birthday;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    _journalContext.Profiles.Update(profile);
                    await _journalContext.SaveChangesAsync();
                    return (true, Array.Empty<string>());
                }
                var errors = result.Errors.Select(e => IdentityErrorCodes.GetDescription(e.Code)).ToArray();
                return (false, errors);
            }
        }
        return (false, new string[] { "Не удалось найти сущность в базе данных" });
    }

    public async Task<(bool success, string[] errors)> CheckAndChangePassword(string? username, UserPasswordWebModel model)
    {
        if (username is null)
            return (false, new string[] { "Должно быть указано имя пользователя" });
        var errors = new List<string>();
        if (await _userManager.FindByNameAsync(username) is User user)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.OldPassword, false);
            if (result.Succeeded)
            {
                var resultRemove = await _userManager.RemovePasswordAsync(user);
                if (resultRemove.Succeeded)
                {
                    var resultAdd = await _userManager.AddPasswordAsync(user, model.Password);
                    if (resultAdd.Succeeded)
                    {
                        return (true, Array.Empty<string>());
                    }
                    foreach (var item in resultAdd.Errors)
                        errors.Add(IdentityErrorCodes.GetDescription(item.Code));
                };
                foreach (var item in resultRemove.Errors)
                    errors.Add(IdentityErrorCodes.GetDescription(item.Code));
            }
            else
                errors.Add("Неправильный старый пароль");
        }
        else
            errors.Add("Не удалось найти сущность в базе данных");
        return (false, errors.ToArray());
    }

    public async Task SignOut()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<bool> UserNameIsFree(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        return user is null;
    }
}

