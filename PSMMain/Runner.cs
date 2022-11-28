namespace PSMMain;

public class Runner
{
    private bool _currentlyDrawing;
    private int _servedCars;
    private int _queCount;
    private int _lostCars;
    private int _vehicleIdCount;
    private double _totalRev;
    private User _currentUser = new("John", "Smith");
    private Timer _carSpawner = new(5000);
    private Random _ran = new();
    private List<Pump> _pumps = new();
    private List<Vehicle> _vehicles = new();

    // as of typing these are the avrage prices of fule in the uk.
    // https://www.mylpg.eu/stations/united-kingdom/prices/
    private double PriceOfUnleaded = 1.6;
    private double PriceOfDiesel = 1.84;
    private double PriceOfLPG = 0.85;
    
    public void Run()
    {
         
#if DEBUG
        var onShift = true;
#else
            var onShift = SecurityScreen();
#endif

        InitialStartup(9);

        while (onShift)
        {
            onShift = ManageForecourt();
        }

        _carSpawner.Close();
        Console.WriteLine("well done, thank you for all your hard work!");
        // out of current scope, TODO calculate wages.
        Console.ReadKey();
    }
    
        private static bool ManageForecourt()
        {
            if (_queCount == 0)
                return true;

            int choice = 0;
            while (_queCount != 0)
            {
                if (Console.KeyAvailable)
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.NumPad1:
                            choice = 1;
                            break;
                        case ConsoleKey.NumPad2:
                            choice = 2;
                            break;
                        case ConsoleKey.NumPad3:
                            choice = 3;
                            break;
                        case ConsoleKey.NumPad4:
                            choice = 4;
                            break;
                        case ConsoleKey.NumPad5:
                            choice = 5;
                            break;
                        case ConsoleKey.NumPad6:
                            choice = 6;
                            break;
                        case ConsoleKey.NumPad7:
                            choice = 7;
                            break;
                        case ConsoleKey.NumPad8:
                            choice = 8;
                            break;
                        case ConsoleKey.NumPad9:
                            choice = 9;
                            break;
                        case ConsoleKey.Escape:
                            return false;
                    }

                var pump = _pumps.SingleOrDefault(x => x.Id == choice);
                if (pump is { CurrentlyActive: false })
                {
                    StartPumping(TimeSpan.FromMilliseconds(_ran.Next(3000, 5000)), choice);
                }
                else if (pump is { CurrentlyActive: true })
                {
                    Console.WriteLine("please select an available pump.");
                }

                choice = 0;
            }
            
