using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject logBook;
    private CanvasGroup logbookCanvas;
    private bool _logbookOpen = false;
    private InputActions _inputActions;
    
    void Start()
    {
        logbookCanvas = logBook.GetComponent<CanvasGroup>();
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
        logbookCanvas.alpha = _logbookOpen ? 1 : 0;
    }
}
