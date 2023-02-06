using Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NearestVehiclePositions
{
    public struct BinaryDataObject
    {
        public int PositionId { get; set; }
        public string VehicleRegistration { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime RecordedTimeUTC { get; set; }
        public Coordinate Coordinates { get; set; }

    }

    public struct ClosestDistance
    {
        public int PostionId { get; set; }
        public double Distance { get; set; }
    }
}
