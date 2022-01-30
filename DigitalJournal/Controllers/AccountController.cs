﻿using DigitalJournal.Services.Interfaces;

namespace DigitalJournal.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly DigitalJournalContext _journalContext;
    private readonly IAccountService _accountService;

    //public AccountController(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, DigitalJournalContext journalContext)
    //{
    //    _userManager = userManager;
    //    _roleManager = roleManager;
    //    _signInManager = signInManager;
    //    _journalContext = journalContext;
    //}

    public AccountController(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, DigitalJournalContext journalContext, IAccountService accountService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _journalContext = journalContext;
        _accountService = accountService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var model = new Services.Interfaces.IndexWebModel();
        if (User.Identity.IsAuthenticated)
        {
            model = await _accountService.GetIndexWebModel(User.Identity.Name);
        }
        return View(model);
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        return View(new RegisterWebModel());
    }
    [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
    public async Task<IActionResult> Register(RegisterWebModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
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
            return RedirectToAction("Index", "Home");
        }
        var errors = result.Errors.Select(e => IdentityErrorCodes.GetDescription(e.Code)).ToArray();
        foreach (var error in errors)
        {
            ModelState.AddModelError("", error);
        }
        return View(model);
    }

    [AllowAnonymous]
    public IActionResult Login(string returnUrl)
    {
        return View(new LoginWebModel { ReturnUrl = returnUrl });
    }
    [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
    public async Task<IActionResult> Login(LoginWebModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
        if (result.Succeeded)
        {
            return LocalRedirect(model.ReturnUrl ?? "/");
        }
        ModelState.AddModelError("", "Ошибка в имени пользователя, либо в пароле при входе в систему");
        return View(new LoginWebModel { UserName = model.UserName, ReturnUrl = model.ReturnUrl });
    }

    public async Task<IActionResult> Edit()
    {
        var username = User.Identity!.Name;
        if (await _userManager.FindByNameAsync(username) is User user)
        {
            var profile = await _journalContext.Profiles.SingleOrDefaultAsync(p => p.Id == user.ProfileId);
            var model = new EditWebModel
            {
                SurName = profile.SurName,
                FirstName = profile.FirstName,
                Patronymic = profile.Patronymic,
                Email = user.Email,
                Birthday = profile.Birthday,
            };
            return View(model);
        }
        return NotFound();
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditWebModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var username = User.Identity!.Name;
        if (await _userManager.FindByNameAsync(username) is User user)
        {
            user.Email = model.Email;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var profile = await _journalContext.Profiles.SingleOrDefaultAsync(p => p.Id == user.ProfileId);
                profile.SurName = model.SurName;
                profile.FirstName = model.FirstName;
                profile.Patronymic = model.Patronymic;
                profile.Birthday = model.Birthday;
                _journalContext.Profiles.Update(profile);
                await _journalContext.SaveChangesAsync();
                return RedirectToAction("Index", "Account");
            }
            var errors = result.Errors.Select(e => IdentityErrorCodes.GetDescription(e.Code)).ToArray();
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        return View(model);
    }

    public IActionResult Password()
    {
        return View(new PasswordWebModel());        
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Password(PasswordWebModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var username = User.Identity!.Name;
        var user = await _userManager.FindByNameAsync(username); 
        var result = await _signInManager.CheckPasswordSignInAsync(user, model.OldPassword, false);
        if (result.Succeeded)
        {
            await _userManager.RemovePasswordAsync(user);
            var result2 = await _userManager.AddPasswordAsync(user, model.Password);
            if (result2.Succeeded)
            {
                return RedirectToAction("Index", "Account");
            }
            var errors = result2.Errors.Select(e => IdentityErrorCodes.GetDescription(e.Code)).ToArray();
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        ModelState.AddModelError("", "Неправильный старый пароль");
        return View(model);
    }

    public async Task<IActionResult> Logout(string returnUrl)
    {
        await _signInManager.SignOutAsync();
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
        var user = await _userManager.FindByNameAsync(UserName);
        return Json(user is null ? "true" : "Такой логин уже занят другим пользователем");
    }

    #endregion

    #region Вебмодели

    /// <summary> Вебмодель сведения о пользователе </summary>
    public class IndexWebModel
    {
        /// <summary> Сведения о профиле пользователя </summary>
        public Profile? Profile { get; set; }
        /// <summary> Сведения о пользовате </summary>
        public User User { get; set; }
        /// <summary> Роли пользователя </summary>
        public IEnumerable<string> UserRoleNames { get; set; } = Enumerable.Empty<string>();
    }
    /// <summary> Веб модель регистрации </summary>
    public class RegisterWebModel
    {
        [Required(ErrorMessage = "Фамилия обязательна для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Фамилия должна быть длинной от 3 до 200 символов")]
        [Display(Name = "Фамилия пользователя")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Имя обязательно для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Имя должно быть длинной от 3 до 200 символов")]
        [Display(Name = "Имя пользователя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Отчество обязательно для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Отчество должно быть длинной от 3 до 200 символов")]
        [Display(Name = "Отчество пользователя")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Нужно обязательно ввести свой адрес электронной почты")]
        [EmailAddress(ErrorMessage = "Нужно ввести корректный адрес своей электронной почты")]
        [Display(Name = "Адрес электронной почты E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна для ввода")]
        [Display(Name = "День рождения пользователя")]
        public DateTime Birthday { get; set; } = DateTime.Today.AddYears(-18);

        [Required(ErrorMessage = "Нужно обязательно ввести логин пользователя")]
        [Display(Name = "Логин пользователя")]
        [Remote("IsNameFree", "Account")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Нужно обязательно придумать и ввести какой-либо пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Нужно обязательно ввести подтверждение пароля")]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }
    /// <summary> Веб модель входа в систему </summary>
    public class LoginWebModel
    {
        [Required(ErrorMessage = "Нужно обязательно ввести логин пользователя")]
        [Display(Name = "Логин пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Нужно обязательно ввести свой пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string? ReturnUrl { get; set; }
    }
    /// <summary> Веб модель редактирования своих сведений </summary>
    public class EditWebModel
    {
        [Required(ErrorMessage = "Фамилия обязательна для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Фамилия должна быть длинной от 3 до 200 символов")]
        [Display(Name = "Фамилия пользователя")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Имя обязательно для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Имя должно быть длинной от 3 до 200 символов")]
        [Display(Name = "Имя пользователя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Отчество обязательно для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Отчество должно быть длинной от 3 до 200 символов")]
        [Display(Name = "Отчество пользователя")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Нужно обязательно ввести свой адрес электронной почты")]
        [EmailAddress(ErrorMessage = "Нужно ввести корректный адрес своей электронной почты")]
        [Display(Name = "Адрес электронной почты E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна для ввода")]
        [Display(Name = "День рождения пользователя")]
        public DateTime Birthday { get; set; } = DateTime.Today.AddYears(-18);
    }
    /// <summary> Веб модель смены пароля </summary>
    public class PasswordWebModel
    {
        [Required(ErrorMessage = "Нужно обязательно ввести свой текущий пароль")]
        [Display(Name = "Текущий пароль")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Нужно обязательно придумать и ввести какой-либо новый пароль")]
        [Display(Name = "Новый пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Нужно обязательно ввести подтверждение нового пароля")]
        [Display(Name = "Подтверждение нового пароля")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }

    #endregion
}

