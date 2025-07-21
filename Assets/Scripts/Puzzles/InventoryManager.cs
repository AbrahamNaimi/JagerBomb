using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject logBook;
    private bool _logbookOpen = false;
    private InputActions _inputActions;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputActions.Inventory.ToggleLogbook.performed += ctx => ToggleLogbook();
    }

    // Update is called once per frame
    void Update()
    {
        
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
