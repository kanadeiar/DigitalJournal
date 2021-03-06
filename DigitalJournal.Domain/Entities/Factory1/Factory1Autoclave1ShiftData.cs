using DigitalJournal.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalJournal.Domain.Entities.Factory1
{
    /// <summary> Данные по автоклаву за смену </summary>
    public class Factory1Autoclave1ShiftData : Entity
    {
        /// <summary> Дата время смены </summary>
        [Required(ErrorMessage = "Дата время смены обязательна")]
        public DateTime Time { get; set; } = DateTime.Today;

        /// <summary> Номер автоклава </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Номер автоклава должен быть положительным числом")]
        public int AutoclaveNumber { get; set; }

        /// <summary> Дата начала автоклаирования </summary>
        [Required(ErrorMessage = "Дата начала автоклавирования обязательна")]
        public DateTime TimeStart { get; set; } = DateTime.Now;

        /// <summary> Продолжительность автоклавирования </summary>
        public TimeSpan AutoclavedTime { get; set; }

        /// <summary> Вид товара </summary>
        [Required, Range(1, int.MaxValue, ErrorMessage = "Должен быть выбран вид товара")]
        public int Factory1ProductTypeId { get; set; }
        /// <summary> Вид товара </summary>
        [ForeignKey(nameof(Factory1ProductTypeId))]
        public Factory1ProductType Factory1ProductType { get; set; }
        /// <summary> Количество пачек автоклавировано </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество пачек автоклавированных должно быть положительным числом")]
        public int AutoclavedCount { get; set; }

        /// <summary> Смена </summary>
        [Required, Range(1, int.MaxValue, ErrorMessage = "Должна быть выбрана смена")]
        public int Factory1ShiftId { get; set; }
        /// <summary> Cмена </summary>
        [ForeignKey(nameof(Factory1ShiftId))]
        public Factory1Shift Factory1Shift { get; set; }

        /// <summary> Автоклавщик </summary>
        [Required, Range(1, int.MaxValue, ErrorMessage = "Должен быть выбран автоклавщик")]
        public int ProfileId { get; set; }
        /// <summary> Автоклавщик </summary>
        [ForeignKey(nameof(ProfileId))]
        public Profile Profile { get; set; }
    }
}
