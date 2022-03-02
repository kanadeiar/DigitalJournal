using DigitalJournal.Domain.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalJournal.Domain.Entities.Documents
{
    public class DocDirectory : Entity
    {
        [Required(ErrorMessage = "Название каталога обязательно нужно ввести")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название каталога должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }

        public int? BaseDirectoryId { get; set; }
        [ForeignKey(nameof(BaseDirectoryId))]
        public DocDirectory? BaseDirectory { get; set; }

        public virtual IEnumerable<DocDocument> Documents { get; set; } = new List<DocDocument>();

        public virtual IEnumerable<DocDirectory> Directorys { get; set; } = new List<DocDirectory>();
    }
}
