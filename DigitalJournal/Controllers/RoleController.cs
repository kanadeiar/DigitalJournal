namespace DigitalJournal.Controllers;

[Authorize(Roles = "admins")]
public class RoleController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly DigitalJournalContext _context;
    public RoleController(UserManager<User> userManager, RoleManager<Role> roleManager, DigitalJournalContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    /// <summary> Обзор всех ролей </summary>
    /// <returns>Роли пользователей</returns>
    public async Task<IActionResult> Index()
    {
        var roles = _roleManager.Roles;
        var models = roles.Select(r => new RoleWebModel
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
        }).ToArray();
        foreach (var item in models)
        {
            var users = await _userManager.GetUsersInRoleAsync(item.Name);
            var profiles = users.Select(u => _context.Profiles.Single(p => p.Id == u.ProfileId));
            string result = !profiles.Any()
                ? "Нет пользователей"
                : string.Join(", ", profiles.Take(3).Select(u => $"{u.SurName} {u.FirstName[0]}. {u.Patronymic[0]}.").ToArray());
            item.UsersNames = users.Count() > 5 ? $"{result}, и др." : result;
            item.UsersCount = users.Count();
        };
        return View(models);
    }

    /// <summary> Создание новой роли </summary>
    public IActionResult Create()
    {
        return View("Edit", new RoleEditWebModel());
    }

    /// <summary> Редактирование роли </summary>
    public async Task<IActionResult> Edit(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return View(new RoleEditWebModel());
        }
        if (await _roleManager.FindByIdAsync(id) is { } role)
        {
            var model = new RoleEditWebModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
            };
            return View(model);
        }
        return NotFound();
    }

    /// <summary> Редактирование роли </summary>
    /// <param name="model">Модель роли</param>
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(RoleEditWebModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var role = await _roleManager.FindByIdAsync(model.Id);
        IdentityResult result;
        if (role is null)
        {
            var newRole = new Role
            {
                Name = model.Name,
                Description = model.Description,
            };
            result = await _roleManager.CreateAsync(newRole);
        }
        else
        {
            role.Name = model.Name;
            role.Description = model.Description;
            result = await _roleManager.UpdateAsync(role);
        }
        if (result.Succeeded)
            return RedirectToAction("Index", "Role");
        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);
        return View(model);
    }

    /// <summary> Удалить роль </summary>
    /// <param name="id">Идентификатор роли</param>
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
            return BadRequest();
        var r = await _roleManager.FindByIdAsync(id);
        if (r is null)
            return NotFound();
        var model = new RoleWebModel
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
        };
        var users = (await _userManager.GetUsersInRoleAsync(r.Name)).Select(u => _context.Profiles.Single(p => p.Id == u.ProfileId));
        string result = !users.Any()
            ? "Нет пользователей"
            : string.Join(", ", users.Take(3).Select(u => $"{u.SurName} {u.FirstName[0]}. {u.Patronymic[0]}.").ToArray());
        model.UsersNames = users.Count() > 3 ? $"{result}, и др." : result;
        model.UsersCount = users.Count();
        return View(model);
    }

    /// <summary> Подтверждение удаления роли </summary>
    /// <param name="id">Идентификатор роли</param>
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (string.IsNullOrEmpty(id))
            return BadRequest();
        var role = await _roleManager.FindByIdAsync(id);
        var users = await _userManager.GetUsersInRoleAsync(role.Name);
        if (users.Any() || role.Name == "admins")
            return BadRequest();
        await _roleManager.DeleteAsync(role);
        return RedirectToAction("Index", "Role");
    }

    #region WebAPI

    [AllowAnonymous]
    public async Task<IActionResult> IsNameFree(string Name, string Id)
    {
        var role = await _roleManager.FindByNameAsync(Name);
        var result = (role is null || (role is { } r && r.Id == Id)) ? "true" : "Роль пользователей с таким именем уже существует";
        return Json(result);
    }

    #endregion

    /// <summary> Веб модель просмотра роли пользователей </summary>
    public class RoleWebModel
    {
        public string? Id { get; set; }

        [Display(Name = "Название роли пользователей")]
        public string Name { get; set; }

        [Display(Name = "Описание роли пользователей")]
        public string Description { get; set; }

        [Display(Name = "Пользователи, имеющие эту роль")]
        public string UsersNames { get; set; }

        [Display(Name = "Количество пользователей у роли")]
        public int UsersCount { get; set; }
    }

    /// <summary> Веб модель редактирования роли пользователей </summary>
    public class RoleEditWebModel
    {
        [HiddenInput(DisplayValue = false)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Системное имя роли обязательна для роли пользователей")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Системное имя роли должно быть длинной от 3 до 200 символов")]
        [Display(Name = "Системное имя роли пользователей")]
        [Remote("IsNameFree", "Role", AdditionalFields = "Id")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Описание обязательно для роли пользователей")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Описание роли должно быть длинной от 3 до 200 символов")]
        [Display(Name = "Описание роли пользователей")]
        public string Description { get; set; }
    }
}

