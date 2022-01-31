namespace DigitalJournal.Services.Interfaces;

public interface IAccountService
{
    /// <summary> Получить данные по аккаунту </summary>
    /// <returns>Данные аккаунта</returns>
    public Task<IndexWebModel> GetIndexWebModel(string userName);
    /// <summary> Команда на регистрацию нового пользователя </summary>
    /// <param name="model">Заполненная модель регистрации</param>
    /// <returns>Успешность регистрации, ошибки</returns>
    public Task<(bool success, string[] errors)> RequestRegisterUser(RegisterWebModel model);
}

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
