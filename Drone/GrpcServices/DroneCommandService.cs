using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedMessages;
using Drone.Contracts;

namespace Drone.GrpcServices
{
    public class DroneCommandService : DroneCommands.DroneCommandsBase
    {
        private readonly IMovementService _movementService;

        public DroneCommandService(IMovementService movementService)
        {
            _movementService = movementService;
        }

        public override async Task<UpdatePositionResponse> UpdatePosition(UpdatePositionCommand request, ServerCallContext context)
        {
            if (request == null || !IsPositionValid(request.Latitude, request.Longitude))
                return new UpdatePositionResponse { Succeeded = false };

            _movementService.UpdateTargetPosition(request.Latitude, request.Longitude);

            return new UpdatePositionResponse { Succeeded = true };
        }

        private static bool IsPositionValid(double latitude, double longitude)
            => latitude >= -90 && latitude <= 90 && longitude >= -180 && longitude <= 180;
    }
}
