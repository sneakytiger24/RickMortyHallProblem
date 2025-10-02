using ConsoleTables;
ParamsParser.ValidateArguments(args);
GameCore game = new GameCore(ParamsParser.BoxNumber, ParamsParser.MortyType);

// Check if auto mode is requested
if (args.Length >= 3 && args[2].ToLower() == "auto")
{
    game.PlayGameAuto();
}
else
{
    game.PlayGame();
}