using System.Timers;
using Timer = System.Timers.Timer;

namespace PSMMain
{
    class Program
    {
        private static int _waitingCars;
        private static List<Pump> pumps = new();

        ///set a timer that works every 1.5 seconds.
        private static Timer _carSpawner = new Timer(1500);

        private static Timer _fuleTimer = new Timer(8000);

        public static void Main(string[] args)
        {
            InitalStartup(9);
            var onShift = true;
            
            
            //can do login here.

            while (onShift)
            {

                // CarGenrater();

                // Console.WriteLine("there are {}");
                Console.ReadKey();
            }
        }


        
        /// <summary>
        /// Sets all 9 of the pumps up.
        /// </summary>
        private static void InitalStartup(int numOfPumps)
        {
            for (var i = 1; i <= numOfPumps; i++)
            {
                pumps.Add(new Pump(i, false, 0.00));
            }
        }

        /// <summary>
        /// Starts the timer that creates cars.
        /// </summary>
        private static void CarGenrater()
        {
            _carSpawner.Elapsed += CarSpawnerOnElapsed;
            _carSpawner.Enabled = true;
            _carSpawner.AutoReset = true;
            _carSpawner.Start();
        }

        private static void StartPumping()
        {
            _fuleTimer.Elapsed += FuleTimerOnElapsed;
            _fuleTimer.Enabled = true;
            _fuleTimer.AutoReset = false;
            _fuleTimer.Start();
        }

        private static void FuleTimerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            
        }

        private static void CarSpawnerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            _waitingCars++;
        }
    }
}