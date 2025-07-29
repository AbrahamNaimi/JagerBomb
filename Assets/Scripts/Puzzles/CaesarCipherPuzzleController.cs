using System.Collections.Generic;
using System.Linq;
using Puzzles;
using Puzzles.Puzzle_Generation;
using UnityEngine;
using Random = System.Random;

public class CaesarCipherPuzzleController : MonoBehaviour, IPuzzle
{
    public bool isPuzzleSolved { get; private set; } = false;
    public GameObject[] buttons;
    public TextMesh displayText;
    
    private CaesarCipherEncoder _caesarCipherEncoder;
    private string _encodedCodeword;
    private string _typedWord = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        _caesarCipherEncoder = new CaesarCipherEncoder();
        _encodedCodeword = _caesarCipherEncoder.EncodeCaesarCipher();
        print(_encodedCodeword);
        
        SetButtonText();
        SetDisplayText();
    }

    void SetButtonText()
    {
        Random rand = new System.Random();
        IEnumerable<char> distinctCodeWordLetters = _encodedCodeword.Distinct().OrderBy(c => rand.Next());
        
        
        foreach (var (letter, i) in distinctCodeWordLetters.Select((letter, index) => (letter, index)))
        {
            buttons[i].GetComponentInChildren<TextMesh>().text = letter.ToString();
        }
    }

    public void ObjectClicked(GameObject hitGameObject)
    {
        string buttonText = hitGameObject.GetComponentInChildren<TextMesh>().text;
        if (buttonText == "←")
        {
            if (_typedWord.Length == 0) return;
            _typedWord = _typedWord.Remove(_typedWord.Length - 1);
        }
        else
        {
            _typedWord += buttonText;
        }
        
        SetDisplayText();
        SetIsPuzzleSolved();
    }

    void SetDisplayText()
    {
        displayText.text = _typedWord;
    }

    void SetIsPuzzleSolved()
    {
        isPuzzleSolved = _encodedCodeword == _typedWord;
    }
}