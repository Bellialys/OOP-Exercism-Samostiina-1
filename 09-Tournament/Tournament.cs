using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

public static class Tournament
{
    private class TeamStats
    {
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int Matches => Wins + Draws + Losses;
        public int Points => Wins * 3 + Draws;
    }
    public static void Tally(Stream inStream, Stream outStream)
    {
        var teams = new Dictionary<string, TeamStats>();
        using var reader = new StreamReader(inStream, Encoding.UTF8, leaveOpen: true);
        using var writer = new StreamWriter(outStream, new UTF8Encoding(false), leaveOpen: true);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            string[] parts = line.Split(';');
            string team1 = parts[0], team2 = parts[1], result = parts[2];
            if (!teams.ContainsKey(team1)) teams[team1] = new TeamStats();
            if (!teams.ContainsKey(team2)) teams[team2] = new TeamStats();
            switch (result)
            {
                case "win": teams[team1].Wins++; teams[team2].Losses++; break;
                case "loss": teams[team1].Losses++; teams[team2].Wins++; break;
                case "draw": teams[team1].Draws++; teams[team2].Draws++; break;
            }
        }
        writer.Write("Team                           | MP |  W |  D |  L |  P");
        foreach (var team in teams.OrderByDescending(t => t.Value.Points).ThenBy(t => t.Key, StringComparer.Ordinal))
        {
            TeamStats stats = team.Value;
            writer.Write($"\n{team.Key,-31}| {stats.Matches,2} | {stats.Wins,2} | {stats.Draws,2} | {stats.Losses,2} | {stats.Points,2}");
        }
        writer.Flush();
    }
}
