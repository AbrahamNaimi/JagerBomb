using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace My_Assets.PuzzleScene.Puzzles.CaesarCipher
{
    public class CaesarCipherEncoder
    {
        private List<string> _easyCodeWords = new() { "BOMBS", "WICK", "WIRE", "MINE", "ATOM" };
        private List<string> _mediumCodeWords = new() { "RETREE", "REFEED" };
        private List<string> _hardCodeWords = new() { "RETREE", "REFEED", "TETHER", "CREEPER", "PEPPERED" };
        private Drunkness _drunkness = (Drunkness)PlayerPrefs.GetInt("Drunkness");
        private int _level = PlayerPrefs.GetInt("Level");
        private string _caesarEncoding = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private string _codeWord;
        private int _shiftValue;

        public string EncodeCaesarCipher()
        {
            GetEncodingVariables();

            string encodedWord = "";
            foreach (char c in _codeWord)
            {
                int caesarCharIndex = _caesarEncoding.IndexOf(c) + _shiftValue;
                encodedWord += _caesarEncoding[caesarCharIndex % _caesarEncoding.Length];
            }

            Debug.Log((_codeWord, _shiftValue));
            return encodedWord;
        }

        private void GetEncodingVariables()
        {
            Random rand = new Random();
            int shiftValue = 0;
            string codeWord = "";

            switch (_level)
            {
                case 1:
                    shiftValue = rand.Next(1, 5);
                    codeWord = _easyCodeWords[rand.Next(0, _easyCodeWords.Count)];
                    break;
                case 2:
                    shiftValue = rand.Next(5, 10);
                    codeWord = _mediumCodeWords[rand.Next(0, _mediumCodeWords.Count)];
                    break;
                case 3:
                    shiftValue = rand.Next(10, 15);
                    codeWord = _hardCodeWords[rand.Next(0, _hardCodeWords.Count)];
                    break;
            }
            _codeWord = codeWord;
            _shiftValue = shiftValue;
        }

        public string GenerateLogbookInstructions()
        {
            string cipherHelper = _drunkness != Drunkness.Light
                ? "During all the fun yesterday you forgot to write down the cipher values for each character. \n" +
                  $"Below you will find the order in which letters are encoded by caesar's cipher, good luck! \n {_caesarEncoding}"
                : $"Below you see the letter corresponding to each encoded letter: \n {GenerateCipherInstructions()}";

            string noSolutionIndicationWarning = _drunkness == Drunkness.Heavy
                ? "You seem too hungover to know if the input is correct. This will be tough \n"
                : "";
            
            return "The letters of the codeword have been encrypted. \n" +
                   $"Each letter is shifted by {_shiftValue} \n" +
                   $"The codeword is {_codeWord} \n" +
                   $"{noSolutionIndicationWarning}" +
                   $"{cipherHelper}";
        }

        private string GenerateCipherInstructions()
        {
            string cipherInstructions = "";
            foreach (var (c, i) in _caesarEncoding.Select((c, index) => (c, index)))
            {
                string seperator = i % 2 == 0 ? "         " : "\n";
                int caesarCharIndex = _caesarEncoding.IndexOf(c) + _shiftValue;
                
                cipherInstructions += $"{c} --- {_caesarEncoding[caesarCharIndex % _caesarEncoding.Length]} {seperator}" ;
            }

            return cipherInstructions;
        }
    }
}