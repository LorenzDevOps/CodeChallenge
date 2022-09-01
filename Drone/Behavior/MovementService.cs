using Drone.Contracts;
using Drone.Helpers;
using Drone.Options;
using Microsoft.Extensions.Options;
using SharedMessages;
using System;
using System.Threading;

namespace Drone.Behavior
{
    public class MovementService : IMovementService
    {
        private (double, double) _currentPosition;
        private (double, double)? _targetPosition;

        private readonly DroneTelemetry.DroneTelemetryClient _client;
        private readonly DroneOptions _options;

        private readonly int _velocity = 5; // meters per second
        private readonly int _updateFrequency = 1; // seconds

        public MovementService(DroneTelemetry.DroneTelemetryClient client, IOptions<DroneOptions> options)
        {
            _client = client ?? throw new ArgumentNullException(paramName: nameof(client));
            _options = options?.Value ?? throw new ArgumentNullException(paramName: nameof(options));

            _currentPosition = (55.353267, 10.407906);
            _ = new Timer(UpdateMovement, null, 0, _updateFrequency * 1000);
        }

        public void UpdateTargetPosition(double latitude, double longitude) => _targetPosition = (latitude, longitude);

        private void UpdateMovement(object? state) 
        {
            if (_targetPosition.HasValue)
                _currentPosition = CoordinateCalculator.CalculateStepCoordinates(_currentPosition, _targetPosition.Value, _velocity);

            var command = new PositionUpdatedCommand { DroneId = _options.DroneId, Latitude = _currentPosition.Item1, Longitude = _currentPosition.Item2 };
            _client.PositionUpdated(command);
        }
    }
}
