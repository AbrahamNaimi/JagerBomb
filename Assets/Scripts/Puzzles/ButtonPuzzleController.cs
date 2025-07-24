using System;
using System.Collections.Generic;
using System.Linq;
using Puzzles;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class ButtonPuzzleController : MonoBehaviour, IPuzzle
{
    public GameObject[] buttons;
    public GameObject[] displayLights;

    private List<Color> _buttonColorPossibilities = new()
        { Color.red, Color.green, Color.blue, Color.cyan, Color.hotPink, Color.gold, Color.violetRed };

    private List<int> _pressedButtons = new();

    // Will be used for moving button when clicked
    private Vector3 _buttonClickedOffset = new Vector3(0f, 0f, 0.05f);
    private float _buttonMoveDuration = 0.2f;
    private Vector3 _buttonOriginalPosition;
    private bool _buttonIsMoving;

    private List<int> solution = new() { 1, 2, 3, 4 };
    public bool isPuzzleSolved { get; private set; } = false;


    void Start()
    {
        SetButtonColors();
    }

    void SetButtonColors()
    {
        foreach (GameObject button in buttons)
        {
            int colorIndex = Random.Range(0, _buttonColorPossibilities.Count);
            button.GetComponent<Renderer>().material.color = _buttonColorPossibilities[colorIndex];
            _buttonColorPossibilities.RemoveAt(colorIndex);
        }
    }

    public void ButtonPressed(GameObject button)
    {
        if (button.name.Contains("Reset"))
        {
            ClearDisplayLights();
            return;
        }

        Color buttonColor = button.GetComponent<Renderer>().material.color;
        int buttonIndex = 0;
        try
        {
            buttonIndex = Int32.Parse(button.name.Split(" ")?[1]);
        }
        catch (Exception e)
        {
            Debug.LogError("Button name must use syntax 'Button {ButtonIndex} \n'" + e.Message );
            return;
        }
        

        if (_pressedButtons.Contains(buttonIndex)) return;

        displayLights[_pressedButtons.Count].GetComponent<Renderer>().material.color = buttonColor;
        _pressedButtons.Add(buttonIndex);
        
        SetIsPuzzleSolved();
    }

    private void ClearDisplayLights()
    {
        foreach (GameObject displayLight in displayLights)
        {
            displayLight.GetComponent<Renderer>().material.color = Color.clear;
        }
        isPuzzleSolved = false;

        _pressedButtons = new();
    }

    private void SetIsPuzzleSolved()
    {
        isPuzzleSolved = _pressedButtons.SequenceEqual(solution);
    }
}