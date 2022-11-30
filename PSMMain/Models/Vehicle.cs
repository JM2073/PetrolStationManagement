using System.Runtime.CompilerServices;

namespace PSMMain;

public class Vehicle
{
    public Vehicle(int id, VehicleTypes vehicleType)
    {
        Id = id;
        VehicleType = vehicleType;
    }

    public int Id { get; set; }
    public double TankLevel { get; set; }
    public int? PumpId { get; set; }
    public VehicleTypes VehicleType { get; set; }
    public FuelTypes FuelType { get; set; }
    
    public bool IsAtPump()
    {
        return PumpId != null;
    }
    
    public enum VehicleTypes
    {
        Car,
        Van,
        HGV
    }
    public enum FuelTypes
    {
        Unleaded,
        Diesel,
        LPG
    }
}