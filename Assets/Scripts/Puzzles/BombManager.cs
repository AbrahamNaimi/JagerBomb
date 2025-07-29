using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Puzzles;
using Puzzles.Puzzle_Generation;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class BombManager : MonoBehaviour
{
    public Camera camera;
    public Camera timerCamera;
    public GameObject[] puzzleSlots;
    public GameObject isSolvedLight;
    public bool isBombSolved {get; private set;} = false;
    public float cameraDistanceToPuzzle = 3.0f;

    private Vector3 screenPoint;
    private Vector3 offset;
    private InputActions _inputActions;
    private List<IPuzzle> _puzzleControllers = new List<IPuzzle>();
    
    private Vector3 _cameraStartPosition;
    [CanBeNull] private IPuzzle _currentPuzzle;
    
    public PuzzleGenerator PuzzleGenerator;

    void Start()
    {
        PuzzleGenerator =  new PuzzleGenerator();
        PuzzleGenerator.SetPuzzles(puzzleSlots);
        GetPuzzles();
        GetPuzzleScripts();
        SetIsSolvedLight(false);
        _inputActions.UI.Click.performed += ctx => MouseRaycast(ctx);
        
        _cameraStartPosition = camera.transform.position;
        timerCamera.enabled = false;
    }

    void Update()
    {
        if (IsBombDefused() != isBombSolved)
        {
            isBombSolved = IsBombDefused();
            SetIsSolvedLight(isBombSolved);
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

    void GetPuzzleScripts()
    {
        foreach (var puzzleSlot in puzzleSlots)
        {
            if (puzzleSlot.GetComponent<IPuzzle>() != null)
            {
                _puzzleControllers.Add(puzzleSlot.GetComponent<IPuzzle>());
            }
        }
    }

    bool IsBombDefused()
    {
        bool puzzlesSolved = true;
        foreach (var puzzleController in _puzzleControllers)
        {
            if (!puzzleController.isPuzzleSolved)
            {
                puzzlesSolved = false;
                break;
            }
        }
        return puzzlesSolved;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void SetIsSolvedLight(bool isSolved)
    {
        if (isSolved)
        {
            isSolvedLight.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            isSolvedLight.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    void GetPuzzles()
    {
        puzzleSlots = GameObject.FindGameObjectsWithTag("Puzzle");
        Array.Sort(puzzleSlots, (a, b) => String.Compare(a.name, b.name, StringComparison.Ordinal));
    }

    void MouseRaycast(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit))
        {
            HandlePuzzleClick(hit.collider);
        }
    }

    void HandlePuzzleClick(Collider hitCollider)
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
            camera.transform.position = _cameraStartPosition;
            timerCamera.enabled = false;
            return;
        }
        _currentPuzzle = clickedPuzzle;
        Vector3 puzzlePosition = (clickedPuzzle as MonoBehaviour)!.gameObject.transform.position;
        camera.transform.position = new  Vector3(puzzlePosition.x, puzzlePosition.y, puzzlePosition.z - cameraDistanceToPuzzle);
        timerCamera.enabled = true;
    }
    
    void HandlePuzzleItemClick(Collider hitCollider)
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
        }
    }
}
