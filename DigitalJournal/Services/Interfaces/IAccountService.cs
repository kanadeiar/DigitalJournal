namespace DigitalJournal.Services.Interfaces;

public interface IAccountService
{
    /// <summary> Получить данные по аккаунту </summary>
    /// <returns>Данные аккаунта</returns>
    public Task<UserIndexWebModel> GetIndexWebModel(string? userName);
    /// <summary> Команда на регистрацию нового пользователя </summary>
    /// <param name="model">Заполненная модель регистрации</param>
    /// <returns>Успешность регистрации, ошибки</returns>
    public Task<(bool success, string[] errors)> RequestRegisterUser(UserRegisterWebModel model);
    /// <summary> Вход в систему пользователя </summary>
    /// <param name="model">Модель входа</param>
    /// <returns>Успешность, ошибки</returns>
    public Task<bool> LoginPasswordSignIn(UserLoginWebModel model);
    /// <summary> Получить для редактирования веб модель профиля пользователя </summary>
    /// <param name="username">Имя пользователя</param>
    /// <returns>Найдена модель, Веб модель для редактирования</returns>
    public Task<(bool founded, UserEditWebModel? model)> GetEditModelByName(string? username);
    /// <summary> Команда на обновление данных профиля пользователя </summary>
    /// <param name="model">Имя пользователя, Обновленные данные</param>
    /// <returns>Успешность, ошибки</returns>
    public Task<(bool success, string[] errors)> RequestUpdateUserProfile(string? username, UserEditWebModel model);
    /// <summary> Команда на изменение пароля </summary>
    /// <param name="username">Имя пользователя</param>
    /// <param name="model">Модель смены пароля</param>
    /// <returns>Успешность, ошибки</returns>
    public Task<(bool success, string[] errors)> CheckAndChangePassword(string? username, UserPasswordWebModel model);
}

#region Вебмодели

/// <summary> Вебмодель сведения о пользователе </summary>
public class UserIndexWebModel
{
    /// <summary> Сведения о профиле пользователя </summary>
    public Profile? Profile { get; set; }
    /// <summary> Сведения о пользовате </summary>
    public User User { get; set; }
    /// <summary> Роли пользователя </summary>
    public IEnumerable<string> UserRoleNames { get; set; } = Enumerable.Empty<string>();
}
/// <summary> Веб модель регистрации </summary>
public class UserRegisterWebModel
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
public class UserLoginWebModel
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
public class UserEditWebModel
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
public class UserPasswordWebModel
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