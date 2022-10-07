namespace PSMMain;

public class Vehicle
{
    public double TankSize { get; set; }
    public VehicleType Type { get; set; }

    public enum VehicleType
    {
        Car,
    }
}