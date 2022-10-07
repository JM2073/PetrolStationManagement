using System.Timers;
using Timer = System.Timers.Timer;

namespace PSMMain
{
    class Program
    {
        private static int _waitingCars;
        private static Timer _carSpawner = new(1500);
        private static Timer _fuleTimer = new();
        private static List<Pump> pumps = new();

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

        public static void DrawForcourt(List<Pump> pumps)
        {
            Console.Clear();
            Console.Write($"1,{(pumps.Single(x => x.Id == 1).CurrenttlyActive ? "Inuse" : "Available")} -------  ");
            Console.Write($"2,{(pumps.Single(x => x.Id == 2).CurrenttlyActive ? "Inuse" : "Available")} -------  ");
            Console.Write($"3,{(pumps.Single(x => x.Id == 3).CurrenttlyActive ? "Inuse" : "Available")} -------  ");
            Console.WriteLine();
            Console.Write($"4,{(pumps.Single(x => x.Id == 4).CurrenttlyActive ? "Inuse" : "Available")} -------  ");
            Console.Write($"5,{(pumps.Single(x => x.Id == 5).CurrenttlyActive ? "Inuse" : "Available")} -------  ");
            Console.Write($"6,{(pumps.Single(x => x.Id == 6).CurrenttlyActive ? "Inuse" : "Available")} -------  ");
            Console.WriteLine();
            Console.Write($"7,{(pumps.Single(x => x.Id == 7).CurrenttlyActive ? "Inuse" : "Available")} -------  ");
            Console.Write($"8,{(pumps.Single(x => x.Id == 8).CurrenttlyActive ? "Inuse" : "Available")} -------  ");
            Console.Write($"9,{(pumps.Single(x => x.Id == 9).CurrenttlyActive ? "Inuse" : "Available")} -------  ");
            Console.WriteLine();
        }

        
        /// <summary>
        /// Sets all 9 of the pumps up.
        /// starts the timer for car generation.
        /// </summary>
        private static void InitalStartup(int numOfPumps)
        {
            for (var i = 1; i <= numOfPumps; i++)
            {
                pumps.Add(new Pump(i, false, 0.00));
            }

            DrawForcourt(pumps);
            CarGenrater();
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

        private static void StartPumping(TimeSpan interval)
        {
            _fuleTimer.Elapsed += FuleTimerOnElapsed;
            _fuleTimer.Interval = interval.TotalMilliseconds;
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