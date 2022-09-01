using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleHive.Contracts
{
    public interface ITelemetryHubClient
    {
        Task UpdatePosition(int droneId, double latitude, double longitude);
    }
}
