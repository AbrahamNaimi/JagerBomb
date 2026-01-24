using My_Assets.Puzzles.Logbook;

public static class LogbookLevelPages
{
    public static LogBookPage GetPageForLevel(int level)
    {
        var lore = LevelLore.GetLore(level);
        return new LogBookPage(lore.title, lore.title, lore.content);
    }
}
