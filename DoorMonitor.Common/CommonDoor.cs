using System;

namespace DoorMonitor.Common
{
    /// <summary>
    /// Representation of the Door object that will be passed between client and server
    /// </summary>
    public class CommonDoor : Response
    {
        /// <summary>
        /// The Door ID
        /// </summary>
        public int DoorId { get; set; }
        /// <summary>
        /// The Door's name
        /// </summary>
        public string DoorName { get; set; }
        /// <summary>
        /// The locked status of the door
        /// </summary>
        public bool IsDoorLocked { get; set; }
        /// <summary>
        /// The opened status of the door
        /// </summary>
        public bool IsDoorOpened { get; set; }
        /// <summary>
        /// The operation performed to invoke the status update
        /// </summary>
        public CommonOperations StatusOperation { get; set; }
    }

    public class Response
    {
        /// <summary>
        /// Status code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Status message
        /// </summary>
        public string Message { get; set; }
    }
}
