namespace PSMMain.Models;

public class Pump
{
    public Pump(int id, bool currentlyActive, double unloadedFuelDescended, double dieselFuelDescended,
        double lpgFuelDescended)
    {
        Id = id;
        CurrentlyActive = currentlyActive;
        UnloadedFuelDescended = unloadedFuelDescended;
        DieselFuelDescended = dieselFuelDescended;
        LpgFuelDescended = lpgFuelDescended;
    }
    public int Id { get; set; }
    public bool CurrentlyActive { get; set; }
    public double UnloadedFuelDescended { get; set; }
    public double DieselFuelDescended { get; set; }
    public double LpgFuelDescended { get; set; }

}