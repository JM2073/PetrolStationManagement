using System.Timers;
using Timer = System.Timers.Timer;

namespace PSMMain
{
    public class CustomTimer : System.Timers.Timer
    {
        public int pumpId { get; set; }
    }

    class Program
    {
        private static int _waitingCars;
        private static int _servedCars;
        private static Timer _carSpawner = new(1500);
        private static List<Pump> pumps = new();

        public static void Main(string[] args)
        {
            InitalStartup(9);
            var onShift = true;


            //can do login here.

            while (onShift)
            {
                DrawForcourt(cleanDraw: true);
                if (_waitingCars > 0)
                {
                    Console.WriteLine("you have cars in the quee, please select an available pump.");
                    Console.Write(":");

                    var chois = CustomMethods.ParseStringToInt(Console.ReadLine(),
                        "please make sure you select a valid pump.");

                    if (pumps.Where(x => x.CurrenttlyActive == false).Any(x => x.Id == chois))
                    {
                        StartPumping(TimeSpan.FromSeconds(8), chois);
                    }
                    else
                    {
                        Console.WriteLine("please make sure the pump is available.");
                        Console.WriteLine("press any key to try again.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    while (_waitingCars == 0)
                    {
                        Console.WriteLine("waiting for a car.");
                    }
                }
            }

            Console.ReadKey();
        }

        public static void DrawForcourt(List<Pump> pumps)
        {
            Console.Clear();
            DrawForcourt();
        }
        
        public static void DrawForcourt()
        {
            Console.SetCursorPosition(0, 0);

            Console.WriteLine("Queue");
            Console.WriteLine($"           Cars: {_waitingCars}");
            Console.WriteLine();
            Console.Write($"1,{(pumps.Single(x => x.Id == 1).CurrenttlyActive ? "Busy" : "Avail")} -------  ");
            Console.Write($"2,{(pumps.Single(x => x.Id == 2).CurrenttlyActive ? "Busy" : "Avail")} -------  ");
            Console.Write($"3,{(pumps.Single(x => x.Id == 3).CurrenttlyActive ? "Busy" : "Avail")} -------  ");
            Console.WriteLine();
            Console.Write($"4,{(pumps.Single(x => x.Id == 4).CurrenttlyActive ? "Busy" : "Avail")} -------  ");
            Console.Write($"5,{(pumps.Single(x => x.Id == 5).CurrenttlyActive ? "Busy" : "Avail")} -------  ");
            Console.Write($"6,{(pumps.Single(x => x.Id == 6).CurrenttlyActive ? "Busy" : "Avail")} -------  ");
            Console.WriteLine();
            Console.Write($"7,{(pumps.Single(x => x.Id == 7).CurrenttlyActive ? "Busy" : "Avail")} -------  ");
            Console.Write($"8,{(pumps.Single(x => x.Id == 8).CurrenttlyActive ? "Busy" : "Avail")} -------  ");
            Console.Write($"9,{(pumps.Single(x => x.Id == 9).CurrenttlyActive ? "Busy" : "Avail")} -------  ");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"total Fuel Pumped = {pumps.Sum(pump => pump.FuleDespenced)}");
            Console.WriteLine($"total served cars = {_servedCars}");
            Console.WriteLine();
            Console.WriteLine();
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

            DrawForcourt();
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
            CustomTimer _fuleTimer = new();
            _waitingCars--;

            pumps.Single(x => x.Id == pumpId).CurrenttlyActive = true;
            _fuleTimer.Elapsed += FuleTimerOnElapsed;
            _fuleTimer.Interval = interval.TotalMilliseconds;
            _fuleTimer.Enabled = true;
            _fuleTimer.AutoReset = false;
            _fuleTimer.pumpId = pumpId;
            _fuleTimer.Start();
        }

        private static void FuleTimerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            int leftPos = Console.CursorLeft, topPos = Console.CursorTop;
            _servedCars++;

            var pump = pumps.Single(x => x.Id == ((CustomTimer)sender).pumpId);
            pump.CurrenttlyActive = false;
            pump.FuleDespenced += (((CustomTimer)sender).Interval / 1000) * 1.5;

            ((CustomTimer)sender).Dispose();
            DrawForcourt();
            Console.SetCursorPosition(leftPos, topPos);
        }

        private static void CarSpawnerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            int leftPos = Console.CursorLeft, topPos = Console.CursorTop;
            DrawForcourt();
            _waitingCars++;
            Console.SetCursorPosition(leftPos, topPos);
        }
    }
}