
namespace RBSoftSkkm
{
    /// <summary>
    /// Элемент списка ККТ 
    /// </summary>
    public class DeviceListResponse
    {
        /// <summary>
        /// Имя устройства.
        /// </summary>
        public string? DeviceName { get; set; }

        /// <summary>
        /// Тип драйвера
        /// </summary>
        public DeviceType Driver { get; set; }

        /// <summary>
        /// Имя пула, в который входит устройство.
        /// </summary>
        public string? Pool { get; set; }

        /// <summary>
        /// Описание статуса устройства.
        /// </summary>
        public string? DeviceStatusDescription { get; set; }
    }
}
