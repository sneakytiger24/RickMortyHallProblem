class LazyMorty : Morty
{
    public LazyMorty(GameCore game, int boxCount, int fairNumber) : base(game, boxCount, fairNumber) { }

    public override int[] EliminateBoxes()
    {
        Game.GenerateFairNumber2();
        Game.ShowMessage("Morty: Ugh, Rick, I'm too lazy to use generated random numbers, so I'll just, uh, pick the lowest numbered boxes to keep.");
        Game.ShowMessage($"Morty: Alright, Rick, I'm gonna open boxes with the lowest indices, but not the one with the portal gun or your guess.");

        int[] result;
        int keeping;

        if (RickGuessNumber == FairNumber)
        {
            // Rick guessed correctly, so we need to find the lowest index that's not Rick's guess
            keeping = 0;
            while (keeping == RickGuessNumber)
            {
                keeping++;
            }
            result = [RickGuessNumber, keeping];
            Array.Sort(result);
        }
        else
        {
            // Rick guessed wrong, so we keep Rick's guess and the box with the portal gun
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
