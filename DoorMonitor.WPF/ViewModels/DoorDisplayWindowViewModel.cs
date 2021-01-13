using DoorMonitor.Common;
using DoorMonitor.WPF.Commands;
using DoorMonitor.WPF.Helpers;
using DoorMonitor.WPF.Models;
using DoorMonitor.WPF.Resources;
using DoorMonitor.WPF.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DoorMonitor.WPF.ViewModels
{
    /// <summary>
    /// DoorWindow's view model
    /// </summary>
    public class DoorDisplayWindowViewModel : BindableBase
    {
        #region Private

        /// <summary>
        /// The Signal R Door Status service
        /// </summary>
        private readonly SignalRDoorStatusService _signalRService;

        /// <summary>
        /// The door service to make API calls
        /// </summary>
        private readonly DoorService _service;

        /// <summary>
        /// Page Title
        /// </summary>
        private string _title;
        /// <summary>
        /// Status messages
        /// </summary>
        private string _hudLabel;
        /// <summary>
        /// New Desk name
        /// </summary>
        private string _newDeskName;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets _title
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// Gets or sets _hudLabel
        /// </summary>
        public string HudLabel
        {
            get => _hudLabel;
            set => SetProperty(ref _hudLabel, value);
        }

        /// <summary>
        /// Gets or sets _newDeskName
        /// </summary>
        public string NewDeskName
        {
            get => _newDeskName;
            set => SetProperty(ref _newDeskName, value);
        }

        /// <summary>
        /// Collection of Doors
        /// </summary>
        public ObservableCollection<BindableDoor> Doors { get; }

        #endregion

        #region Commands

        /// <summary>
        /// Command for changing door name
        /// </summary>
        public ICommand ChangeDoorNameCommand { get; }
        /// <summary>
        /// Command for changing door open state
        /// </summary>
        public ICommand ChangeDoorOpenCommand { get; }
        /// <summary>
        /// Command for changing door lock state
        /// </summary>
        public ICommand ChangeDoorLockCommand { get; }
        /// <summary>
        /// Command to delete door
        /// </summary>
        public ICommand DeleteDoorCommand { get; }
        /// <summary>
        /// Command to add new door
        /// </summary>
        public ICommand AddDoorCommand { get; }

        #endregion

        public DoorDisplayWindowViewModel(HubConnection hubConnection, DoorService service)
        {
            _service = service;

            Title = StringResources.DoorMonitor;

            Doors = new ObservableCollection<BindableDoor>();

            _signalRService = new SignalRDoorStatusService(hubConnection);

            _signalRService.DoorStatusReceived += SignalRService_DoorStatusReceived;
            _signalRService.DoorDeleted += SignalRService_DoorDeleted;
            _signalRService.NewDoorAdded += SignalRService_NewDoorAdded;

            ChangeDoorNameCommand = new Command<BindableDoor>(async(x) => await ChangeDoorNameAction(x));
            ChangeDoorOpenCommand = new Command<BindableDoor>(async (x) => await ChangeDoorOpenAction(x));
            ChangeDoorLockCommand = new Command<BindableDoor>(async (x) => await ChangeDoorLockAction(x));
            DeleteDoorCommand = new Command<BindableDoor>(async (x) => await DeleteDoorAction(x));
            AddDoorCommand = new Command(async () => await AddDoorAction());

            var tasks = new Task[] { FetchDoors() };
            Task.WhenAll(tasks);
        }

        #region Methods
        /// <summary>
        /// Called whenever a new door is broadcasted from the SignalR Hub.
        /// </summary>
        /// <param name="commonDoor"></param>
        private void SignalRService_NewDoorAdded(CommonDoor commonDoor)
        {
            if (commonDoor != null && commonDoor.Code == 200)
            {
                Doors.Add(BindableDoor.ToBindableDoor(commonDoor));
                HudLabel = $"{StringResources.DoorAdded}: {commonDoor.DoorName}";
            }
        }

        /// <summary>
        /// Called whenever the client receives a broadcast from the SignalR Hub.
        /// </summary>
        /// <param name="commonDoor"></param>
        private void SignalRService_DoorStatusReceived(CommonDoor commonDoor)
        {
            if (commonDoor.Code != 200)
            {
                HudLabel = commonDoor.Message;
                return;
            }

            var door = Doors.FirstOrDefault(x => x.DoorId == commonDoor.DoorId);
            if (door != null)
            {
                door.DoorId = commonDoor.DoorId;
                door.DoorName = commonDoor.DoorName;
                door.IsDoorLocked = commonDoor.IsDoorLocked;
                door.IsDoorOpened = commonDoor.IsDoorOpened;
                HudLabel = commonDoor.Message;
            }
        }

        /// <summary>
        /// Removes door from the Doors collection and updates HudLabel
        /// </summary>
        /// <param name="commonDoor"></param>
        private void SignalRService_DoorDeleted(CommonDoor commonDoor)
        {
            var door = Doors.FirstOrDefault(x => x.DoorId == commonDoor.DoorId);
            if (door != null)
            {
                if (commonDoor.Code == 200)
                {
                    Doors.Remove(door);
                    HudLabel = $"{StringResources.DoorDeleted}: {commonDoor.DoorName}";

                    if (Doors.Count == 0)
                    {
                        HudLabel = StringResources.NoDoors;
                    }

                }
                else
                {
                    HudLabel = commonDoor.Message;
                }
            }
        }

        /// <summary>
        /// Populate the Doors collection and establishes connection with the SignalR hub
        /// </summary>
        /// <returns></returns>
        private async Task FetchDoors()
        {
            try
            {
                var result = await _service.GetAllDoors();
                if (result == null)
                {
                    HudLabel = StringResources.SomethingWrong;
                    return;
                }

                var doors = new List<Door>();

                await _signalRService.Connect().ContinueWith((task) =>
                {
                    if (task.Exception != null)
                    {
                        HudLabel = StringResources.HubConnectFail;
                    }
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        HudLabel = StringResources.HubConnectSuccess;
                    }
                });

                if (result.Count == 0)
                {
                    HudLabel = StringResources.NoDoors;
                    return;
                }

                HudLabel = StringResources.LoBehold;

                foreach (Door door in result)
                {
                    Doors.Add(BindableDoor.ToBindableDoor(door));
                }
            }
            catch (Exception ex)
            {
                HudLabel = StringResources.SomethingWrong;
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Add a new door and broadcast it to other clients
        /// </summary>
        /// <param name="updatedDoor"></param>
        /// <returns></returns>
        private async Task AddDoorAction()
        {
            try
            {
                string doorName = NewDeskName.Trim();
                if (doorName == "")
                {
                    HudLabel = StringResources.ProvideName;
                    return;
                }

                Door newDoor = new Door
                {
                    DoorId = 0, //New door
                    DoorName = doorName,
                    IsDoorLocked = true,
                    IsDoorOpened = false
                };

                //var result = await _service.AddNewDoor(newDoor);

                //if (result == null || result.Code != 200)
                //{
                //    HudLabel = StringResources.SomethingWrong;
                //    return;
                //}
                
                await _signalRService.AddDoorResponse(BindableDoor.ToCommonDoor(newDoor));
            }
            catch (Exception)
            {
                HudLabel = StringResources.SomethingWrong;
            }
        }

        /// <summary>
        /// Delted the door and broadcast it to other clients
        /// </summary>
        /// <param name="updatedDoor"></param>
        /// <returns></returns>
        private async Task DeleteDoorAction(BindableDoor updatedDoor)
        {
            try
            {
                //var result = await _service.DeleteDoor(updatedDoor.DoorId.ToString());

                //if (result == null || result.Code != 200)
                //{
                //    HudLabel = StringResources.SomethingWrong;
                //    return;
                //}

                await _signalRService.DeleteDoorResponse(ToCommonDoor(updatedDoor));
            }
            catch (Exception)
            {
                HudLabel = StringResources.SomethingWrong;
            }
        }

        /// <summary>
        /// Broad cast lock change to all clients via the SignalR hub.
        /// </summary>
        /// <param name="updatedDoor"></param>
        /// <returns></returns>
        private async Task ChangeDoorLockAction(BindableDoor updatedDoor)
        {
            try
            {
                if (updatedDoor.DoorName.Trim() == "")
                {
                    HudLabel = StringResources.ProvideName;
                    return;
                }

                if (updatedDoor.IsDoorOpened)
                {
                    HudLabel = StringResources.CannotLock;
                    return;
                }

                updatedDoor.IsDoorLocked = !updatedDoor.IsDoorLocked;

                CommonDoor commonDoor = ToCommonDoor(updatedDoor);
                commonDoor.StatusOperation = CommonOperations.ChangeLock;
                await _signalRService.SendDoorStatusResponse(commonDoor);

                HudLabel = StringResources.Updated + updatedDoor.DoorName;
            }
            catch (Exception)
            {
                HudLabel = StringResources.SomethingWrong;
            }
        }

        /// <summary>
        /// Broad cast open change to all clients via the SignalR hub.
        /// </summary>
        /// <param name="updatedDoor"></param>
        /// <returns></returns>
        private async Task ChangeDoorOpenAction(BindableDoor updatedDoor)
        {
            try
            {
                if (updatedDoor.DoorName.Trim() == "")
                {
                    HudLabel = StringResources.ProvideName;
                    return;
                }

                if (updatedDoor.IsDoorLocked)
                {
                    HudLabel = StringResources.CannotOpen;
                    return;
                }

                updatedDoor.IsDoorOpened = !updatedDoor.IsDoorOpened;

                CommonDoor commonDoor = ToCommonDoor(updatedDoor);
                commonDoor.StatusOperation = CommonOperations.ChangeOpen;
                await _signalRService.SendDoorStatusResponse(commonDoor);

                HudLabel = StringResources.Updated + updatedDoor.DoorName;
            }
            catch (Exception)
            {
                HudLabel = StringResources.SomethingWrong;
            }
        }

        /// <summary>
        /// Broad cast name change to all clients via the SignalR hub.
        /// </summary>
        /// <param name="updatedDoor"></param>
        /// <returns></returns>
        private async Task ChangeDoorNameAction(BindableDoor updatedDoor)
        {
            try
            {
                if (updatedDoor.DoorName.Trim() == "")
                {
                    HudLabel = StringResources.ProvideName;
                    return;
                }

                CommonDoor commonDoor = ToCommonDoor(updatedDoor);
                commonDoor.StatusOperation = CommonOperations.ChangeName;
                await _signalRService.SendDoorStatusResponse(commonDoor);

                HudLabel = StringResources.Updated + updatedDoor.DoorName;
            }
            catch (Exception)
            {
                HudLabel = StringResources.NameChangeFail;
            }
        }

        /// <summary>
        /// Converts Door to CommonDoor object
        /// </summary>
        /// <param name="door"></param>
        /// <returns></returns>
        private CommonDoor ToCommonDoor(BindableDoor door)
        {
            return new CommonDoor
            {
                DoorId = door.DoorId,
                DoorName = door.DoorName,
                IsDoorLocked = door.IsDoorLocked,
                IsDoorOpened = door.IsDoorOpened
            };
        }

        #endregion
    }
}
