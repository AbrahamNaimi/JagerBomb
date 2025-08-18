using System;
using System.Collections;
using System.Collections.Generic;
using Puzzles.Logbook;
using Puzzles.Puzzle_Generation;
using UnityEngine;

namespace Puzzles.Controllers
{
    public class ButtonPuzzleController : MonoBehaviour, IPuzzle
    {
        public GameObject[] buttons;
        public GameObject[] displayLights;
        public bool IsPuzzleSolved { get; private set; }
        public LogbookController logbookController;
        public TimerController timerController;
        public float buttonFlashSpeed = 0.5f;


        private List<Renderer> _buttonRenderers = new();
        private List<Renderer> _displayLightRenderers = new();
        private ButtonPuzzleGenerator _buttonPuzzleGenerator;
        private Color[] _buttonColors;

        private List<int> _pressedButtons = new();

        private List<int> _solution;
        private int _previousFlashingButton;
        private int _flashingButton;
        private bool _isFlashing;
        private float _timeSinceFlash;
        private int _currentCycle = 1;

        void Start()
        {
            GetButtonRenderers();
            _buttonPuzzleGenerator = new();
            _solution = _buttonPuzzleGenerator.GetSolution();
            _buttonColors = _buttonPuzzleGenerator.GenerateButtonColors();
            SetButtons();
            SetFlashingButton(_solution[_pressedButtons.Count]);
            SetDisplayLights();
            GenerateLogbookPage();
        }

        void Update()
        {
            _timeSinceFlash += Time.deltaTime;
            if (_timeSinceFlash > buttonFlashSpeed)
            {
                _timeSinceFlash = 0;
                FlashButton();
            }
        }

        public void ObjectClicked(GameObject hitGameObject)
        {
            if (IsPuzzleSolved) return;
            string objectName = hitGameObject.name;

            if (objectName == "Reset Button")
            {
                _currentCycle = 1;
                ClearDisplayLights();
                _pressedButtons.Clear();
                SetFlashingButton(_solution[_pressedButtons.Count]);
                return;
            }

            int buttonIndex;
            try
            {
                buttonIndex = Int32.Parse(hitGameObject.name.Split(" ")![1]);
            }
            catch (NullReferenceException e)
            {
                Debug.LogError("Button name must use syntax 'Button {ButtonIndex}' \n" + e.Message);
                return;
            }

            _pressedButtons.Add(buttonIndex);

            CheckCorrectButtonPressed();
        }

        private void GetButtonRenderers()
        {
            foreach (var button in buttons)
            {
                _buttonRenderers.Add(button.GetComponent<Renderer>());
            }
        }

        private void SetButtons()
        {
            for (int i = 0; i < _buttonRenderers.Count; i++)
            {
                _buttonRenderers[i].material.color = _buttonColors[i];
            }
        }

        private void GenerateLogbookPage()
        {
            logbookController.AddPage(new LogBookPage("Colour button puzzle", "Colour button puzzle",
                _buttonPuzzleGenerator.GenerateLogBookPage(_buttonColors, buttonFlashSpeed)));
        }

        private void SetDisplayLights()
        {
            if (_solution.Count == 3)
            {
                displayLights[3].SetActive(false);
            }

            foreach (var displayLight in displayLights)
            {
                _displayLightRenderers.Add(displayLight.GetComponent<Renderer>());
            }

            ClearDisplayLights();
        }

        private void ClearDisplayLights()
        {
            foreach (Renderer displayLightRenderer in _displayLightRenderers)
            {
                displayLightRenderer.material.color = Color.clear;
            }

            IsPuzzleSolved = false;

            _pressedButtons = new();
        }

        private IEnumerator FlashRedDisplay()
        {
            ClearDisplayLights();

            foreach (Renderer displayLightRenderer in _displayLightRenderers)
            {
                displayLightRenderer.material.color = Color.red;
            }

            yield return new WaitForSeconds(buttonFlashSpeed);

            ClearDisplayLights();
        }

        private void SetFlashingButton(int buttonToBePressed)
        {
            switch (buttonToBePressed)
            {
                case 0:
                    _flashingButton = 1;
                    break;
                case 1:
                    _flashingButton = 0;
                    break;
                case 2:
                    _flashingButton = 3;
                    break;
                case 3:
                    _flashingButton = 2;
                    break;
                default:
                    _flashingButton = -1;
                    SetButtons();
                    break;
            }
        }

        private void FlashButton()
        {
            if (_flashingButton == -1) return;

            if (_previousFlashingButton != _flashingButton)
            {
                _buttonRenderers[_previousFlashingButton].material.color = _buttonColors[_previousFlashingButton];
                _previousFlashingButton = _flashingButton;
            }

            Color changeToColor = _buttonRenderers[_previousFlashingButton].material.color == new Color(0, 0, 0, 0)
                ? _buttonColors[_flashingButton]
                : Color.clear;

            _buttonRenderers[_previousFlashingButton].material.color = changeToColor;
        }

        private void CheckCorrectButtonPressed()
        {
            SetIsPuzzleSolved();
            if (IsPuzzleSolved) return;

            for (int i = 0; i < _pressedButtons.Count; i++)
            {
                if (_pressedButtons[i] != _solution[i] + 1)
                {
                    StartCoroutine(FlashRedDisplay());
                    _currentCycle = 1;
                    timerController.DeductTime(10f);
                    SetFlashingButton(_solution[_pressedButtons.Count]);
                    return;
                }
            }

            if (_pressedButtons.Count == _currentCycle)
            {
                _pressedButtons.Clear();
                displayLights[_currentCycle - 1].GetComponent<Renderer>().material.color = Color.green;
                _currentCycle++;
            }

            SetFlashingButton(_solution[_pressedButtons.Count]);
        }

        private void SetIsPuzzleSolved()
        {
            if (_pressedButtons.Count != _solution.Count) return;
            for (int i = 0; i < _pressedButtons.Count; i++)
            {
                if (_pressedButtons[i] - 1 != _solution[i]) return;
            }

            displayLights[_currentCycle - 1].GetComponent<Renderer>().material.color = Color.green;
            IsPuzzleSolved = true;
            SetFlashingButton(-1);
        }
    }
}