using System;
using System.Collections.Generic;
using Puzzles;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class BombManager : MonoBehaviour
{
    public Camera camera;
    public GameObject[] puzzleSlots;
    public GameObject isSolvedLight;
    public bool isBombSolved {get; private set;} = false;

    private Vector3 screenPoint;
    private Vector3 offset;
    private InputActions _inputActions;
    private List<IPuzzle> _puzzleControllers = new List<IPuzzle>();

    void Start()
    {
        GetPuzzles();
        GetPuzzleScripts();
        _inputActions.UI.Click.performed += ctx => MouseRaycast(ctx);
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
            if (puzzleSlot.GetComponent<ButtonPuzzleController>() != null)
            {
                _puzzleControllers.Add(puzzleSlot.GetComponent<ButtonPuzzleController>());
            }
        }
    }

    bool IsBombDefused()
    {
        bool puzzlesSolved = true;
        foreach (var puzzleController in _puzzleControllers)
        {
            print(puzzleController.isPuzzleSolved);
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
         ButtonPuzzleController bombPuzzleScript = hitCollider.transform.parent.GetComponent<ButtonPuzzleController>();
         if (bombPuzzleScript != null && hitCollider.name.Contains("Button"))
         {
             bombPuzzleScript.ButtonPressed(hitCollider.gameObject);
         }
    }
}
