using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace My_Assets.Puzzles.Puzzle_Generation
{
    public class WireCutPuzzleGenerator
    {
        private readonly Dictionary<Color, List<string>> _riddlesByColor = new()
        {
            {
                Color.white, new List<string>
                {
                    "I hide in plain sight, pure and bare.",
                    "I reflect all yet claim no shade.",
                    "Snow and bone share my disguise.",
                    "The absence of color reveals me.",
                    "I shine brightest when untouched."
                }
            },
            {
                Color.blue, new List<string>
                {
                    "I mirror the endless sky.",
                    "Cold depths whisper my name.",
                    "Calm seas wear my coat.",
                    "I drown fire without sound.",
                    "Storms are born beneath me."
                }
            },
            {
                Color.green, new List<string>
                {
                    "I am a man who lives in a swamp and my best friend is a donkey, what is my skin color?",
                    "Fields and forests claim me.",
                    "I bloom when danger fades.",
                    "Nature’s favorite disguise.",
                    "I thrive where balance lives."
                }
            },
            {
                Color.yellow, new List<string>
                {
                    "I glow without heat.",
                    "The sun favors my tone.",
                    "Caution is my calling card.",
                    "Gold without value.",
                    "I warn before disaster."
                }
            },
            {
                Color.black, new List<string>
                {
                    "I swallow all light.",
                    "The end wears my color.",
                    "Silence follows my path.",
                    "I hide what should not be seen.",
                    "Darkness obeys me."
                }
            }
        };

        public List<Color> GenerateWireColors(int count)
        {
            return _riddlesByColor.Keys
                .OrderBy(_ => Random.value)
                .Take(count)
                .ToList();
        }

        public Dictionary<int, WireOutcome> GenerateOutcomeMap(int wireCount)
        {
            // first 2 correct, 1 explode, rest penalty
            Dictionary<int, WireOutcome> map = new();

            for (int i = 0; i < wireCount; i++)
            {
                if (i < 2) map[i] = WireOutcome.Correct;
                else map[i] = WireOutcome.WrongTimePenalty;
            }

            return map;
        }

        public string GenerateLogbookPage(List<Color> correctColors)
        {
            List<string> riddles = new();

            foreach (Color c in correctColors)
            {
                var list = _riddlesByColor[c];
                riddles.Add(list[Random.Range(0, list.Count)]);
            }

            return
                "Two wires must be cut. What color you may ask? That's easy!!\n\n" +
                "Just solve the riddles the answer to each is a color:\n" +
                $"- {riddles[0]}\n" +
                $"- {riddles[1]}\n\n" +
                "Not every mistake is forgiven.";
        }
    }
}
