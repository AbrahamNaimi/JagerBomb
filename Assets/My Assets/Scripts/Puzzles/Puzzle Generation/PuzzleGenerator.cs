using System.Linq;
using UnityEngine;

namespace My_Assets.Puzzles.Puzzle_Generation
{
    public class PuzzleGenerator
    {
        private CaesarCipherEncoder _caesarCipherEncoder;

        // puzzleSlot 5 contains the timer and should always be active, can be excluded later but is left for now in case random puzzle slot allocation will be implemented
        private readonly int[] _easyPuzzles = { 0, 1, 5 };
        private readonly int[] _mediumPuzzles = { 0, 1, 3, 5 };
        private readonly int[] _hardPuzzles = { 0, 1, 3, 4, 5 };

        public void SetPuzzles(GameObject[] puzzleSlots)
        {
            int[] toBeActivatedPuzzles = { };
            switch (PlayerPrefs.GetInt("Level"))
            {
                case 1:
                    toBeActivatedPuzzles = _easyPuzzles;
                    break;
                case 2:
                    toBeActivatedPuzzles = _mediumPuzzles;
                    break;
                case 3:
                    toBeActivatedPuzzles = _hardPuzzles;
                    break;
            }

            foreach (var (puzzleSlot, index) in puzzleSlots.Select((slot, index) => (slot, index)))
            {
                if (toBeActivatedPuzzles.Contains(index))
                {
                    puzzleSlot.SetActive(true);
                }
            }
        }
    }
}