// See https://aka.ms/new-console-template for more information
using System.Text;
using Geolocation;
using System.Diagnostics;
using NearestVehiclePositions;

string path = @"..\\VehiclePositions.dat";
Stream stream = File.Open(path, FileMode.Open);

BinaryReader reader = new BinaryReader(stream);
var result = new List<BinaryDataObject>();
while (stream.Position != stream.Length)
{
    var currentVehicle = new BinaryDataObject();

    currentVehicle.PositionId = reader.ReadInt32();

    //binary reader cannot read ASCII string that. Looping untill null terminating char 
    var currentRegString = new StringBuilder();
    while (true)
    {
        byte b = reader.ReadByte();
        if (0 == b)
            break;
        currentRegString.Append((char)b);
    }
    currentVehicle.VehicleRegistration = currentRegString.ToString();

    currentVehicle.Latitude = reader.ReadSingle();
    currentVehicle.Longitude = reader.ReadSingle();
    currentVehicle.RecordedTimeUTC = DateTime.FromBinary((long)reader.ReadUInt64());

    currentVehicle.Coordinates = new Coordinate()
    {
        Latitude = currentVehicle.Latitude,
        Longitude = currentVehicle.Longitude,
    };

    //geocal();

    result.Add(currentVehicle);
}
reader.Close();

var givenPositions = new List<Coordinate>();
givenPositions.Add(new Coordinate { Latitude = 34.544909, Longitude = -102.100843 });
givenPositions.Add(new Coordinate { Latitude = 32.345544, Longitude = -99.123124 });
givenPositions.Add(new Coordinate { Latitude = 33.234235, Longitude = -100.214124 });
givenPositions.Add(new Coordinate { Latitude = 35.195739, Longitude = -95.348899 });
givenPositions.Add(new Coordinate { Latitude = 31.895839, Longitude = -97.789573 });
givenPositions.Add(new Coordinate { Latitude = 32.895839, Longitude = -101.789573 });
givenPositions.Add(new Coordinate { Latitude = 34.115839, Longitude = -100.225732 });
givenPositions.Add(new Coordinate { Latitude = 32.335839, Longitude = -99.992232 });
givenPositions.Add(new Coordinate { Latitude = 33.535339, Longitude = -94.792232 });
givenPositions.Add(new Coordinate { Latitude = 32.234235, Longitude = -100.222222 });


ClosestDistance[] closestDistanceList = new ClosestDistance[10];


foreach (var position in result)
{
    foreach (var (givenPostion, index) in givenPositions.Select((value, i) => (value, i)))
    {
        var currentDistance = GeoCalculator.GetDistance(position.Coordinates, givenPostion);

        if (closestDistanceList[index].Distance >= 0)
        {
            if (currentDistance < closestDistanceList[index].Distance || closestDistanceList[index].Distance == 0)
            {
                closestDistanceList[index].PostionId = position.PositionId;
                closestDistanceList[index].Distance = currentDistance;
            }
        }
    }
}

var counter = 1;
foreach (var location in closestDistanceList)
{
    Console.WriteLine("Location " + counter);
    Console.WriteLine("PostionId: " + location.PostionId);
    Console.WriteLine("Vehicle Registration: " + result.First(x => x.PositionId == location.PostionId).VehicleRegistration);
    Console.WriteLine("Distance: " + location.Distance);

    counter++;
}
