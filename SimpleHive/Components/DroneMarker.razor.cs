using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using SharedMessages;
using SimpleHive.Contracts;
using System;
using System.Threading.Tasks;

namespace SimpleHive.Components
{
    public partial class DroneMarker : IAsyncDisposable
    {
        [Parameter]
        public int DroneId { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public NavigationManager NavMan { get; set; }
        [Inject]
        public DroneCommands.DroneCommandsClient Client { get; set; }

        private HubConnection _hubConnection;
        private double? _latitude;
        private double? _longitude;

        protected override async Task OnInitializedAsync()
        {
            await SetupHubConnection();
        }

        private async Task SetupHubConnection()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(NavMan.ToAbsoluteUri(Hubs.TelemetryHub.Route), Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets)
                .WithAutomaticReconnect().Build();
            _hubConnection.On<int, double, double>(nameof(ITelemetryHubClient.UpdatePosition), async (droneId, latitude, longitude) =>
            {
                _latitude = latitude;
                _longitude = longitude;
                await JSRuntime.InvokeVoidAsync("window.leafletMap.addOrUpdateMarker", droneId, latitude, longitude, DotNetObjectReference.Create(this));

                StateHasChanged();
            });

            await _hubConnection.StartAsync();
        }

        [JSInvokable]
        public async Task NotifyDragEnd(double lat, double lng)
        {
            await Client.UpdatePositionAsync(new UpdatePositionCommand { Latitude = lat, Longitude = lng });
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
                await _hubConnection.DisposeAsync();
        }
    }
}
