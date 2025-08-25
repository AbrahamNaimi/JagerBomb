using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Puzzles.Controllers;
using Puzzles.Puzzle_Generation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Puzzles
{
    [RequireComponent(typeof(BoxCollider))]
    public class BombManager : MonoBehaviour
    {
        public Camera mainCamera;
        public Camera timerCamera;
        public GameObject explosion;
        public GameObject[] puzzleSlots;
        public GameObject isSolvedLight;
        public InventoryManager inventoryManager;
        public bool IsBombSolved { get; private set; } = false;
        public float cameraDistanceToPuzzle = 0.75f;
        public TimerController timerController;
        public GameObject puzzleEndScreen;
        public float[] timePerLevelSeconds = { 120f, 180f,240f };

        private Vector3 _screenPoint;
        private Vector3 _offset;
        private InputActions _inputActions;
        private List<IPuzzle> _puzzleControllers = new ();

        private Vector3 _cameraStartPosition;
        [CanBeNull] private IPuzzle _currentPuzzle;

        public PuzzleGenerator PuzzleGenerator;

        void Start()
        {
            _inputActions.UI.Click.performed += ctx => MouseRaycast(ctx);
        
            PuzzleGenerator = new PuzzleGenerator();
            PuzzleGenerator.SetPuzzles(puzzleSlots);
            GetPuzzles();
            GetPuzzleScripts();
            SetIsSolved(false);
        
            _cameraStartPosition = mainCamera.transform.position;
            timerCamera.enabled = false;
            
            timerController.StartTimer(timePerLevelSeconds[PlayerPrefs.GetInt("Level") - 1]);
        }

        void Update()
        {
            if (IsBombDefused() != IsBombSolved)
            {
                IsBombSolved = IsBombDefused();
                SetIsSolved(IsBombSolved);
            }
        }

        void Awake()
        {
            _inputActions = new InputActions();
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        public void ExplodeBomb()
        {
            timerCamera.enabled = false;
            mainCamera.enabled = false;
            inventoryManager.inventory.SetActive(false);
            explosion.SetActive(true);
            puzzleEndScreen.SetActive(true);
        }

        private void GetPuzzleScripts()
        {
            foreach (var puzzleSlot in puzzleSlots)
            {
                if (puzzleSlot.GetComponent<IPuzzle>() != null)
                {
                    _puzzleControllers.Add(puzzleSlot.GetComponent<IPuzzle>());
                }
            }
        }

        private bool IsBombDefused()
        {
            bool puzzlesSolved = true;
            foreach (var puzzleController in _puzzleControllers)
            {
                if (!puzzleController.IsPuzzleSolved)
                {
                    puzzlesSolved = false;
                    break;
                }
            }

            return puzzlesSolved;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void SetIsSolved(bool isSolved)
        {
            if (isSolved)
            {
                timerController.PauseTimer();
                isSolvedLight.GetComponent<Renderer>().material.color = Color.green;
                puzzleEndScreen.SetActive(true);
            }
            else
            {
                isSolvedLight.GetComponent<Renderer>().material.color = Color.red;
            }
        }

        private void GetPuzzles()
        {
            puzzleSlots = GameObject.FindGameObjectsWithTag("Puzzle");
            Array.Sort(puzzleSlots, (a, b) => String.Compare(a.name, b.name, StringComparison.Ordinal));
        }

        private void MouseRaycast(InputAction.CallbackContext context)
        {
            if (inventoryManager.LogbookOpen || IsBombSolved) return;
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit))
            {
                HandlePuzzleClick(hit.collider);
            }
        }

        private void HandlePuzzleClick(Collider hitCollider)
        {
            IPuzzle clickedPuzzle = hitCollider.gameObject.GetComponentInParent<IPuzzle>();

            if (clickedPuzzle == _currentPuzzle)
            {
                HandlePuzzleItemClick(hitCollider);
                return;
            }

            if (clickedPuzzle == null)
            {
                _currentPuzzle = null;
                mainCamera.transform.position = _cameraStartPosition;
                timerCamera.enabled = false;
                return;
            }

            _currentPuzzle = clickedPuzzle;
            Vector3 puzzlePosition = (clickedPuzzle as MonoBehaviour)!.gameObject.transform.position;
            mainCamera.transform.position =
                new Vector3(puzzlePosition.x, puzzlePosition.y, puzzlePosition.z + cameraDistanceToPuzzle);
            timerCamera.enabled = true;
        }

        private void HandlePuzzleItemClick(Collider hitCollider)
        {
            IPuzzle puzzleScript = hitCollider.transform.parent.GetComponent<IPuzzle>();
            switch (puzzleScript)
            {
                case ButtonPuzzleController puzzleController:
                    puzzleController.ObjectClicked(hitCollider.gameObject);
                    break;
                case CaesarCipherPuzzleController puzzleController:
                    puzzleController.ObjectClicked(hitCollider.gameObject);
                    break;
                case MazePuzzleController puzzleController:
                    puzzleController.ObjectClicked(hitCollider.gameObject);
                    break;
                case WireCutPuzzleController puzzleController:
                    puzzleController.ObjectClicked(hitCollider.gameObject);
                    break;
            }
        }
    }
}