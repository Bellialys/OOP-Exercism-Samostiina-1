using System.Collections.Generic;
using System.Linq;

public static class Camicia
{
    public enum GameStatus { Finished, Loop }
    public record GameResult(GameStatus Status, int Tricks, int Cards);

    private static int CardValue(string card) => card switch
    {
        "J" => 1, "Q" => 2, "K" => 3, "A" => 4, _ => 0
    };

    public static GameResult SimulateGame(string[] playerA, string[] playerB)
    {
        var decks = new[] { new Queue<int>(playerA.Select(CardValue)),
                            new Queue<int>(playerB.Select(CardValue)) };
        var seen = new HashSet<string>();
        int player = 0, tricks = 0, cards = 0;
        while (decks[0].Count > 0 && decks[1].Count > 0)
        {
            string state = $"{player}:{string.Join(",", decks[0])}|{string.Join(",", decks[1])}";
            if (!seen.Add(state)) return new GameResult(GameStatus.Loop, tricks, cards);
            tricks++;
            var pile = new Queue<int>();
            int penalty = 0;
            while (true)
            {
                if (decks[player].Count == 0)
                    return new GameResult(GameStatus.Finished, tricks, cards);
                int card = decks[player].Dequeue();
                pile.Enqueue(card);
                cards++;
                if (card > 0)
                {
                    penalty = card;
                    player = 1 - player;
                }
                else if (penalty > 0)
                {
                    if (--penalty == 0)
                    {
                        player = 1 - player;
                        while (pile.Count > 0) decks[player].Enqueue(pile.Dequeue());
                        break;
                    }
                }
                else player = 1 - player;
            }
        }
        return new GameResult(GameStatus.Finished, tricks, cards);
    }
}
