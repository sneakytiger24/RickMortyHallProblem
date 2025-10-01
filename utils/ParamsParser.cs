using System.ComponentModel.DataAnnotations;

class ParamsParser
{
    public static int BoxNumber { get; private set; }
    public static string MortyType { get; private set; }
    public static void ValidateArguments(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Number of boxes is required.");
            Environment.Exit(0);
        }
        if (int.Parse(args[0]) < 3)
        {
            Console.WriteLine("Number of boxes must be at least 3.");
            Environment.Exit(0);
        }
        BoxNumber = int.Parse(args[0]);
        if (args.Length < 2)
        {
            Console.WriteLine("Morty type is required.");
            Environment.Exit(0);
        }
        MortyType = args[1];
    }
}
