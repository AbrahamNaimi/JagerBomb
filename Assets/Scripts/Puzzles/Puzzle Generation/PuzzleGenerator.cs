using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

namespace Puzzles.Puzzle_Generation
{
    public class PuzzleGenerator
    {
        private CaesarCipherEncoder _caesarCipherEncoder;
        
        // puzzleSlot 5 contains the timer and should always be active, can be excluded later but is left for now in case random puzzle slot allocation will be implemented
        private int[] _easyPuzzles = { 0, 1, 5 }; 
        private int[] _mediumPuzzles = { 0, 1, 2, 3, 5 };
        private int[] _hardPuzzles = { 0, 1, 2, 3, 4, 5 };

        public PuzzleGenerator()
        {
            PlayerPrefs.SetInt("Drunkness", (int)Drunkness.Light);
            PlayerPrefs.Save();
        }

        public void SetPuzzles(GameObject[] puzzleSlots)
        {
            int[] toBeActivatedPuzzles = new int[] { };
            switch ((Drunkness)PlayerPrefs.GetInt("Drunkness"))
            {
                case Drunkness.Light:
                    toBeActivatedPuzzles = _easyPuzzles;
                    break;
                case Drunkness.Medium:
                    toBeActivatedPuzzles = _mediumPuzzles;
                    break;
                case Drunkness.Heavy:
                    toBeActivatedPuzzles = _hardPuzzles;
                    break;
            }

            foreach (var (puzzleSlot, index) in puzzleSlots.Select((slot, index) => (slot, index)))
            {
                if (!toBeActivatedPuzzles.Contains(index))
                {
                    puzzleSlot.SetActive(false);
                }
            }
        }
    }
}