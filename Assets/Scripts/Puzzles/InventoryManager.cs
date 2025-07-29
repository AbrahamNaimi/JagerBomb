using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject logBook;
    private bool _logbookOpen = false;
    private InputActions _inputActions;
    
    void Start()
    {
        _inputActions.Puzzle.ToggleLogBook.performed += ctx => ToggleLogbook();
    }

    private void Awake()
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

    private void ToggleLogbook()
    {
        _logbookOpen = !_logbookOpen;
        logBook.SetActive(_logbookOpen);
    }
}
