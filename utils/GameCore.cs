using ConsoleTables;
class GameCore
{
    private int BoxCount;
    private int FairNumber1;
    private int FairNumber2;
    private string MortyType;
    private Morty? Morty;
    private string? key1;
    private string? key2;
    private string? hmac1;
    private string? hmac2;
    private int m1;
    private int m2;
    private int r1;
    private int r2;
    private int b;
    private int[]? remainingBoxes;
    private StatisticsAccumulator statisticsAccumulator;
    private bool isAutoMode = false;
    public GameCore(int boxCount, string mortyType)
    {
        this.BoxCount = boxCount;
        this.MortyType = mortyType;
        this.statisticsAccumulator = new StatisticsAccumulator();
        MortyPlugin.ValidateMortyType(mortyType);
    }
    public void GenerateFairNumber1()
    {
        key1 = ProvablyFairProtocol.GenerateKey();
        m1 = ProvablyFairProtocol.GenerateRandomNumber(BoxCount);
        hmac1 = ProvablyFairProtocol.ComputeHMAC(key1, m1);
        Console.WriteLine($"Morty: Oh geez, Rick, I'm gonna hide your portal gun in one of the {BoxCount} boxes, okay?");
        Console.WriteLine($"Morty: HMAC1={hmac1}");
        Console.WriteLine($"Morty: Rick, enter your number [0,{BoxCount}) so you don't whine later that I cheated, alright?");
        while (!int.TryParse(Console.ReadLine(), out r1) || r1 >= BoxCount || r1 < 0)
        {
            Console.WriteLine($"Morty: Aw geez Rick, you need to enter a number between 0 and {BoxCount - 1}!");
        }
        FairNumber1 = ProvablyFairProtocol.GenerateFairNumber(m1, r1, BoxCount);
        Morty = MortyPlugin.GetMortyInstance(MortyType, this, BoxCount, FairNumber1);
        Console.WriteLine($"Morty: Okay, okay, I hid the gun. What's your guess [0,{BoxCount})?");
        while (!int.TryParse(Console.ReadLine(), out b) || b >= BoxCount || b < 0)
        {
            Console.WriteLine($"Morty: Aw geez Rick, you need to enter a number between 0 and {BoxCount - 1}!");
        }
        Morty.ReceiveRickGuess(b);
    }
    
    public void GenerateFairNumber1Auto()
    {
        key1 = ProvablyFairProtocol.GenerateKey();
        m1 = ProvablyFairProtocol.GenerateRandomNumber(BoxCount);
        hmac1 = ProvablyFairProtocol.ComputeHMAC(key1, m1);
        
        // Auto-generate r1 using ProvablyFairProtocol
        r1 = ProvablyFairProtocol.GenerateRandomNumber(BoxCount);
        
        FairNumber1 = ProvablyFairProtocol.GenerateFairNumber(m1, r1, BoxCount);
        Morty = MortyPlugin.GetMortyInstance(MortyType, this, BoxCount, FairNumber1);
        
        // Auto-generate initial guess using ProvablyFairProtocol
        b = ProvablyFairProtocol.GenerateRandomNumber(BoxCount);
        
        Morty.ReceiveRickGuess(b);
    }
    public int GenerateFairNumber2()
    {
        key2 = ProvablyFairProtocol.GenerateKey();
        m2 = ProvablyFairProtocol.GenerateRandomNumber(BoxCount);
        hmac2 = ProvablyFairProtocol.ComputeHMAC(key2, m2);
        
        if (!isAutoMode)
        {
            Console.WriteLine($"Morty: HMAC2={hmac2}");
            var numbers = Enumerable.Range(0, BoxCount).Where(n => n != b);
            Console.WriteLine($"Morty: Rick, enter your number [{string.Join(", ", numbers)}] so you don't whine later that I cheated, alright?");
            while (!int.TryParse(Console.ReadLine(), out r2) || !numbers.Contains(r2))
            {
                Console.WriteLine($"Morty: Aw geez Rick, you need to enter a number from [{string.Join(", ", numbers)}]!");
            }
        }
        else
        {
            // Auto-generate r2 from available numbers
            var numbers = Enumerable.Range(0, BoxCount).Where(n => n != b).ToArray();
            r2 = numbers[ProvablyFairProtocol.GenerateRandomNumber(numbers.Length)];
        }
        
        remainingBoxes = Enumerable.Range(0, BoxCount).Where(n => n != FairNumber1 && n != b).ToArray();
        FairNumber2 = remainingBoxes[ProvablyFairProtocol.GenerateFairNumber(m2, r2, remainingBoxes.Length)];

        return FairNumber2;
    }
    public void ShowMessage(string message) { Console.WriteLine(message); }

    public StatisticsAccumulator GetStatistics()
    {
        return statisticsAccumulator;
    }

