using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Puzzles.Puzzle_Generation
{
    public class CaesarCipherEncoder
    {
        private List<string> _easyCodeWords = new() { "BOMBS", "WICK", "WIRE", "MINE", "ATOM" };
        private List<string> _mediumCodeWords = new() { "RETREE", "REFEED" };
        private List<string> _hardCodeWords = new() { "RETREE", "REFEED", "TETHER", "CREEPER", "PEPPERED" };
        private Drunkness _drunkness = (Drunkness)PlayerPrefs.GetInt("Drunkness");
        private string _caesarEncoding = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public string EncodeCaesarCipher()
        {
            (string codeWord, int shiftValue) = GetEncodingVariables();

            string encodedWord = "";
            foreach (char c in codeWord)
            {
                int caesarCharIndex = _caesarEncoding.IndexOf(c) + shiftValue;
                encodedWord += _caesarEncoding[caesarCharIndex % _caesarEncoding.Length];
            }

            Debug.Log((codeWord, shiftValue));
            return encodedWord;
        }

        private (string codeWord, int shiftValue) GetEncodingVariables()
        {
            Random rand = new Random();
            int shiftValue = 0;
            string codeWord = "";

            switch (_drunkness)
            {
                case Drunkness.Light:
                    shiftValue = rand.Next(1, 5);
                    codeWord = _easyCodeWords[rand.Next(0, _easyCodeWords.Count)];
                    break;
                case Drunkness.Medium:
                    shiftValue = rand.Next(5, 10);
                    codeWord = _mediumCodeWords[rand.Next(0, _mediumCodeWords.Count)];
                    break;
                case Drunkness.Heavy:
                    shiftValue = rand.Next(10, 15);
                    codeWord = _hardCodeWords[rand.Next(0, _hardCodeWords.Count)];
                    break;
            }

            return (codeWord, shiftValue);
        }
    }
}