using System.Runtime.CompilerServices;

namespace PSMMain;

public class Vehicle
{
    public Vehicle(int id,  double tankLevel, CustomMethods.VehicleTypes vehicleType)
    {
        Id = id;
        TankLevel = tankLevel;
        VehicleType = vehicleType;
    }

    public int Id { get; set; }
    public double TankLevel { get; set; }
    public int? PumpId { get; set; }
    public CustomMethods.VehicleTypes VehicleType { get; set; }
    public CustomMethods.FuelTypes FuelType { get; set; }
    
    public bool IsAtPump()
    {
        return PumpId != null;
    }
}