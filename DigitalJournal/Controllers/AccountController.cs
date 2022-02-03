using DigitalJournal.Services.Interfaces;

namespace DigitalJournal.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var model = new Services.Interfaces.UserIndexWebModel();
        if (User.Identity!.IsAuthenticated)
        {
            model = await _accountService.GetIndexWebModel(User.Identity.Name);
        }
        return View(model);
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        return View(new Services.Interfaces.UserRegisterWebModel());
    }
    [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
    public async Task<IActionResult> Register(Services.Interfaces.UserRegisterWebModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var (result, errors) = await _accountService.RequestRegisterUser(model);
        if (result)
        {
            return RedirectToAction("Index", "Home");
        }
        foreach (var error in errors)
            ModelState.AddModelError("", error);
        return View(model);
    }

    [AllowAnonymous]
    public IActionResult Login(string returnUrl)
    {
        return View(new Services.Interfaces.UserLoginWebModel { ReturnUrl = returnUrl });
    }
    [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
    public async Task<IActionResult> Login(Services.Interfaces.UserLoginWebModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        if (await _accountService.LoginPasswordSignIn(model))
        {
            return LocalRedirect(model.ReturnUrl ?? "/");
        }
        ModelState.AddModelError("", "Ошибка в имени пользователя, либо в пароле при входе в систему");
        return View(new Services.Interfaces.UserLoginWebModel { UserName = model.UserName, ReturnUrl = model.ReturnUrl });
    }

    public async Task<IActionResult> Edit()
    {
        var username = User.Identity!.Name;
        var (result, model) = await _accountService.GetEditModelByName(username);
        if (result)
        {
            return View(model);
        }
        return NotFound();
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserEditWebModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var username = User.Identity!.Name;
        var (result, errors) = await _accountService.RequestUpdateUserProfile(username, model);
        if (result)
        {
            return RedirectToAction("Index", "Account");
        }
        foreach (var error in errors)
            ModelState.AddModelError("", error);
        return View(model);
    }

    public IActionResult Password()
    {
        return View(new UserPasswordWebModel());
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Password(UserPasswordWebModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var username = User.Identity!.Name;
        var (result, errors) = await _accountService.CheckAndChangePassword(username, model);
        if (result)
        {
            return RedirectToAction("Index", "Account");
        }
        foreach (var error in errors)
            ModelState.AddModelError("", error);
        return View(model);
    }

    public async Task<IActionResult> Logout(string returnUrl)
    {
        await _accountService.SignOut();
        return LocalRedirect(returnUrl ?? "/");
    }

    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    #region WebAPI

    [AllowAnonymous]
    public async Task<IActionResult> IsNameFree(string UserName)
    {
        var result = await _accountService.UserNameIsFree(UserName);
        return Json(result ? "true" : "Такой логин уже занят другим пользователем");
    }

    #endregion
}

