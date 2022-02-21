using DigitalJournal.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DigitalJournal.Domain.Entities.Office1
{
    /// <summary> Должности сотрудников </summary>
    public class Office1Position : Entity
    {
        /// <summary> Название должности </summary>
        [Required(ErrorMessage = "Название должности обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название должности должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }

        /// <summary> Данные по компетенциям </summary>
        public List<Office1Skills> OfficeSkills { get; set; } = new List<Office1Skills>();
    }
}