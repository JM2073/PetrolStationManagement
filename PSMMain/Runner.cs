using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;

namespace PSMMain
{
    public class Runner
    {
        private readonly User _currentUser = new("John", "Smith");
        private CustomTimer _carSpawner = new(5000);
        private readonly Random _ran = new();
        private List<Pump> _pumps = new();
        private List<Vehicle> _vehicles = new();

        private bool _currentlyDrawing;
        private int _servedCars;
        private int _queCount;
        private int _lostCars;
        private int _vehicleIdCount;
        private double _totalRev;

        // as of typing these are the avrage prices of fule in the uk.
        // https://www.mylpg.eu/stations/united-kingdom/prices/
        private double _priceOfUnleaded = 1.6;
        private double _priceOfDiesel = 1.84;
        private double _priceOfLpg = 0.85;

        //setting the fill speed of the pumps, to help speed debugging, the pumps fill slightly faster to reduce waiting times.
        //in a release environment the fill time will be required the 1.5
#if DEBUG
        private const double FuelPumpedPerSecond = 3;
#else
        private const double FuelPumpedPerSecond = 1.5;
#endif


        //entry point from the start of the application. 
        public void Run()
        {
            //to aid debugging when in a DEBUG environment the login is skipped.
#if DEBUG
            var onShift = true;
#else
            var onShift = SecurityScreen();
#endif

            InitialStartup(9);

            //main functionality loop 
            while (onShift)
            {
                onShift = ManageForecourt();
            }


            _carSpawner.Close();
            Console.WriteLine("well done, thank you for all your hard work!");
            Console.ReadKey();
        }

        /// <summary>
        /// generates a number of pumps based on the param numOfPumps, this will be the forecourt.
        /// </summary>
        /// <param name="numOfPumps">the number of pumps.</param>
        private void InitialStartup(int numOfPumps)
        {
            for (var i = 1; i <= numOfPumps; i++)
                _pumps.Add(new Pump(i, false, 0.00, 0.00, 0.00));

            _pumps = _pumps.ToList();

            RenderForecourt();
            CarGenerator();
        }

        /// <summary>
        /// waits and takes the user input, the process if that input can move a car to a pump or not.
        /// </summary>
        /// <returns>a bool for if the program should end the loop, thus ending the program.</returns>
        private bool ManageForecourt()
        {
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                return false;
            }

            while (_queCount > 0)
            {
                if (_pumps.Any(x => x.CurrentlyActive == false))
                    StartPumping(_pumps.First(x => x.CurrentlyActive == false).Id);
            }

            return true;
        }

        /// <summary>
        /// the login screen where it asks the user for there username and password, hiding the password for security reasons.
        /// </summary>
        /// <returns>a bool for if the login was successful</returns>
        private bool SecurityScreen()
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


        /// <summary>
        /// displays the information to the user.
        /// </summary>
        private void RenderForecourt()
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
            Console.WriteLine($"           (DEBUG) car que details:\n");
            foreach (var vehicle in _vehicles.Where(x => x.IsAtPump() == false))
            {
                Console.WriteLine(
                    $"           (DEBUG) id:{vehicle.Id}, fuel type:{vehicle.FuelType.ToString()}, vehicle type:{vehicle.VehicleType}, starting tank level:{vehicle.TankLevel}\n");
            }
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

#if DEBUG
            Console.WriteLine($"           (DEBUG) cars at pumps details:\n");
            foreach (var vehicle in _vehicles.Where(x => x.PumpId != null))
            {
                Console.WriteLine(
                    $"           (DEBUG) id:{vehicle.Id}, fuel type:{vehicle.FuelType.ToString()}, vehicle type:{vehicle.VehicleType}, starting tank level:{vehicle.TankLevel}, pump assigned to:{vehicle.PumpId}\n");
            }
#endif

            Console.WriteLine($"Liters sold:");
            Console.WriteLine($"    Unleaded Fuel Pumped = {_pumps.Sum(pump => pump.UnloadedFuelDescended):F}");
            Console.WriteLine($"    Diesel Fuel Pumped = {_pumps.Sum(pump => pump.DieselFuelDescended):F}");
            Console.WriteLine($"    LPG Fuel Pumped = {_pumps.Sum(pump => pump.LpgFuelDescended):F}");
            Console.WriteLine($"total Money Made = {_totalRev:F}");
            Console.WriteLine($"1% for commission = {_totalRev / 100:F}");
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

        /// <summary>
        /// starts the timer for cars to be genrated every 
        /// </summary>
        private void CarGenerator()
        {
            _carSpawner.Elapsed += CarSpawnerOnElapsed;
            _carSpawner.Enabled = true;
            _carSpawner.AutoReset = true;
            _carSpawner.Start();
        }

        /// <summary>
        /// makes a new vehicle and adds it to the list of current vehicles
        /// </summary>
        /// <param name="sender">the timer, that is of class CustomTimer</param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentOutOfRangeException">if the vehicle type is set outside of the enum set.</exception>
        private void CarSpawnerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            _carSpawner.Interval = _ran.Next(1500, 2200);

            // make sure there is only five car in que
            if (_vehicles.Where(x => x.IsAtPump() == false).ToList().Count >= 5)
                return;

            _queCount++;
            _vehicleIdCount++;

