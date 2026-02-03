using UnityEngine;

namespace My_Assets.PuzzleScene.Logbook
{
    public class InventoryManager : MonoBehaviour
    {
        public GameObject inventory;
        
        private CanvasGroup _inventoryCanvas;
        public bool LogbookOpen {get; private set;}
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
            LogbookOpen = !LogbookOpen;
            _inventoryCanvas.alpha = LogbookOpen ? 1 : 0;
        }
    }
}
