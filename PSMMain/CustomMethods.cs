namespace PSMMain;

public static class CustomMethods
{
    public enum VehicleTypes
    {
        Car,
        Van,
        HGV
    }
    public enum FuelTypes
    {
        Unleaded,
        Diesel,
        LPG
    }
    public static int ParseStringToInt(string? inputText, string errorMessage)
    {
        var loop = true;
        var result = 0;
        while (loop)
        {
            var tryParse = int.TryParse(inputText, out result);
            if (string.IsNullOrEmpty(inputText) | !tryParse | result > 9)
            {
                Console.WriteLine(errorMessage);
                Console.Write(":");
                inputText = Console.ReadLine();
            }
            else
            {
                loop = false;
            }
        }
        return result;
    }
    
    /// <summary>
    /// https://stackoverflow.com/a/8946847
    /// </summary>
    public static void ClearCurrentConsoleLine(int cursorTop)
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, cursorTop);
        Console.Write(new string(' ', Console.WindowWidth)); 
        Console.SetCursorPosition(0, currentLineCursor);
    }
    
    
    /// <summary>
    /// https://stackoverflow.com/a/47647871
    /// </summary>
    public class ConsoleSpinner
    {
        static string[,] sequence = null;

        public int Delay { get; set; } = 200;

        int totalSequences = 0;
        int counter;

        public ConsoleSpinner()
        {
            counter = 0;
            sequence = new string[,] {
                { "/", "-", "\\", "|" },
                { ".", "o", "0", "o" },
                { "+", "x","+","x" },
                { "V", "<", "^", ">" },
                { ".   ", "..  ", "... ", "...." },
                { "=>   ", "==>  ", "===> ", "====>" },
                // ADD YOUR OWN CREATIVE SEQUENCE HERE IF YOU LIKE
            };

            totalSequences = sequence.GetLength(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequenceCode"> 0 | 1 | 2 |3 | 4 | 5 </param>
        public void Turn(string displayMsg = "", int sequenceCode = 0)
        {
            counter++;
            
            Thread.Sleep(Delay);

            sequenceCode = sequenceCode > totalSequences - 1 ? 0 : sequenceCode;

            int counterValue = counter % 4;

            string fullMessage = displayMsg + sequence[sequenceCode, counterValue];
            int msglength = fullMessage.Length;

            Console.Write(fullMessage);

            Console.SetCursorPosition(Console.CursorLeft - msglength, Console.CursorTop);
        }
    }
    
}


