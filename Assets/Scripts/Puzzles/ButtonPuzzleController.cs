using System;
using System.Collections.Generic;
using System.Linq;
using Puzzles.Logbook;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Puzzles
{
    public class ButtonPuzzleController : MonoBehaviour, IPuzzle
    {
        public GameObject[] buttons;
        public GameObject[] displayLights;
        public bool IsPuzzleSolved { get; private set; } = false;
        public LogbookController logbookController;


        private List<Color> _buttonColorPossibilities = new()
            { Color.red, Color.green, Color.blue, Color.cyan, Color.hotPink, Color.gold, Color.violetRed };

        private List<int> _pressedButtons = new();

        private List<int> _solution = new() { 1, 2, 3, 4 };

        void Start()
        {
            SetButtonColors();
            ClearDisplayLights();
            GenerateLogbookPage();
        }

        public void ObjectClicked(GameObject hitGameObject)
        {
            if (hitGameObject.name.Contains("Reset"))
            {
                ClearDisplayLights();
                return;
            }

            Color buttonColor = hitGameObject.GetComponent<Renderer>().material.color;
            int buttonIndex = 0;
            try
            {
                buttonIndex = Int32.Parse(hitGameObject.name.Split(" ")?[1]);
            }
            catch (NullReferenceException e)
            {
                Debug.LogError("Button name must use syntax 'Button {ButtonIndex} \n'" + e.Message);
                return;
            }


            if (_pressedButtons.Contains(buttonIndex)) return;

            displayLights[_pressedButtons.Count].GetComponent<Renderer>().material.color = buttonColor;
            _pressedButtons.Add(buttonIndex);

            SetIsPuzzleSolved();
        }

        private void SetButtonColors()
        {
            foreach (GameObject button in buttons)
            {
                int colorIndex = Random.Range(0, _buttonColorPossibilities.Count);
                button.GetComponent<Renderer>().material.color = _buttonColorPossibilities[colorIndex];
                _buttonColorPossibilities.RemoveAt(colorIndex);
            }
        }

        private void GenerateLogbookPage()
        {
            logbookController.AddPage(new LogBookPage("Colour button puzzle", "Colour button puzzle",
                "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic type"));
        }

        private void ClearDisplayLights()
        {
            foreach (GameObject displayLight in displayLights)
            {
                displayLight.GetComponent<Renderer>().material.color = Color.clear;
            }

            IsPuzzleSolved = false;

            _pressedButtons = new();
        }

        private void SetIsPuzzleSolved()
        {
            IsPuzzleSolved = _pressedButtons.SequenceEqual(_solution);
        }
    }
}