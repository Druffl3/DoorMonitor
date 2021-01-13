using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoorService.SignalR.Entities
{
    /// <summary>
    /// Entity representation of Door
    /// </summary>
    [Table("DoorTable")]
    public class Door : DoorDTO
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        [Required]
        public int DoorId { get; set; }
    }

    /// <summary>
    /// Data Transfer Object representation of Door to protect primary Key
    /// </summary>
    public class DoorDTO
    {
        /// <summary>
        /// Name of the door
        /// </summary>
        [StringLength(50)]
        public string DoorName { get; set; }
        /// <summary>
        /// True if Door is Open
        /// False if Door is Closed
        /// </summary>
        public bool IsDoorOpened { get; set; }
        /// <summary>
        /// True if Door is locked
        /// False if Door is Unlocked
        /// </summary>
        public bool IsDoorLocked { get; set; }
    }
}
