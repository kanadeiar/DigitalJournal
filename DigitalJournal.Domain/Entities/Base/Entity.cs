using System.ComponentModel.DataAnnotations;

namespace DigitalJournal.Domain.Entities.Base
{
    public abstract class Entity
    {
        /// <summary> Идентификатор </summary>
        [Key]
        public int Id { get; set; }
        /// <summary> Датовременной штамп </summary>
        [Timestamp]
        public byte[]? Timestamp { get; set; }
    }
}
