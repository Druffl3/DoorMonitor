using DoorMonitor.Common;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace DoorMonitor.WPF.Services
{
    /// <summary>
    /// Provides Event handlers and callback methods that communicate with the DoorService SignalR hub.
    /// </summary>
    public class SignalRDoorStatusService
    {
        /// <summary>
        /// The hub connection instance
        /// </summary>
        private readonly HubConnection _connection;

        /// <summary>
        /// When there's a door update
        /// </summary>
        public event Action<CommonDoor> DoorStatusReceived;
        /// <summary>
        /// When new door is added
        /// </summary>
        public event Action<CommonDoor> NewDoorAdded;
        /// <summary>
        /// When existing door is deleted
        /// </summary>
        public event Action<CommonDoor> DoorDeleted;

        public SignalRDoorStatusService(HubConnection connection)
        {
            _connection = connection;

            _connection.On<CommonDoor>(CommonConstants.ReceivedDoorResponse, (door) =>
            {
                DoorStatusReceived?.Invoke(door);
            });
            
            _connection.On<CommonDoor>(CommonConstants.AddNewDoorResponse, (door) =>
            {
                NewDoorAdded?.Invoke(door);
            });
            
            _connection.On<CommonDoor>(CommonConstants.DeleteDoorResponse, (door) =>
            {
                DoorDeleted?.Invoke(door);
            });
        }

        /// <summary>
        /// Establish connection to the hub.
        /// </summary>
        /// <returns></returns>
        public async Task Connect()
        {
            await _connection.StartAsync();
        }

        /// <summary>
        /// Update in database and Broadcast door update
        /// </summary>
        /// <param name="commonDoor"></param>
        /// <returns></returns>
        public async Task SendDoorStatusResponse(CommonDoor commonDoor)
        {
            await _connection.SendAsync("SendDoorStatusResponse", commonDoor);
        }

        /// <summary>
        /// Add door in database and Broadcast door update
        /// </summary>
        /// <param name="commonDoor"></param>
        /// <returns></returns>
        public async Task AddDoorResponse(CommonDoor commonDoor)
        {
            await _connection.SendAsync("AddDoorResponse", commonDoor);
        }

        /// <summary>
        /// Delete door from database and broadcast door update
        /// </summary>
        /// <param name="commonDoor"></param>
        /// <returns></returns>
        public async Task DeleteDoorResponse(CommonDoor commonDoor)
        {
            await _connection.SendAsync("DeleteDoorResponse", commonDoor);
        }
    }
}
