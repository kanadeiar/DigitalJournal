using DigitalJournal.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalJournal.Domain.Entities.Factory1
{
    /// <summary> Данные по складу готовой продукции за смену </summary>
    public class Factory1Warehouse2ShiftData : Entity
    {
        /// <summary> Дата время смены </summary>
        [Required(ErrorMessage = "Дата время смены обязательна")]
        public DateTime Time { get; set; } = DateTime.Today;

        /// <summary> Вид товара на месте хранения 1 </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должен быть выбран вид товара на месте хранения 1")]
        public int Place1ProductTypeId { get; set; }
        /// <summary> Вид товара на месте хранений 1 </summary>
        [ForeignKey(nameof(Place1ProductTypeId))]
        public Factory1ProductType Place1ProductType { get; set; }
        /// <summary> Количество товара на месте хранения 1 </summary>
        public int Place1ProductsCount { get; set; }

        /// <summary> Вид товара на месте хранения 2 </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должен быть выбран вид товара на месте хранения 2")]
        public int Place2ProductTypeId { get; set; }
        /// <summary> Вид товара на месте хранений 2 </summary>
        [ForeignKey(nameof(Place2ProductTypeId))]
        public Factory1ProductType Place2ProductType { get; set; }
        /// <summary> Количество товара на месте хранения 2 </summary>
        public int Place2ProductsCount { get; set; }

        /// <summary> Вид товара на месте хранения 3 </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должен быть выбран вид товара на месте хранения 3")]
        public int Place3ProductTypeId { get; set; }
        /// <summary> Вид товара на месте хранений 3 </summary>
        [ForeignKey(nameof(Place3ProductTypeId))]
        public Factory1ProductType Place3ProductType { get; set; }
        /// <summary> Количество товара на месте хранения 3 </summary>
        public int Place3ProductsCount { get; set; }

        /// <summary> Смена </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должна быть выбрана смена")]
        public int Factory1ShiftId { get; set; }
        /// <summary> Cмена </summary>
        [ForeignKey(nameof(Factory1ShiftId))]
        public Factory1Shift Factory1Shift { get; set; }

        /// <summary> Кладовщик </summary>
        [Required(ErrorMessage = "Должен быть выбран кладовщик")]
        public string UserId { get; set; }
    }
}
