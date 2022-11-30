namespace PSMMain;

public class CustomTimer : System.Timers.Timer
{
        public CustomTimer(double interval): base(interval)
        {
                
        }

        public int? PumpId { get; set; }
        public int? CarId { get; set; }
}