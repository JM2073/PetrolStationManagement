namespace PSMMain.Models;

public class User
{
    public User(string firstName, string lastName, DateTime shiftStart)
    {
        FirstName = firstName;
        LastName = lastName;
        ShiftStart = shiftStart;
        HourlyWage = 12.49;
        Password = $"{firstName[0]}{lastName[0]}PLeaseChangeMe1234!";
        Email = $"{firstName}.{lastName}@BrokenPetrol.com";
    }


    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Email { get; set; }
    public string Password { get; set; }

    public DateTime ShiftStart { get; set; }
    public double HourlyWage { get; set; }

    public double CalcWage(DateTime shiftEnd)
    {
        double money;
        TimeSpan workedTimer = shiftEnd.Subtract(this.ShiftStart);

#if DEBUG
        money = (double)workedTimer.TotalMinutes * HourlyWage;
#else
        money = (double)workedTimer.TotalHours * HourlyWage;
#endif

        return money;

    }
}