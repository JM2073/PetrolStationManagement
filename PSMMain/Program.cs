using System.Runtime.CompilerServices;
using System.Timers;
using Timer = System.Timers.Timer;

namespace PSMMain
{
    public class CustomTimer : Timer
    {
        public int? PumpId { get; set; }
        public int? CarId { get; set; }
    }

    class Program
    {
        private static CustomMethods.ConsoleSpinner _spinner = new();

        private static bool _currentlyDrawing = false;
        private static int _servedCars = 0;
        private static int _lostCars;
        private static int _vehicalIdCount;
        private static User _currentUser = new("John", "Smith");
        private static Timer _carSpawner = new(5000);
        private static Random _ran = new();
        private static List<Pump> _pumps = new();
        private static List<Vehicle> _vehicles = new();

        public static void Main(string[] args)
        {
            var onShift = SecurityScreen();

            InitialStartup(9);
            while (onShift)
            {
                DrawForecourt();

                Console.WriteLine("========");
                Console.WriteLine("MENU");
                Console.WriteLine("========");
                Console.WriteLine("1.) Send a Car");
                Console.WriteLine("2.) Logout");
                var choice = CustomMethods.ParseStringToInt(Console.ReadLine(), "please choice a valid option");
                switch (choice)
                {
                    case 1:
                        ManageForecourt();
                        break;
                    case 2:
                        onShift = false;
                        break;
                    default:
                        Console.WriteLine("please choice a valid option");
                        break;
                }
            }

            _carSpawner.Close();
            Console.WriteLine("well done, you have done a hard days work.");
            // out of current scope, TODO calculate wages.
            Console.ReadKey();
        }

        private static void ManageForecourt()
        {
            if (_vehicles.Any(x => x.IsAtPump == false))
            {
                Console.WriteLine("you have cars in the queue, please select an available pump.");

                var choice = CustomMethods.ParseStringToInt(Console.ReadLine(),
                    "please make sure you select a valid pump.");

                _currentlyDrawing = false;

                if (_pumps.Where(x => x.CurrentlyActive == false).Any(x => x.Id == choice))
                {
                    StartPumping(TimeSpan.FromMilliseconds(_ran.Next(3000, 5000)), choice);
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
                _currentlyDrawing = false;

                _spinner.Delay = 300;
                while (_vehicles.ToList().Count(x => x.IsAtPump) == _vehicles.ToList().Count || _vehicles.Count == 0)
                    _spinner.Turn(displayMsg: "Please Wait for the next car ", sequenceCode: 5);
            }
        }

        private static bool SecurityScreen()
        {
            var authenticate = false;
            while (authenticate == false)
            {
                Console.WriteLine("welcome to the Petrol Station Management software for Broken Petrol Ltd");
                Console.Write("Please enter your Email: ");
                var email = Console.ReadLine();
                Console.Write("Please enter your Password, Press enter when your done.");

                //https://stackoverflow.com/a/3404522
                var pass = string.Empty;
                ConsoleKey key;
                do
                {
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if (key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        Console.Write("\b \b");
                        pass = pass[0..^1];
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Console.Write("*");
                        pass += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);

                Console.WriteLine();

                if (email == _currentUser.Email == false && pass == _currentUser.Password == false)
                {
                    Console.Clear();
                    Console.WriteLine("Please enter valid login information\n");
                }
                else
                {
                    authenticate = true;
                    Console.Clear();
                }
            }

            return authenticate;
        }

        private static void InitialStartup(int numOfPumps)
        {
            for (var i = 1; i <= numOfPumps; i++)
                _pumps.Add(new Pump(i, false, 0.00, 0.00, 0.00));

            _pumps = _pumps.ToList();

            CarGenerator();
        }


        private static void DrawForecourt()
        {
            //wait for the screen to be drawn before drawing again. 
            while (_currentlyDrawing)
            {
            }

            Console.WriteLine("\n\n\n\n");
            _currentlyDrawing = true;

            Console.WriteLine($"Petrol Station Management - Welcome {_currentUser.FirstName} {_currentUser.LastName}");
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
            Console.WriteLine($"total Unleaded Fuel Pumped = {_pumps.Sum(pump => pump.UnloadedFuelDescended):0.00}");
            Console.WriteLine($"total Diesel Fuel Pumped = {_pumps.Sum(pump => pump.DieselFuelDescended):0.00}");
            Console.WriteLine($"total LPG Fuel Pumped = {_pumps.Sum(pump => pump.LpgFuelDescended):0.00}");
            Console.WriteLine($"total served cars = {_servedCars}");
            Console.WriteLine($"total Lost cars = {_lostCars}");
            Console.WriteLine("\n\n");

            _currentlyDrawing = false;
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
            var vehicle = _vehicles.Single(x => x.PumpId == pump.Id);

            switch (vehicle.FuelType)
            {
                case CustomMethods.FuelTypes.Unleaded:
                    pump.UnloadedFuelDescended += (timer.Interval / 1000) * 1.5;
                    break;
                case CustomMethods.FuelTypes.Diesel:
                    pump.DieselFuelDescended += (timer.Interval / 1000) * 1.5;
                    break;
                case CustomMethods.FuelTypes.LPG:
                    pump.LpgFuelDescended += (timer.Interval / 1000) * 1.5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _vehicles.Remove(vehicle);
            _vehicles = _vehicles.ToList();

            _servedCars++;
            timer.Close();
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
            _carSpawner.Interval = _ran.Next(1500, 2200);

            if (_vehicles.Where(x => x.IsAtPump == false).ToList().Count == 1)
                return;
            _vehicalIdCount++;

            // at this current time the tankSize is irrelevant. for future TODO: generate tank size and assign it to new vehicle 
            var vehicle = new Vehicle(_vehicalIdCount, 0.00, null, false, (CustomMethods.VehicleTypes)_ran.Next(1, 4),
                (CustomMethods.FuelTypes)_ran.Next(1, 4));
            _vehicles.Add(vehicle);

            _vehicles = _vehicles.ToList();
            DrawForecourt();

            CarTimout(vehicle);
        }

        private static void CarTimout(Vehicle vehicle)
        {
            CustomTimer carTimeOut = new();

            carTimeOut.Elapsed += CarTimeOutOnElapsed;
            carTimeOut.Interval = 1500;
            carTimeOut.CarId = vehicle.Id;
            carTimeOut.Enabled = true;
            carTimeOut.AutoReset = false;
            carTimeOut.Start();
        }

        private static void CarTimeOutOnElapsed(object? sender, ElapsedEventArgs e)
        {
            var timer = (CustomTimer)sender!;
            var vehicle = _vehicles.Single(x => x.Id == timer.CarId);

            if (vehicle.IsAtPump)
                return;
            
            _vehicles.Remove(vehicle);
            _vehicles = _vehicles.ToList();
            _lostCars++;
            timer.Close();
            DrawForecourt();
        }
    }
}