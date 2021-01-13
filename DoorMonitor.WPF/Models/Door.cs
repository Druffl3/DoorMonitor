using DoorMonitor.Common;
using DoorMonitor.WPF.Helpers;
using Newtonsoft.Json;

namespace DoorMonitor.WPF.Models
{
    /// <summary>
    /// Model representation of Door for deserialising from its JSON representaion
    /// </summary>
    public class Door : DoorResponse
    {
        /// <summary>
        /// The Door ID
        /// </summary>
        [JsonProperty("doorId")]
        public int DoorId { get; set; }
        /// <summary>
        /// The Door's name
        /// </summary>
        [JsonProperty("doorName")]
        public string DoorName { get; set; }
        /// <summary>
        /// The locked status of the door
        /// </summary>
        [JsonProperty("isDoorLocked")]
        public bool IsDoorLocked { get; set; }
        /// <summary>
        /// The opened status of the door
        /// </summary>
        [JsonProperty("isDoorOpened")]
        public bool IsDoorOpened { get; set; }
    }

    /// <summary>
    /// Responsed that are received
    /// </summary>
    public class DoorResponse
    {
        /// <summary>
        /// Response Status Code
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }
        /// <summary>
        /// Response message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class BindableDoor : BindableBase
    {
        /// <summary>
        /// The Door ID
        /// </summary>
        public int DoorId;
        /// <summary>
        /// The Door's name
        /// </summary>
        private string _doorName;
        public string DoorName
        {
            get => _doorName;
            set => SetProperty(ref _doorName, value);
        }
        /// <summary>
        /// The locked status of the door
        /// </summary>
        private bool _isDoorLocked;
        public bool IsDoorLocked
        {
            get => _isDoorLocked;
            set => SetProperty(ref _isDoorLocked, value);
        }
        /// <summary>
        /// The opened status of the door
        /// </summary>
        private bool _isDoorOpened;
        public bool IsDoorOpened
        {
            get => _isDoorOpened;
            set => SetProperty(ref _isDoorOpened, value);
        }

        public static BindableDoor ToBindableDoor(Door door)
        {
            return new BindableDoor
            {
                DoorId = door.DoorId,
                DoorName = door.DoorName,
                IsDoorLocked = door.IsDoorLocked,
                IsDoorOpened = door.IsDoorOpened
            };
        }

        public static BindableDoor ToBindableDoor(CommonDoor commonDoor)
        {
            return new BindableDoor
            {
                DoorId = commonDoor.DoorId,
                DoorName = commonDoor.DoorName,
                IsDoorLocked = commonDoor.IsDoorLocked,
                IsDoorOpened = commonDoor.IsDoorOpened
            };
        }

        public static CommonDoor ToCommonDoor(Door door)
        {
            return new CommonDoor
            {
                DoorId = door.DoorId,
                DoorName = door.DoorName,
                IsDoorLocked = door.IsDoorLocked,
                IsDoorOpened = door.IsDoorOpened
            };
        }
    }
}
