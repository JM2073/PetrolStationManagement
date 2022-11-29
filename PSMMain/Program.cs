// TODO https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentbag-1?view=net-6.0 check this out. might replace lists with this 
// TODO consider splitting _vehicles inot 2 lists, one for holding cars in que and the other for holding cars fuleing. 
// TODO blackbox testing lookup 

namespace PSMMain
{
    class Program
    {
        public static void Main(string[] args)
        {
            new Runner().Run();
        }
    }
}