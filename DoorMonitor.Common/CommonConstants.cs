using System;
using System.Collections.Generic;
using System.Text;

namespace DoorMonitor.Common
{
    /// <summary>
    /// A static class which contains common constants used in both WPF and SignalR projects 
    /// </summary>
    public static class CommonConstants
    {
        /// <summary>
        /// ReceivedDoorResponse
        /// </summary>
        public static string ReceivedDoorResponse { get; } = "ReceivedDoorResponse";
        /// <summary>
        /// DeleteDoorResponse
        /// </summary>
        public static string DeleteDoorResponse { get; } = "DeleteDoorResponse";
        /// <summary>
        /// DeleteDoorResponse
        /// </summary>
        public static string AddNewDoorResponse { get; } = "AddNewDoorResponse";
        /// <summary>
        /// /doorStatus
        /// </summary>
        public static string DoorHubPath { get; } = "/doorStatus";
    }
}
