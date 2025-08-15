using UnityEngine;

namespace Puzzles
{
    public class InventoryManager : MonoBehaviour
    {
        public GameObject inventory;
        
        private CanvasGroup _inventoryCanvas;
        private bool _logbookOpen = false;
        private InputActions _inputActions;
    
        void Start()
        {
            _inventoryCanvas = inventory.GetComponent<CanvasGroup>();
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
            _inventoryCanvas.alpha = _logbookOpen ? 1 : 0;
        }
    }
}
