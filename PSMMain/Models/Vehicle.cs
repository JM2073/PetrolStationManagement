using System.Runtime.CompilerServices;

namespace PSMMain;

public class Vehicle
{
    public Vehicle(int id,  double tankSize, CustomMethods.VehicleTypes vehicleType, CustomMethods.FuelTypes fuelType)
    {
        Id = id;
        TankSize = tankSize;
        VehicleType = vehicleType;
        FuelType = fuelType;
    }

    public int Id { get; set; }
    public double TankSize { get; set; }
    public int? PumpId { get; set; }
    public CustomMethods.VehicleTypes VehicleType { get; set; }
    public CustomMethods.FuelTypes FuelType { get; set; }
    
    public bool IsAtPump()
    {
        return PumpId != null;
    }
}