            //creates a new Vehicle, setting the fuel type and tank level baced on the Vehicle Type.
            var vehicle = new Vehicle(_vehicleIdCount, (Vehicle.VehicleTypes)_ran.Next(0, 3));
            switch (vehicle.VehicleType)
            {
                case Vehicle.VehicleTypes.Car:
                    vehicle.FuelType = (Vehicle.FuelTypes)_ran.Next(0, 3);
                    vehicle.TankLevel = _ran.Next(1, 25);
                    break;
                case Vehicle.VehicleTypes.Van:
                    vehicle.FuelType = (Vehicle.FuelTypes)_ran.Next(0, 2);
                    vehicle.TankLevel = _ran.Next(1, 40);
                    break;
                case Vehicle.VehicleTypes.HGV:
                    vehicle.FuelType = Vehicle.FuelTypes.Diesel;
                    vehicle.TankLevel = _ran.Next(1, 75);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _vehicles.Add(vehicle);
            _vehicles = _vehicles.ToList();

            CarTimout(vehicle.Id);
            RenderForecourt();
        }

        /// <summary>
        /// starts a timer for how long the vehicle will wait before leaving. 
        /// </summary>
        /// <param name="vehicleId">the vehicleId of the vehicle that the timer is for.</param>
        private void CarTimout(int vehicleId)
        {
            CustomTimer carTimeOut = new CustomTimer(_ran.Next(1000, 2000));

            carTimeOut.Elapsed += CarTimeOutOnElapsed;
            carTimeOut.CarId = vehicleId;
            carTimeOut.Enabled = true;
            carTimeOut.AutoReset = false;
            carTimeOut.Start();
        }

        /// <summary>
        /// checks if the vehicle is currently at a pump and returns urly if it is. otherwise, remove it from the list of vehicles.
        /// </summary>
        /// <param name="sender">the timer, that is of class CustomTimer</param>
        /// <param name="e"></param>
        private void CarTimeOutOnElapsed(object? sender, ElapsedEventArgs e)
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

        /// <summary>
        /// makes and starts the timer for adding fuel to a vehicle.
        /// </summary>
        /// <param name="pumpId">the id of the pump that the vehicle will fill at. </param>
        private void StartPumping(int pumpId)
        {
            //sets the target pump as currently being used. 
            _pumps.Single(x => x.Id == pumpId).CurrentlyActive = true;

            // grabs the first vehicle and assigns it to the pump by setting vehicles pumpid to the pumps id.
            var vehicle = _vehicles.First(x => x.PumpId == null);
            vehicle.PumpId = pumpId;
            _vehicles = _vehicles.ToList();

            //make a new timer for the vehicle; associated by the pump id; where the interval is Calculate in the CalculateFuelTime method.
            //the timer is for how long the vehicle will be at the pump. 
            CustomTimer fuelTimer = new CustomTimer(CalculateFuelTime(vehicle.VehicleType, vehicle.TankLevel));


            fuelTimer.Elapsed += FuelTimerOnElapsed;
            fuelTimer.PumpId = pumpId;
            fuelTimer.Enabled = true;
            fuelTimer.AutoReset = false;
            fuelTimer.Start();
            _queCount--;
            RenderForecourt();
        }

        /// <summary>
        /// takes the vehicle type and the current fuel in the tank and works out how long it will take to fill in milliseconds. 
        /// </summary>
        /// <param name="vehicleType">what kind of vehicle it is; car, van, HGV</param>
        /// <param name="tankLevel">how much fuel is currently in the vehicle tank.</param>
        /// <returns>a double for the time it would take to fill in milliseconds.</returns>
        /// <exception cref="ArgumentOutOfRangeException">this would trigger if vehicleType is set to something other than 0~2 </exception>
        private double CalculateFuelTime(Vehicle.VehicleTypes vehicleType, double tankLevel)
        {
            double remainingTankSize = vehicleType switch
            {
                Vehicle.VehicleTypes.Car => 50 - tankLevel,
                Vehicle.VehicleTypes.Van => 80 - tankLevel,
                Vehicle.VehicleTypes.HGV => 150 - tankLevel,
                _ => throw new ArgumentOutOfRangeException()
            };

            return (remainingTankSize / FuelPumpedPerSecond) * 1000;
        }

        /// <summary>
        /// when a vehicle is done pumping, (when the pumping timer ends) this adds the pumped fuel and cost to the totals.
        /// </summary>
        /// <param name="sender">the timer, that is of class CustomTimer</param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentOutOfRangeException">if for some reason the vehicles</exception>
        private void FuelTimerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            var timer = (CustomTimer)sender!;
            var pump = _pumps.Single(x => x.Id == timer.PumpId);


            var vehicle = _vehicles.Single(x => x.PumpId == pump.Id);
            double amountPumped = (timer.Interval / 1000) * FuelPumpedPerSecond;

            switch (vehicle.FuelType)
            {
                case Vehicle.FuelTypes.Unleaded:
                    pump.UnloadedFuelDescended += amountPumped;
                    _totalRev += amountPumped * _priceOfUnleaded;
                    break;
                case Vehicle.FuelTypes.Diesel:
                    pump.DieselFuelDescended += amountPumped;
                    _totalRev += amountPumped * _priceOfDiesel;
                    break;
                case Vehicle.FuelTypes.LPG:
                    pump.LpgFuelDescended += amountPumped;
                    _totalRev += amountPumped * _priceOfLpg;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _vehicles.Remove(vehicle);
            _vehicles = _vehicles.ToList();
            pump.CurrentlyActive = false;

            _servedCars++;
            timer.Close();
            RenderForecourt();
        }
    }
}