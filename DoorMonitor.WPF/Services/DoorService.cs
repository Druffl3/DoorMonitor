using DoorMonitor.Common;
using DoorMonitor.WPF.Models;
using DoorMonitor.WPF.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoorMonitor.WPF.Services
{
    /// <summary>
    /// Provides service methods to make API calls on the DoorService server
    /// </summary>
    public class DoorService
    {
        /// <summary>
        /// The request provider
        /// </summary>
        private readonly RequestProvider _requestProvider;

        private readonly static string _feature = GlobalConstants.ApiPath + "/doors";

        #region APIs
        /// <summary>
        /// /getAllDoors
        /// </summary>
        private readonly string getAllDoors = _feature + "/getAllDoors";
        /// <summary>
        /// /addNewDoor
        /// </summary>
        private readonly string addNewDoor = _feature + "/addNewDoor";
        /// <summary>
        /// /deleteDoor
        /// </summary>
        private readonly string deleteDoor = _feature + "/deleteDoor";
        /// <summary>
        /// /changeDoorLockState
        /// </summary>
        private readonly string changeDoorLockState = _feature + "/changeDoorLockState";
        /// <summary>
        /// /changeDoorOpenState
        /// </summary>
        private readonly string changeDoorOpenState = _feature + "/changeDoorOpenState";
        /// <summary>
        /// /changeDoorName
        /// </summary>
        private readonly string changeDoorName = _feature + "/changeDoorName";

        #endregion

        public DoorService(RequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        #region Methods
        /// <summary>
        /// Fetches all the doors from the server
        /// </summary>
        /// <returns>All doors as list or error response</returns>
        public async Task<List<Door>> GetAllDoors()
        {
            var builder = new UriBuilder(GlobalConstants.LocalEndPoint)
            {
                Path = getAllDoors
            };
            var uri = builder.ToString();
            var result = await _requestProvider.GetAsync<List<Door>>(uri);
            return result;
        }

        /// <summary>
        /// Adds a new Door to the server
        /// </summary>
        /// <returns>API Status Response</returns>
        public async Task<Door> AddNewDoor(Door data)
        {
            var builder = new UriBuilder(GlobalConstants.LocalEndPoint)
            {
                Path = addNewDoor
            };
            var uri = builder.ToString();
            var result = await _requestProvider.PostAsync<Door>(uri, data);
            return result;
        }

        /// <summary>
        /// Deletes the door matching the Id passed from the server
        /// </summary>
        /// <returns>API Status Response</returns>
        public async Task<Door> DeleteDoor(string doorId)
        {
            var builder = new UriBuilder(GlobalConstants.LocalEndPoint)
            {
                Path = deleteDoor
            };
            var uri = builder.ToString() + $"?doorId={doorId}";
            var result = await _requestProvider.DeleteAsync<Door>(uri);
            return result;
        }

        /// <summary>
        /// Updates the door's open state in the server
        /// </summary>
        /// <returns>API Status Response</returns>
        public async Task<Door> UpdateDoor(Door data, CommonOperations doorOperations)
        {
            string path = changeDoorName;
            if (doorOperations == CommonOperations.ChangeOpen)
                path = changeDoorOpenState;
            else if (doorOperations == CommonOperations.ChangeLock)
                path = changeDoorLockState;

            var builder = new UriBuilder(GlobalConstants.LocalEndPoint)
            {
                Path = changeDoorOpenState
            };
            var uri = builder.ToString();
            var result = await _requestProvider.PutAsync<Door>(uri, data);
            return result;
        }

        #endregion
    }
}
