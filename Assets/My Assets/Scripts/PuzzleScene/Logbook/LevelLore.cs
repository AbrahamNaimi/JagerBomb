using System.Collections.Generic;

namespace My_Assets.Puzzles.Logbook
{
    public static class LevelLore
    {
        private static readonly Dictionary<(int, int), (string title, string content)> loreByLevel = new()
        {
            { (1,0), ("Wakey wakey Mr alcoholic", "Yes who wrote this you may ask? Definitely not Alfred. Your name is Alfred by the way, a great bomb defuser.... Well you used to be untill you got a drinking problem. Alfred I think you should quit the alcohol and focus more on defusing the bomb or else the whole building will explode! Go and fix your first puzzle, that's the first step in defusing the bomb. Oh by the way... Lay off the drinking, it's bad for you.") },
            { (1,1), ("Wakey wakey Mr alcoholic", "Yes who wrote this you may ask? Definitely not Alfred. Your name is Alfred by the way, a great bomb defuser.... Well you used to be untill you got a drinking problem. Alfred I think you should quit the alcohol and focus more on defusing the bomb or else the whole building will explode! Go and fix your first puzzle, that's the first step in defusing the bomb. Oh by the way... Lay off the drinking, it's bad for you.") },
            { (1,2), ("Wakey wakey Mr alcoholic", "Yes who wrote this you may ask? Definitely not Alfred. Your name is Alfred by the way, a great bomb defuser.... Well you used to be untill you got a drinking problem. Alfred I think you should quit the alcohol and focus more on defusing the bomb or else the whole building will explode! Go and fix your first puzzle, that's the first step in defusing the bomb. Oh by the way... Lay off the drinking, it's bad for you.") },
            { (2,0), ("Average Joe", "Whoaaa restraint is a beautiful thing, you just had a tiny drink. Makes you very boring. Makes long nights feel... Long. The puzzles are waiting for you mr. boring.") },
            { (2,2), ("Close but not close enough", "OKAY ALFRED. NOW THAT YOUR LIFE IS AT STAKE YOU TRY TO SAY BYE TO YOUR FRIEND JACK? In all seriousness drinking on the job is not normal. Go solve those damn puzzles and try to drink even less tomorrow.") },
            { (2,3), ("Mr world WILD", "You crazy crazy man. Wild day yesterday huh? Honestly I did not expect you to make it. Good on you for drinking... Less.. Now keep that focus and solve the next puzzles. It will be a blast! :DD! No pun intended.") },
            { (3,0), ("The final arc", "I'm sobering up. Turns out Alfred... Is me ..... One of the bad things about drinking is that you can get blackout wasted. I'm writing this down as to remember that I'm almost there. The last day. I HAVE TO DEFUSE THE BOMB!!! VIVA LA REHAB!!!!!") },
            { (3,1), ("Dangerous games", "You tried Alfred. Fine. I'll give you a hint as to who I am. You know me very well. Better than I know myself at the moment. Maybe after these final puzzles you will get slightly better at riddles. Goodluck. Sincerely Your best friend and Jack ~ Daniels.") },
            { (3,2), ("The final arc", "I DONT FEEL SO GOOD. TOO MUCH BOOZE. ALFRED YOU ARE ME. I WISH I HAD STOPPED DRINKING I DO NOT KNOW IF YOU WILL BE ABLE TO DEFUSE THE BOMB AFTER I HAVE BEEN DRINKING ALL DARN NIGHT AGAIN. STOP DRINKING PLEASE. SINCERELY.... YOU!! ALFRED") },
        };

        public static (string title, string content) GetLore(int level, int drunkness)
        {
            if (loreByLevel.TryGetValue((level, drunkness), out var lore))
                return lore;
            return ($"Level {level}", "No lore defined for this level."); 
        }
    }
}
