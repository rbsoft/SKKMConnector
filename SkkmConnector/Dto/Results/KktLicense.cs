using System;

namespace RBSoftSkkm
{
    /// <summary>
    /// Лицензия ККТ 
    /// </summary>
    public class KktLicense
    {
        /// <summary>
        /// Номер лицензии.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Наименование лицензии.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Действует с.
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Действует до.
        /// </summary>
        public DateTime ValidUntil { get; set; }

        /// <summary>
        /// Версия узла
        /// </summary>
        public string? UnitVersion { get; set; }

        /// <summary>
        /// Описание лицензии.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Признак активной лицензии.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
