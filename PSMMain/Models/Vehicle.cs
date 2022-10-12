namespace PSMMain;

public class Vehicle
{
    public Vehicle(double tankSize, VehicleType type, int? pumpId, bool isAtPump)
    {
        TankSize = tankSize;
        Type = type;
        PumpId = pumpId;
        IsAtPump = isAtPump;
    }

    public double TankSize { get; set; }
    
    public bool IsAtPump { get; set; }
    public int? PumpId { get; set; }
    public VehicleType Type { get; set; }

    public enum VehicleType
    {
        Car,
    }
}