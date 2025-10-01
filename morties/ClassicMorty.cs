class ClassicMorty : Morty
{
    public ClassicMorty(GameCore game, int boxCount, int fairNumber) : base(game, boxCount, fairNumber) { }
    public override int[] EliminateBoxes()
    {
        Game.ShowMessage("Morty: Let’s, uh, generate another value now, I mean, to select a box to keep in the game.");

        Game.ShowMessage($"Morty: Alright, Rick, I’m gonna open one of the boxes that doesn’t have the portal gun and isn’t your guess.");
        int[] result;
        int keeping;
        int fairNumber2 = Game.GenerateFairNumber2();
        if (RickGuessNumber == FairNumber)
        {
            result = [RickGuessNumber, fairNumber2];
            Array.Sort(result);
            keeping = fairNumber2;
        }
        else
        {
            result = [RickGuessNumber, FairNumber];
            Array.Sort(result);
            keeping = FairNumber;
        }
        Console.WriteLine($"Morty: I'm keeping the box you chose, I mean {RickGuessNumber}, and the box {keeping}.");
        return result;
    }
    public override string[] CalculateExactProbability()
    {
        return [(1.0 - 1.0 / (double)BoxCount) * 100 + "%", 1.0 / (double)BoxCount * 100 + "%"];
    }
}