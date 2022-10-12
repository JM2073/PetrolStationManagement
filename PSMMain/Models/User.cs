namespace PSMMain;

public class User
{
    public User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        Password = $"{firstName[0]}{lastName[0]}PLeaseChangeMe1234!";
        Email = $"{firstName}.{lastName}@BrokenPetrol.com";
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string Email { get; set; }
    public string Password { get; set; }
    
}