using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using SharedMessages;
using SimpleHive.Contracts;
using SimpleHive.Hubs;

namespace SimpleHive.GrpcServices
{
    public class DroneTelemetryService : DroneTelemetry.DroneTelemetryBase
    {
        private readonly IHubContext<TelemetryHub, ITelemetryHubClient> _hub;

        public DroneTelemetryService(IHubContext<TelemetryHub, ITelemetryHubClient> hub)
        {
            _hub = hub;
        }

        public override async Task<PositionUpdatedResponse> PositionUpdated(PositionUpdatedCommand request, ServerCallContext context)
        {
            if (request == null || !IsPositionValid(request.Latitude, request.Longitude))
                return new PositionUpdatedResponse { Succeeded = false };

            await _hub.Clients.All.UpdatePosition(request.DroneId, request.Latitude, request.Longitude);

            return new PositionUpdatedResponse { Succeeded = true };
        }

        public override async Task<BatteryLevelUpdatedResponse> BatteryLevelUpdated(BatteryLevelUpdatedCommand request, ServerCallContext context)
        {
            // TODO: Implement Battery Level

            return new BatteryLevelUpdatedResponse { Succeeded = false };
        }

        private static bool IsPositionValid(double latitude, double longitude)
            => latitude >= -90 && latitude <= 90 && longitude >= -180 && longitude <= 180;
    }
}
