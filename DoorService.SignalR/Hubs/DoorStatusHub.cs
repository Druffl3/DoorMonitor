using DoorMonitor.Common;
using DoorService.SignalR.Contexts;
using DoorService.SignalR.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DoorService.SignalR.Hubs
{
    public class DoorStatusHub : Hub
    {
        /// <summary>
        /// The Door database context
        /// </summary>
        private readonly DoorDbContext _context;

        public DoorStatusHub(DoorDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method is triggered whenever a SendAsync request is made on it from the client.
        /// </summary>
        /// <param name="commonDoor"></param>
        /// <returns></returns>
        public async Task SendDoorStatusResponse(CommonDoor commonDoor)
        {
            try
            {
                var door = _context.DoorTable.FindAsync(commonDoor.DoorId);
                if (door.Result == null)
                {
                    commonDoor.Code = StatusCodes.Status400BadRequest;
                    commonDoor.Message = "The door is not available";
                }

                string message = "updated";
                if (commonDoor.StatusOperation == CommonOperations.ChangeOpen)
                {
                    door.Result.IsDoorOpened = commonDoor.IsDoorOpened;
                    message = commonDoor.IsDoorOpened ? "Opened" : "Closed";
                }
                else if (commonDoor.StatusOperation == CommonOperations.ChangeLock)
                {
                    door.Result.IsDoorLocked = commonDoor.IsDoorLocked;
                    message = commonDoor.IsDoorLocked ? "Locked" : "Unlocked";
                }
                else 
                {
                    door.Result.DoorName = commonDoor.DoorName;
                }

                await _context.SaveChangesAsync();

                commonDoor.Code = StatusCodes.Status200OK;
                commonDoor.Message = $"Door,{commonDoor.DoorName} {message}";
            }
            catch (Exception ex)
            {
                commonDoor.Code = StatusCodes.Status500InternalServerError;
                commonDoor.Message = $"Internal Server Error:\n{ex.Message}";
            }

            await Clients.All.SendAsync(CommonConstants.ReceivedDoorResponse, commonDoor);
        }

        /// <summary>
        /// Add the new door to the database and broadcast the same
        /// </summary>
        /// <param name="newDoor"></param>
        /// <returns></returns>
        public async Task AddDoorResponse(CommonDoor newDoor)
        {
            try
            {
                var door = ToDoor(newDoor);
                await _context.AddAsync(door);
                await _context.SaveChangesAsync();
                var lastDoor = _context.DoorTable.OrderByDescending(x => x.DoorId)
                         .FirstOrDefault();

                if (lastDoor != null)
                {
                    newDoor.Code = StatusCodes.Status200OK;
                    newDoor.Message = "Door added succeffully";
                    newDoor.DoorId = lastDoor.DoorId;
                }
            }
            catch (Exception ex)
            {
                newDoor.Code = StatusCodes.Status500InternalServerError;
                newDoor.Message = $"Internal Server Error:\n{ex.Message}";
            }

            await Clients.All.SendAsync(CommonConstants.AddNewDoorResponse, newDoor);
        }

        /// <summary>
        /// Delete the door from the database and broadcast the same
        /// </summary>
        /// <param name="commonDoor"></param>
        /// <returns></returns>
        public async Task DeleteDoorResponse(CommonDoor commonDoor)
        {
            try
            {
                var door = _context.DoorTable.FindAsync(commonDoor.DoorId);
                if (door.Result == null)
                {
                    commonDoor.Code = StatusCodes.Status400BadRequest;
                    commonDoor.Message = "The door is not available";
                }

                _context.Remove(door.Result);
                await _context.SaveChangesAsync();

                commonDoor.Code = StatusCodes.Status200OK;
                commonDoor.Message = $"Door {door.Result.DoorName} removed successfully";
            }
            catch (Exception ex)
            {
                commonDoor.Code = StatusCodes.Status500InternalServerError;
                commonDoor.Message = $"Internal Server Error:\n{ex.Message}";
            }

            await Clients.All.SendAsync(CommonConstants.DeleteDoorResponse, commonDoor);
        }


        /// <summary>
        /// Converts a CommonDoor object to a Door object
        /// </summary>
        /// <param name="door"></param>
        /// <returns></returns>
        public static Door ToDoor(CommonDoor door)
        {
            return new Door
            {
                IsDoorLocked = door.IsDoorLocked,
                DoorId = 0, //New door
                DoorName = door.DoorName,
                IsDoorOpened = door.IsDoorOpened
            };
        }

    }
}
