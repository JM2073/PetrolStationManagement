using System.Runtime.CompilerServices;
using System.Timers;
using Timer = System.Timers.Timer;

namespace PSMMain
{
    public class CustomTimer : Timer
    {
        public int PumpId { get; set; }
    }

    class Program
    {
        private static int _servedCars;
        private static Timer _carSpawner = new(1500);
        private static List<Pump> _pumps = new();
        private static List<Vehicle> _vehicles = new();

        public static void Main(string[] args)
        {
            InitialStartup(9);
            var onShift = true;

            while (onShift)
            {
                if (_vehicles.Any(x => x.IsAtPump == false))
                {
                    Console.SetCursorPosition(0, 14);
                    Console.WriteLine("you have cars in the queue, please select an available pump.");

                    var choice = CustomMethods.ParseStringToInt(Console.ReadLine(),
                        "please make sure you select a valid pump.");

                    if (_pumps.Where(x => x.CurrentlyActive == false).Any(x => x.Id == choice))
                    {
                        StartPumping(TimeSpan.FromSeconds(8), choice);
                    }
                    else
                    {
                        
                        Console.SetCursorPosition(0, 18);
                        Console.WriteLine("please make sure the pump is available.");
                        Console.SetCursorPosition(0, 19);
                        Console.WriteLine("press any key to try again.");
                        Console.SetCursorPosition(0, 20);
                        Console.ReadKey();
                    }
                }
                else
                {
                    // if there are no objects wait for objects. if there are objects but they are all busy wait till a free object
                    if (_vehicles.Count == 0)
                    {
                        while (_vehicles.Count == 0)
                        {
                            //TODO placeholder, replace with something like a waiting bar
                        }
                    }
                    else
                    {
                        while (_vehicles.Count(x => x.IsAtPump) == _vehicles.Count)
                        {
                            //TODO placeholder, replace with something like a waiting bar
                        }
                    }
                }
                Console.SetCursorPosition(0, 15);
                CustomMethods.ClearCurrentConsoleLine();
            }
            Console.ReadKey();
        }


        private static void InitialStartup(int numOfPumps)
        {
            for (var i = 1; i <= numOfPumps; i++)
                _pumps.Add(new Pump(i, false, 0.00));

            _pumps = _pumps.ToList();

            CarGenerator();
            DrawForecourt();
        }


        private static void DrawForecourt()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Queue");
            Console.SetCursorPosition(0, 1);
            Console.WriteLine($"           Cars: {_vehicles.Count(x => x.PumpId == null)}");
            Console.SetCursorPosition(0, 2);
            Console.WriteLine();
            Console.SetCursorPosition(0, 3);
            Console.Write($"1,{(_pumps.Single(x => x.Id == 1).CurrentlyActive ? "BUSY " : "AVAIL")} -------  2,{(_pumps.Single(x => x.Id == 2).CurrentlyActive ? "BUSY " : "AVAIL")} -------  3,{(_pumps.Single(x => x.Id == 3).CurrentlyActive ? "BUSY " : "AVAIL")} -------  ");
            Console.SetCursorPosition(0, 4);
            Console.WriteLine();
            Console.SetCursorPosition(0, 5);
            Console.Write($"4,{(_pumps.Single(x => x.Id == 4).CurrentlyActive ? "BUSY " : "AVAIL")} -------  5,{(_pumps.Single(x => x.Id == 5).CurrentlyActive ? "BUSY " : "AVAIL")} -------  6,{(_pumps.Single(x => x.Id == 6).CurrentlyActive ? "BUSY " : "AVAIL")} -------  ");
            Console.SetCursorPosition(0, 6);
            Console.WriteLine();
            Console.SetCursorPosition(0, 7);
            Console.Write($"7,{(_pumps.Single(x => x.Id == 7).CurrentlyActive ? "BUSY " : "AVAIL")} -------  8,{(_pumps.Single(x => x.Id == 8).CurrentlyActive ? "BUSY " : "AVAIL")} -------  9,{(_pumps.Single(x => x.Id == 9).CurrentlyActive ? "BUSY " : "AVAIL")} -------  ");
            Console.SetCursorPosition(0, 8);
            Console.WriteLine();
            Console.SetCursorPosition(0, 9);
            Console.WriteLine($"total Fuel Pumped = {_pumps.Sum(pump => pump.FuelDescended)}");
            Console.SetCursorPosition(0, 10);
            Console.WriteLine($"total served cars = {_servedCars}");
            Console.SetCursorPosition(0, 15);
        }


        private static void StartPumping(TimeSpan interval, int pumpId)
        {
            CustomTimer fuelTimer = new();
            var vehicle = _vehicles.First(x => x.PumpId == null);

            vehicle.PumpId = pumpId;
            vehicle.IsAtPump = true;

            _vehicles = _vehicles.ToList();

            _pumps.Single(x => x.Id == pumpId).CurrentlyActive = true;

            fuelTimer.Elapsed += FuelTimerOnElapsed;
            fuelTimer.Interval = interval.TotalMilliseconds;
            fuelTimer.PumpId = pumpId;
            fuelTimer.Enabled = true;
            fuelTimer.AutoReset = false;
            fuelTimer.Start();
            DrawForecourt();
        }


        private static void FuelTimerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            int leftPos = Console.CursorLeft, topPos = Console.CursorTop;

            var timer = (CustomTimer)sender!;
            var pump = _pumps.Single(x => x.Id == timer.PumpId);
            pump.CurrentlyActive = false;
            pump.FuelDescended += (timer.Interval / 1000) * 1.5;

            _vehicles.Remove(_vehicles.Single(x => x.PumpId == pump.Id));
            _vehicles = _vehicles.ToList();

            _servedCars++;
            Console.SetCursorPosition(leftPos, topPos);
            timer.Dispose();
            DrawForecourt();
        }


        private static void CarGenerator()
        {
            _carSpawner.Elapsed += CarSpawnerOnElapsed;
            _carSpawner.Enabled = true;
            _carSpawner.AutoReset = true;
            _carSpawner.Start();
        }


        private static void CarSpawnerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            // at this current time the tankSize is irrelevant. for future TODO: generate tank size and assign it to new vehicle 
            _vehicles.Add(new Vehicle(tankSize: 0.00, type: Vehicle.VehicleType.Car, pumpId: null, isAtPump: false));
            _vehicles = _vehicles.ToList();
            DrawForecourt();
        }
    }
}