using Drone.Options;
using Microsoft.Extensions.Options;
using SharedMessages;
using System;
using System.Threading;

namespace Drone.Behavior
{
    public class BatteryService
    {
        private int _currentBatteryLevel;
        private DateTimeOffset _lastUpdated;

        private readonly DroneTelemetry.DroneTelemetryClient _client;
        private readonly DroneOptions _options;

        private readonly int _updateFrequency = 1; // seconds
        private readonly int _batteryDischargeInterval = 5; // seconds

        public BatteryService(DroneTelemetry.DroneTelemetryClient client, IOptions<DroneOptions> options)
        {
            _client = client ?? throw new ArgumentNullException(paramName: nameof(client));
            _options = options?.Value ?? throw new ArgumentNullException(paramName: nameof(options));

            _currentBatteryLevel = 100;
            _lastUpdated = DateTimeOffset.Now;

            _ = new Timer(UpdateBattery, null, 0, _updateFrequency * 1000);
        }

        private void UpdateBattery(object? state) 
        {
            if (_currentBatteryLevel > 0 && _lastUpdated.AddSeconds(_batteryDischargeInterval) <= DateTimeOffset.Now)
            {
                _currentBatteryLevel--;
                _lastUpdated = DateTimeOffset.Now;
            }

            var command = new BatteryLevelUpdatedCommand { DroneId = _options.DroneId, BatteryLevel = _currentBatteryLevel };
            _client.BatteryLevelUpdated(command);
        }
    }
}
