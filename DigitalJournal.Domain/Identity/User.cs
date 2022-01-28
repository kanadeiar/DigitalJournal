using Microsoft.AspNetCore.Identity;

namespace DigitalJournal.Domain.Identity
{
    /// <summary> Пользователь </summary>
    public class User : IdentityUser
    {
        /// <summary> Метка удаления </summary>
        public bool IsDelete { get; set; }

        /// <summary> Идентификатор профиля пользователя связь один к одному </summary>
        public int? ProfileId { get; set; }
    }
}
