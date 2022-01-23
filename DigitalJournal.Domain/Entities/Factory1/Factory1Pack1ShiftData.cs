using DigitalJournal.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalJournal.Domain.Entities.Factory1
{
    /// <summary> Данные по упаковке за смену </summary>
    public class Factory1Pack1ShiftData : Entity
    {
        /// <summary> Дата время смены </summary>
        [Required(ErrorMessage = "Дата время смены обязательна")]
        public DateTime Time { get; set; } = DateTime.Today;

        /// <summary> Вид товара </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должен быть выбран вид товара")]
        public int Factory1ProductTypeId { get; set; }
        /// <summary> Вид товара </summary>
        [ForeignKey(nameof(Factory1ProductTypeId))]
        public Factory1ProductType Factory1ProductType { get; set; }
        /// <summary> Количество упакованного товара </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество упакованнох товаров должно быть положительным числом")]
        public int ProductCount { get; set; }

        /// <summary> Смена </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должна быть выбрана смена")]
        public int Factory1ShiftId { get; set; }
        /// <summary> Cмена </summary>
        [ForeignKey(nameof(Factory1ShiftId))]
        public Factory1Shift Factory1Shift { get; set; }

        /// <summary> Пресовщик </summary>
        [Required(ErrorMessage = "Должен быть выбран пресовщик")]
        public string UserId { get; set; }
    }
}
