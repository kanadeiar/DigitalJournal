using DigitalJournal.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalJournal.Domain.Entities.Factory1
{
    /// <summary> Данные по складу сырья за смену </summary>
    public class Factory1Warehouse1ShiftData : Entity
    {
        /// <summary> Дата время смены </summary>
        [Required(ErrorMessage = "Дата время смены обязательна")]
        public DateTime Time { get; set; } = DateTime.Today;

        /// <summary> Объем песка в баке 1 </summary>
        public double Tank1LooseRawValue { get; set; }

        /// <summary> Объем цемента в баке 2 </summary>
        public double Tank2LooseRawValue { get; set; }

        /// <summary> Объем извести в баке 3 </summary>
        public double Tank3LooseRawValue { get; set; }

        /// <summary> Смена </summary>
        [Required, Range(1, int.MaxValue, ErrorMessage = "Должна быть выбрана смена")]
        public int Factory1ShiftId { get; set; }
        /// <summary> Cмена </summary>
        [ForeignKey(nameof(Factory1ShiftId))]
        public Factory1Shift Factory1Shift { get; set; }

        /// <summary> Кладовщик </summary>
        [Required, Range(1, int.MaxValue, ErrorMessage = "Должен быть выбран кладовщик")]
        public int ProfileId { get; set; }
        /// <summary> Кладовщик </summary>
        [ForeignKey(nameof(ProfileId))]
        public Profile Profile { get; set; }
    }
}
