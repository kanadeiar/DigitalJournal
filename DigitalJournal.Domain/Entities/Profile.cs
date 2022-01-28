using DigitalJournal.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace DigitalJournal.Domain.Entities
{
    /// <summary> Профиль пользователя </summary>
    public class Profile : Entity
    {
        [Required(ErrorMessage = "Фамилия обязательна для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Фамилия должна быть длинной от 3 до 200 символов")]
        public string SurName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имя обязательно для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Имя должно быть длинной от 3 до 200 символов")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Отчество обязательно для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Отчество должно быть длинной от 3 до 200 символов")]
        public string Patronymic { get; set; } = string.Empty;

        [Required(ErrorMessage = "Дата рождения обязательна для пользователя")]
        public DateTime Birthday { get; set; } = DateTime.Today.AddYears(-18);

        /// <summary> Идентификатор пользователя связь один к одному </summary>
        [Required(ErrorMessage = "Обязательно нужно указать пользователя")]
        public string UserId { get; set; }
    }
}
