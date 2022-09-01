using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drone.Contracts
{
    public interface IMovementService
    {
        void UpdateTargetPosition(double latitude, double longitude);
    }
}
