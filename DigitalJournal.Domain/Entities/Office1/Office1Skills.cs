using DigitalJournal.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalJournal.Domain.Entities.Office1
{
    /// <summary> Компетенции сотрудников </summary>
    public class Office1Skills : Entity
    {
        [Required(ErrorMessage = "Фамилия сотрудника обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Фамилия сотрудника должна быть длинной от 3 до 200 символов")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Имя сотрудника обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Имя сотрудника должна быть длинной от 3 до 200 символов")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Отчество сотрудника обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Отчество сотрудника должна быть длинной от 3 до 200 символов")]
        public string Patronymic { get; set; }

        /// <summary> Должность </summary>
        [Required, Range(1, int.MaxValue, ErrorMessage = "Должена быть выбрана должность сотрудника")]
        public int Office1PositionId { get; set; }
        /// <summary> Должность </summary>
        [ForeignKey(nameof(Office1PositionId))]
        public Office1Position Office1Position { get; set; }

        [Range(0, 10, ErrorMessage = "Нужно ввести уровень владения языком Assembler в диапазоне от 0 до 10")]
        public int Assembler { get; set; }

        [Range(0, 10, ErrorMessage = "Нужно ввести уровень владения языком C/C++ в диапазоне от 0 до 10")]
        public int CCpp { get; set; }

        [Range(0, 10, ErrorMessage = "Нужно ввести уровень владения языком C# в диапазоне от 0 до 10")]
        public int CSharp { get; set; }

        [Range(0, 10, ErrorMessage = "Нужно ввести уровень владения языком Java в диапазоне от 0 до 10")]
        public int Java { get; set; }

        [Range(0, 10, ErrorMessage = "Нужно ввести уровень владения языком PHP в диапазоне от 0 до 10")]
        public int PHP { get; set; }

        [Range(0, 10, ErrorMessage = "Нужно ввести уровень владения языком SQL в диапазоне от 0 до 10")]
        public int SQL { get; set; }
    }
}