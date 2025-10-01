using System.Reflection;

class MortyPlugin
{
    public static string[] GetAvailableMortyTypes()
    {
        string[] mortyTypes = Directory.GetFiles("./morties", "*.cs")
            .SelectMany(file => File.ReadAllLines(file))
            .Where(line => line.Contains("class") && line.Contains("Morty") && !line.Contains("abstract"))
            .Select(line => line.Split(' ')[1].Trim())
            .ToArray();
        return mortyTypes;
    }
    public static Morty GetMortyInstance(string mortyType, GameCore game, int boxCount, int fairNumber)
    {
        string[] availableTypes = GetAvailableMortyTypes();
        if (!availableTypes.Contains(mortyType))
        {
            Console.WriteLine($"Invalid Morty type. Supported types: {string.Join(", ", availableTypes)}");
            Environment.Exit(1);
        }

        var type = Assembly.GetExecutingAssembly()
                   .GetTypes()
                   .SingleOrDefault(t => t.Name == mortyType);

        if (type == null)
        {
            Console.WriteLine($"Could not load Morty type {mortyType}");
            Environment.Exit(1);
        }

        var mortyInstance = Activator.CreateInstance(type, game, boxCount, fairNumber);
        if (mortyInstance == null)
        {
            Console.WriteLine($"Failed to create instance of Morty type {mortyType}");
            Environment.Exit(1);
        }
        return (Morty)mortyInstance;
    }

    public static void ValidateMortyType(string mortyType)
    {
        string[] availableTypes = GetAvailableMortyTypes();
        if (!availableTypes.Contains(mortyType))
        {
            Console.WriteLine($"Invalid Morty type. Supported types: {string.Join(", ", availableTypes)}");
            Environment.Exit(1);
        }
    }
}
