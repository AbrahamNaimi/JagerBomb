using System.Collections.Generic;
using UnityEditor.Rendering;

public static class LevelLore
{
    private static readonly Dictionary<int, (string title, string content)> loreByLevel = new()
    {
        { 1, ("Wakey wakey Mr alcoholic", "Yes who wrote this you may ask? Definitely not Alfred. Your name is Alfred by the way, a great bomb defuser.... Well you used to be untill you got a drinking problem. Alfred I think you should quit the alcohol and focus more on defusing the bomb or else the whole building will explode! Go and fix your first puzzle, that's the first step in defusing the bomb. Oh by the way... Lay off the drinking, it's bad for you.") },
        { 2, ("Mr world WILD", "You crazy crazy man. Wild day yesterday huh? Honestly I did not expect you to make it. Good on you for drinking... Less.. Now keep that focus and solve the next puzzles. It will be a blast! :DD! No pun intended.") },
        { 3, ("The final arc", "I'm sobering up. Turns out Alfred... Is me ..... One of the bad things about drinking is that you can get blackout wasted. I'm writing this down as to remember that I'm almost there. The last day. I HAVE TO DEFUSE THE BOMB!!! VIVA LA REHAB!!!!!") },
    };

    public static (string title, string content) GetLore(int level)
    {
        if (loreByLevel.TryGetValue(level, out var lore))
            return lore;
        return ($"Level {level}", "No lore defined for this level."); 
    }
}
