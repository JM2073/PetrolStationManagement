namespace PSMMain;

public class Pump
{
    public Pump(int id, bool currenttlyActive, double fuleDespenced)
    {
        Id = id;
        CurrenttlyActive = currenttlyActive;
        FuleDespenced = fuleDespenced;
    }

    public int Id { get; set; }
    public bool CurrenttlyActive { get; set; }
    public double FuleDespenced { get; set; }
}