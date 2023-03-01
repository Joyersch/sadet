using System.Collections.Immutable;

namespace Sadet.Steam.DataObjects;

public static class LibraryExtension
{
    public static List<Game> Started(this List<Game> games)
    {
        games.RemoveAll(g => g.Completion == 0F);
        return games;
    }

    public static List<Game> Unfinished(this List<Game> games)
    {
        games.RemoveAll(g => g.Completion == 100F);
        return games;
    }

    public static List<Game> SortedByCompletion(this List<Game> game, bool asc)
    {
        game.Sort((g1, g2) =>
        {
            if (g1.Completion < g2.Completion) return asc ? -1 : 1;
            if (g1.Completion > g2.Completion) return asc ? 1 : -1;
            return 0;
        });
        return game;
    }

    public static List<Game> SortedByDifficulty(this List<Game> game, bool asc)
    {
        game.Sort((g1, g2) =>
        {
            if (g1.Difficulty < g2.Difficulty) return asc ? -1 : 1;
            if (g1.Difficulty > g2.Difficulty) return asc ? 1 : -1;
            return 0;
        });
        return game;
    }
    public static float? TotalCompletion(this List<Game> games)
        => games.Where(g => g.Completion is not null && g.Completion != 0F).Average(g => g.Completion ?? 0);
}