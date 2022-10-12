namespace PSMMain;

public class Pump
{
    public Pump(int id, bool currentlyActive, double fuelDescended)
    {
        Id = id;
        CurrentlyActive = currentlyActive;
        FuelDescended = fuelDescended;
    }

    public int Id { get; set; }
    public bool CurrentlyActive { get; set; }
    public double FuelDescended { get; set; }
}