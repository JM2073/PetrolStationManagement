namespace PSMMain;

public static class CustomMethods
{
    public static int ParseStringToInt(string? inputText, string errorMessage)
    {
        var loop = true;
        var result = 0;
        while (loop)
        {
            var tryParse = int.TryParse(inputText, out result);
            if (string.IsNullOrEmpty(inputText) | !tryParse | result > 9)
            {
                
                Console.SetCursorPosition(0, 16);
                Console.WriteLine(errorMessage);
                Console.SetCursorPosition(0, 17);
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
    public static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth)); 
        Console.SetCursorPosition(0, currentLineCursor);
    }
}