    public void PlayRound()
    {
        GenerateFairNumber1();
        remainingBoxes = Morty!.EliminateBoxes();
        Console.WriteLine("Morty: You can switch your box (enter 0), or, you know, stick with it (enter 1).");
        int switchOrNot;
        while (!int.TryParse(Console.ReadLine(), out switchOrNot) || (switchOrNot != 0 && switchOrNot != 1))
        {
            Console.WriteLine("Morty: Aw geez Rick, you need to enter 0 to switch or 1 to stick with your choice!");
        }
        Console.WriteLine($"Morty: Aww man, my 1st random value is {m1}.");
        Console.WriteLine($"Morty: KEY1={key1}");
        Console.WriteLine($"Morty: So the 1st fair number is ({m1} + {r1}) % {BoxCount} = {FairNumber1}. ");
        Console.WriteLine($"Morty: Aww man, my 2nd random value is {m2}.  ");
        Console.WriteLine($"Morty: KEY2={key2}  ");
        Console.WriteLine($"Morty: Uh, okay, the 2nd fair number is ({m2} + {r2}) % ({BoxCount - 1}) = {FairNumber2}");
        Console.WriteLine($"Morty: You portal gun is in the box {FairNumber1}. ");
        int finalBox = switchOrNot == 0 ? remainingBoxes.First(box => box != b) : b;
        bool isWin = finalBox == FairNumber1;
        bool isSwitched = switchOrNot == 0;

        statisticsAccumulator.RecordRound(isWin, isSwitched);

        if (isWin)
        {
            Console.WriteLine("Morty: You won the portal gun! I mean, you got the portal gun!");
        }
        else
        {
            Console.WriteLine("Morty: You lost! I mean, you didn't get the portal gun!");
        }
    }

    public void PlayRoundAuto()
    {
        isAutoMode = true;
        GenerateFairNumber1Auto();
        remainingBoxes = Morty!.EliminateBoxes();
        
        // Auto-generate switch decision using ProvablyFairProtocol
        int switchOrNot = ProvablyFairProtocol.GenerateRandomNumber(2); // 0 or 1

        Console.WriteLine($"Auto: Rick's guess = {b}, Switch decision = {(switchOrNot == 0 ? "Switch" : "Stay")}");

        int finalBox = switchOrNot == 0 ? remainingBoxes.First(box => box != b) : b;
        bool isWin = finalBox == FairNumber1;
        bool isSwitched = switchOrNot == 0;

        statisticsAccumulator.RecordRound(isWin, isSwitched);

        Console.WriteLine($"Portal gun was in box {FairNumber1}. Final choice: box {finalBox}. Result: {(isWin ? "WIN" : "LOSE")}");
        
        isAutoMode = false;
    }

    public void PlayGame()
    {
        bool continuePlaying = true;
        while (continuePlaying)
        {
            PlayRound();
            Console.WriteLine("Morty: Do you want to play another round? (y/n)");
            string response = Console.ReadLine()!.Trim().ToLower();
            continuePlaying = response == "yes" || response == "y";
        }
        Console.WriteLine("Morty: Okay... uh... Bye, Rick.");
        string[] exactProbabilities = Morty!.CalculateExactProbability();
        string[] estimatedProbabilities = statisticsAccumulator.CalculateEstimateProbability();
        var table = new ConsoleTable("Game Results", "Switched", "Stayed");
        table.AddRow("Rounds", statisticsAccumulator.SwitchedRounds, statisticsAccumulator.StayedRounds)
             .AddRow("Wins", statisticsAccumulator.SwitchedWins, statisticsAccumulator.StayedWins)
            .AddRow("Exact Probability", exactProbabilities[0], exactProbabilities[1])
            .AddRow("Estimated Probability", estimatedProbabilities[0], estimatedProbabilities[1]);

        table.Write();
        Console.WriteLine();
    }

    public void PlayGameAuto()
    {
        Console.WriteLine("Morty: Aw geez Rick, starting automated game with 100 rounds...");

        for (int round = 1; round <= 100; round++)
        {
            Console.WriteLine($"\n--- Round {round}/100 ---");
            PlayRoundAuto();
        }

        Console.WriteLine("\nMorty: Aw man, the automated game is done, Rick!");
        string[] exactProbabilities = Morty!.CalculateExactProbability();
        string[] estimatedProbabilities = statisticsAccumulator.CalculateEstimateProbability();
        var table = new ConsoleTable("Game Results", "Switched", "Stayed");
        table.AddRow("Rounds", statisticsAccumulator.SwitchedRounds, statisticsAccumulator.StayedRounds)
             .AddRow("Wins", statisticsAccumulator.SwitchedWins, statisticsAccumulator.StayedWins)
            .AddRow("Exact Probability", exactProbabilities[0], exactProbabilities[1])
            .AddRow("Estimated Probability", estimatedProbabilities[0], estimatedProbabilities[1]);

        table.Write();
        Console.WriteLine();
    }

}