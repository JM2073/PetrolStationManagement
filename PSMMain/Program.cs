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
        private static CustomMethods.ConsoleSpinner _spinner = new();

        private static int _servedCars;
        private static Timer _carSpawner = new(5000);
        private static List<Pump> _pumps = new();
        private static List<Vehicle> _vehicles = new();
        private static bool CurrentlyDrawing = false;

        public static void Main(string[] args)
        {
            InitialStartup(9);
            var onShift = true;

            while (onShift)
            {
                DrawForecourt();

                if (_vehicles.Any(x => x.IsAtPump == false))
                {
                    Console.WriteLine("you have cars in the queue, please select an available pump.");

                    var choice = CustomMethods.ParseStringToInt(Console.ReadLine(),
                        "please make sure you select a valid pump.");

                    CurrentlyDrawing = false;

                    if (_pumps.Where(x => x.CurrentlyActive == false).Any(x => x.Id == choice))
                    {
                        StartPumping(TimeSpan.FromSeconds(8), choice);
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
                    CurrentlyDrawing = false;

                    _spinner.Delay = 300;
                    while (_vehicles.ToList().Count(x => x.IsAtPump) == _vehicles.ToList().Count ||  _vehicles.Count == 0)
                        _spinner.Turn(displayMsg: "Please Wait for the next car ", sequenceCode: 5);
                }
            }

            _carSpawner.Close();
            Console.WriteLine("well done, you have done a hard days work.");
            // out of current scope, TODO calculate wages.
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
            while (CurrentlyDrawing)
            {
                //wait for the screen to be drawn before drawing again. 
            }

            Console.WriteLine("\n\n\n\n");
            CurrentlyDrawing = true;
            Console.WriteLine("Queue");
            Console.WriteLine($"           Cars: {_vehicles.Count(x => x.PumpId == null)}");
            Console.WriteLine();
            Console.Write($"1,{(_pumps.Single(x => x.Id == 1).CurrentlyActive ? "BUSY " : "AVAIL")} ------- ");
            Console.Write($"2,{(_pumps.Single(x => x.Id == 2).CurrentlyActive ? "BUSY " : "AVAIL")} ------- ");
            Console.Write($"3,{(_pumps.Single(x => x.Id == 3).CurrentlyActive ? "BUSY " : "AVAIL")} ------- ");
            Console.WriteLine();
            Console.Write($"4,{(_pumps.Single(x => x.Id == 4).CurrentlyActive ? "BUSY " : "AVAIL")} ------- ");
            Console.Write($"5,{(_pumps.Single(x => x.Id == 5).CurrentlyActive ? "BUSY " : "AVAIL")} ------- ");
            Console.Write($"6,{(_pumps.Single(x => x.Id == 6).CurrentlyActive ? "BUSY " : "AVAIL")} ------- ");
            Console.WriteLine();
            Console.Write($"7,{(_pumps.Single(x => x.Id == 7).CurrentlyActive ? "BUSY " : "AVAIL")} ------- ");
            Console.Write($"8,{(_pumps.Single(x => x.Id == 8).CurrentlyActive ? "BUSY " : "AVAIL")} ------- ");
            Console.Write($"9,{(_pumps.Single(x => x.Id == 9).CurrentlyActive ? "BUSY " : "AVAIL")} ------- ");
            Console.WriteLine("\n\n");
            Console.WriteLine($"total Fuel Pumped = {_pumps.Sum(pump => pump.FuelDescended)}");
            Console.WriteLine($"total served cars = {_servedCars}");
            Console.WriteLine("\n\n");

            CurrentlyDrawing = false;
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
            var timer = (CustomTimer)sender!;
            var pump = _pumps.Single(x => x.Id == timer.PumpId);
            pump.CurrentlyActive = false;
            pump.FuelDescended += (timer.Interval / 1000) * 1.5;

            _vehicles.Remove(_vehicles.Single(x => x.PumpId == pump.Id));
            _vehicles = _vehicles.ToList();

            _servedCars++;
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