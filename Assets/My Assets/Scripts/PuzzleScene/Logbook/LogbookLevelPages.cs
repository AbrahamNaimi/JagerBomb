namespace My_Assets.PuzzleScene.Logbook
{
    public static class LogbookLevelPages
    {
        public static LogBookPage GetPageForLevel(int level, int drunkness)
        {
            var lore = LevelLore.GetLore(level, drunkness);
            return new LogBookPage(lore.title, lore.title, lore.content);
        }
    }
}