            //https://stackoverflow.com/a/6541403 
            //cycles though cached inputs to clear them out.
            while ( Console.KeyAvailable )
            {
                ConsoleKeyInfo key = Console.ReadKey();
            }
            return true;
        }

        private static bool SecurityScreen()
        {
            bool authenticate = false;
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

                if (email == _currentUser.Email == false || pass == _currentUser.Password == false)
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

            RenderForecourt();
            CarGenerator();
        }


        private static void RenderForecourt()
        {
            //wait for the screen to be drawn before drawing again. 
            while (_currentlyDrawing)
            {
            }

            _currentlyDrawing = true;
            Console.Clear();
            // Console.SetCursorPosition(0,0);
            Console.WriteLine($"Petrol Station Management - Welcome {_currentUser.FirstName} {_currentUser.LastName}");
            Console.WriteLine("Queue");
            Console.WriteLine($"           Cars: {_queCount}");
#if DEBUG
            Console.WriteLine($"           (DEBUG) TOTAL Cars: {_vehicleIdCount}");
#endif
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
            Console.WriteLine($"Liters sold:");
            Console.WriteLine($"    Unleaded Fuel Pumped = {_pumps.Sum(pump => pump.UnloadedFuelDescended):0.00}");
            Console.WriteLine($"    Diesel Fuel Pumped = {_pumps.Sum(pump => pump.DieselFuelDescended):0.00}");
            Console.WriteLine($"    LPG Fuel Pumped = {_pumps.Sum(pump => pump.LpgFuelDescended):0.00}");
            Console.WriteLine($"total served cars = {_servedCars}");
            Console.WriteLine($"total cars left early = {_lostCars}");
            Console.WriteLine("\n\n");
            Console.WriteLine("To finish your shift for the day please press the 'ESC' key");

            if (_queCount != 0)
            {
                Console.WriteLine("you have cars in the queue, please select an available pump.");
                Console.WriteLine(
                    "inorder to send a car to a pump please select the corresponding key on your number pad, eg: for pump 1 press the 'NUM 1' key");
            }

            if (_queCount == 0)
            {
                Console.WriteLine("please wait for a vehicle to join the que ");
            }

            _currentlyDrawing = false;
        }


        private static void StartPumping(TimeSpan interval, int pumpId)
        {
            //TODO: check there is a pump matching the ID
            CustomTimer fuelTimer = new CustomTimer();
            var vehicle = _vehicles.First(x => x.PumpId == null);

            vehicle.PumpId = pumpId;

            _vehicles = _vehicles.ToList();

            _pumps.Single(x => x.Id == pumpId).CurrentlyActive = true;
            
            fuelTimer.Elapsed += FuelTimerOnElapsed;
            fuelTimer.Interval = interval.TotalMilliseconds;
            fuelTimer.PumpId = pumpId;
            fuelTimer.Enabled = true;
            fuelTimer.AutoReset = false;
            fuelTimer.Start();
            _queCount--;
            RenderForecourt();
        }


        private static void FuelTimerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            var timer = (CustomTimer)sender!;
            var pump = _pumps.Single(x => x.Id == timer.PumpId);
            pump.CurrentlyActive = false;

            var vehicle = _vehicles.Single(x => x.PumpId == pump.Id);
            double amountPumped = (timer.Interval / 1000) * 1.5;
            
            switch (vehicle.FuelType)
            {
                case Vehicle.FuelTypes.Unleaded:
                    pump.UnloadedFuelDescended += amountPumped;
                    _totalRev += amountPumped * PriceOfUnleaded;
                    break;
                case Vehicle.FuelTypes.Diesel:
                    pump.DieselFuelDescended += amountPumped;
                    _totalRev += amountPumped * PriceOfDiesel;
                    break;
                case Vehicle.FuelTypes.LPG:
                    pump.LpgFuelDescended += amountPumped;
                    _totalRev += amountPumped * PriceOfLPG;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _vehicles.Remove(vehicle);
            _vehicles = _vehicles.ToList();


            _servedCars++;
            timer.Close();
            RenderForecourt();
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

            // make sure there is only one car in que
            if (_vehicles.Where(x => x.IsAtPump() == false).ToList().Count >= 5)
                return;

            _queCount++;
            _vehicleIdCount++;
            // at this current time the tankSize is irrelevant. for future TODO: generate tank size and assign it to new vehicle 
            var vehicle = new Vehicle(_vehicleIdCount, 0.00, (Vehicle.VehicleTypes)_ran.Next(0, 3));
            switch (vehicle.VehicleType)
            {
                case Vehicle.VehicleTypes.Car:
                    vehicle.FuelType = (Vehicle.FuelTypes)_ran.Next(0, 3);
                    vehicle.TankLevel = _ran.Next(1, 25);
                    break;
                case Vehicle.VehicleTypes.Van:
                    vehicle.FuelType = (Vehicle.FuelTypes) _ran.Next(0, 2);
                    vehicle.TankLevel = _ran.Next(1,40);
                    break;
                case Vehicle.VehicleTypes.HGV:
                    vehicle.FuelType = Vehicle.FuelTypes.Diesel;
                    vehicle.TankLevel = _ran.Next(1, 75);
                    break;
            }
            
            _vehicles.Add(vehicle);
            _vehicles = _vehicles.ToList();
            
            CarTimout(vehicle);
            RenderForecourt();
        }

        private static void CarTimout(Vehicle vehicle)
        {
            CustomTimer carTimeOut = new();

            carTimeOut.Elapsed += CarTimeOutOnElapsed;
            carTimeOut.Interval = _ran.Next(1000,2000);
            carTimeOut.CarId = vehicle.Id;
            carTimeOut.Enabled = true;
            carTimeOut.AutoReset = false;
            carTimeOut.Start();
        }

        private static void CarTimeOutOnElapsed(object? sender, ElapsedEventArgs e)
        {
            var timer = (CustomTimer)sender!;

            var vehicle = _vehicles.Single(x => x.Id == timer.CarId);

            if (vehicle.IsAtPump())
            {
                timer.Close();
                return;
            }

            _vehicles.Remove(vehicle);
            _vehicles = _vehicles.ToList();

            _queCount--;
            _lostCars++;
            timer.Close();
            RenderForecourt();
        }
     
}