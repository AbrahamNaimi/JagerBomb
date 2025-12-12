using System.Collections.Generic;
using Puzzles.Puzzle_Generation;
using UnityEngine;

namespace My_Assets.Puzzles.Puzzle_Generation
{
    public static class Colors
    {
        public static readonly Dictionary<Color, string> NameMap = new()
        {
            { Color.aquamarine, "aquamarine" },
            { Color.beige, "beige" },
            { Color.blanchedAlmond, "blanchedAlmond" },
            { Color.blue, "blue" },
            { Color.cyan, "cyan" },
            { Color.darkOrange, "darkOrange" },
            { Color.deepPink, "deepPink" },
            { Color.gold, "gold" },
            { Color.green, "green" },
            { Color.hotPink, "hotPink" },
            { Color.limeGreen, "limeGreen" },
            { Color.magenta, "magenta" },
            { Color.maroon, "maroon" },
            { Color.olive, "olive" },
            { Color.orange, "orange" },
            { Color.pink, "pink" },
            { Color.purple, "purple" },
            { Color.red, "red" },
            { Color.softYellow, "softYellow" },
            { Color.steelBlue, "steelBlue" },
            { Color.teal, "teal" },
            { Color.yellow, "yellow" },
            { Color.yellowGreen, "yellowGreen" }
        };
    }
    
    public class ButtonPuzzleGenerator
    {
        private readonly List<Color[]> _lightDrunknessColors = new()
        {
            new[] { Color.red, Color.green, Color.blue, Color.yellow },
            new[] { Color.orange, Color.purple, Color.cyan, Color.magenta },
            new[] { Color.darkOrange, Color.teal, Color.purple, Color.olive },
            new[] { Color.cyan, Color.deepPink, Color.yellow, Color.limeGreen },
        };

        private readonly List<Color[]> _mediumDrunknessColors = new()
        {
            new[] { Color.blue, Color.cyan, Color.red, Color.hotPink },
            new[] { Color.beige, Color.darkOrange, Color.yellowGreen, Color.gold },
            new[] { Color.pink, Color.magenta, Color.blue, Color.cyan },
        };

        private readonly List<Color[]> _heavyDrunknessColors = new()
        {
            new[] { Color.steelBlue, Color.blue, Color.teal, Color.aquamarine },
            new[] { Color.yellow, Color.yellowGreen, Color.softYellow, Color.blanchedAlmond },
            new[] { Color.red, Color.hotPink, Color.purple, Color.maroon }
        };
        
        private Drunkness _drunkness;

        private Drunkness GetDrunkness()
        {
            if (_drunkness == 0)
            {
                _drunkness = (Drunkness)PlayerPrefs.GetInt("Drunkness");
            }
            return _drunkness;
        }

        public Color[] GenerateButtonColors()
        {
            List<Color[]> availableColors;
            switch (GetDrunkness())
            {
                case Drunkness.Heavy:
                    availableColors = _heavyDrunknessColors;
                    break;
                case Drunkness.Medium:
                    availableColors = _mediumDrunknessColors;
                    break;
                default:
                    availableColors = _lightDrunknessColors;
                    break;
            }

            return availableColors[Random.Range(0, availableColors.Count)];
        }

        public string GenerateLogBookPage(Color[] buttonColors, float buttonFlashSpeed)
        {
            List<string> buttonColorNames = new();
            foreach (Color color in buttonColors)
            {
                buttonColorNames.Add($"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{Colors.NameMap[color]}</color>");
            }

            string drunknessWarning = GetDrunkness() != Drunkness.Light ? "The colors are a bit fuzzy. Did you have a little too much fun yesterday?" : "";

            string logBookPage = "One of the four buttons will flash initially. \n" +
                                 "Using the table below, find out which button should be pressed. \n" +
                                 "The same button as before will flash, followed by another. Repeat the button presses in the corresponding order. \n" +
                                 "The display at the top will show how many sequences have been completed, and how many are still to come. \n" +
                                 $"The sequence will lengthen every time, making a mistake will cost you {buttonFlashSpeed} seconds and reset the sequence. \n \n" +
                                 $"Flashing button: {buttonColorNames[0]}   {buttonColorNames[1]}  {buttonColorNames[2]}   {buttonColorNames[3]} \n" +
                                 $"Button to press: {buttonColorNames[1]}   {buttonColorNames[0]}  {buttonColorNames[3]}   {buttonColorNames[2]} \n \n" +
                                 $"{drunknessWarning}";
            return logBookPage;
        }

        public List<int> GetSolution()
        {
            int solutionLength = PlayerPrefs.GetInt("Level") == 1 ? 3 : 4;
            List<int> solution = new();
            int previous = -1;

            for (int i = 1; i <= solutionLength; i++)
            {
                int next;
                do
                {
                    next = Random.Range(0, 4);
                } while (next == previous);
                
                solution.Add(next);
                previous = next;
            }
            
            return solution;
        }
    }
}