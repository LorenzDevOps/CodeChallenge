using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drone.Helpers
{
    public static class CoordinateCalculator
    {
        public static (double, double) CalculateStepCoordinates((double, double) origin, (double, double) target, int stepInMeters)
        {
            var metersPerDegree = 111139;

            // Calculate distance from origin to target in meters
            var latitudeDiff = target.Item1 - origin.Item1;
            var longitudeDiff = target.Item2 - origin.Item2;
            var distanceToTargetInDegrees = Math.Sqrt(Math.Pow(latitudeDiff, 2) + Math.Pow(longitudeDiff, 2));
            var distanceToTargetInMeters = distanceToTargetInDegrees * metersPerDegree;

            // Check step is exceeding target
            if (stepInMeters >= distanceToTargetInMeters)
                return target;

            var ratio = stepInMeters / distanceToTargetInMeters;
            var stepLatitude = latitudeDiff * ratio;
            var stepLongitude = longitudeDiff * ratio;
            
            return (origin.Item1 + stepLatitude, origin.Item2 + stepLongitude);
        }
    }
}
