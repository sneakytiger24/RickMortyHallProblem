class StatisticsAccumulator
{
    private int SwitchedRounds = 0;
    private int SwitchedWins = 0;
    private int StayedRounds = 0;
    private int StayedWins = 0;

    public void RecordRound(bool isWin, bool isSwitched)
    {
        if (isSwitched)
        {
            SwitchedRounds++;
            if (isWin) SwitchedWins++;
        }
        else
        {
            StayedRounds++;
            if (isWin) StayedWins++;
        }
    }

    public string[] CalculateEstimateProbability()
    {
        string switchedProb = SwitchedRounds == 0 ? "?" :
            ((double)SwitchedWins / (double)SwitchedRounds * 100) + "%";

        string stayedProb = StayedRounds == 0 ? "?" :
            ((double)StayedWins / (double)StayedRounds * 100) + "%";

        return [switchedProb, stayedProb];
    }
}