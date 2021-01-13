using DoorMonitor.Common;
using DoorMonitor.WPF.Models;
using DoorMonitor.WPF.Resources;
using DoorMonitor.WPF.Services;
using DoorMonitor.WPF.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;
using System.Windows.Controls;

namespace DoorMonitor.WPF.Views
{
    /// <summary>
    /// Interaction logic for DoorDisplayWindow.xaml
    /// </summary>
    public partial class DoorDisplayWindow : Window
    {
        private readonly DoorDisplayWindowViewModel _vm;
        public DoorDisplayWindow()
        {
            InitializeComponent();
            RequestProvider requestProvider = new RequestProvider();
            DoorService service = new DoorService(requestProvider);

            HubConnection hubConnection = new HubConnectionBuilder()
                .WithUrl(GlobalConstants.LocalEndPoint + CommonConstants.DoorHubPath)
                .Build();

            _vm = new DoorDisplayWindowViewModel(hubConnection, service);
            DataContext = _vm;
        }

        private void changeNameBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var door = btn.DataContext as BindableDoor;
            _vm.ChangeDoorNameCommand.Execute(door);
        }

        private void changeOpenBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var door = btn.DataContext as BindableDoor;
            _vm.ChangeDoorOpenCommand.Execute(door);
        }

        private void changeLockBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var door = btn.DataContext as BindableDoor;
            _vm.ChangeDoorLockCommand.Execute(door);
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var door = btn.DataContext as BindableDoor;
            _vm.DeleteDoorCommand.Execute(door);
        }
    }
}
