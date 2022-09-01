using Microsoft.AspNetCore.SignalR;
using SimpleHive.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleHive.Hubs
{
    public class TelemetryHub : Hub<ITelemetryHubClient>
    {
        public static readonly string Route = "hub/telemetry";

        public Task DronePositionTelemetry(int droneId, double latitude, double longitude) 
            => Clients.All.UpdatePosition(droneId, latitude, longitude);
    }
}
