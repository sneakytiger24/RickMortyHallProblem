using ConsoleTables;
ParamsParser.ValidateArguments(args);
GameCore game = new GameCore(ParamsParser.BoxNumber, ParamsParser.MortyType);
game.PlayGame();