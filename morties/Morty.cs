abstract class Morty
{
    protected GameCore Game;
    protected int BoxCount { get; set; }
    protected int FairNumber { get; set; }
    protected int RickGuessNumber { get; set; }
    public Morty(GameCore game, int boxCount, int fairNumber)
    {
        this.Game = game;
        this.BoxCount = boxCount;
        this.FairNumber = fairNumber;
    }
    public void ReceiveRickGuess(int rickGuessNumber)
    {
        this.RickGuessNumber = rickGuessNumber;
    }
    abstract public int[] EliminateBoxes();
    abstract public string[] CalculateExactProbability();
}
