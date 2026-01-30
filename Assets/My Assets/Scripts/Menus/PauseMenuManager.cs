using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace My_Assets.Menus
{
    public class PauseMenuManager : MonoBehaviour
    {
        [Header("References")]
        public GameObject pauseMenuCanvas;
        public PlayerInput playerInput;

        private bool isPaused = false;
        private CursorLockMode previousCursorState;

        [Header("Player Input Actions")]
        public InputActionReference moveAction;
        public InputActionReference lookAction;
        public InputActionReference jumpAction;
        public InputActionReference pauseAction;
        private static PauseMenuManager instance;

        [SerializeField] private bool shouldLock;

        void Start()
        {
            if (pauseMenuCanvas != null)
                pauseMenuCanvas.SetActive(false);

            LockCursor();
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject); // prevent duplicates
            }
        }


        public void SetupPauseMenuForScene()
        {
            if (pauseMenuCanvas != null)
                pauseMenuCanvas.SetActive(false);

            pauseAction?.action.Enable();
        }


        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            if (pauseAction != null)
            {
                pauseAction.action.Enable();
                pauseAction.action.performed += OnPausePressed;
            }
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            if (pauseAction != null)
            {
                pauseAction.action.performed -= OnPausePressed;
            }
        }


        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Time.timeScale = 1f;
            isPaused = false;

            if (pauseMenuCanvas == null)
                pauseMenuCanvas = GameObject.Find("PauseMenuCanvas");

            if (pauseMenuCanvas != null)
                pauseMenuCanvas.SetActive(false);
        }


        private void OnPausePressed(InputAction.CallbackContext context)
        {
            TogglePause();
        }


        public void TogglePause()
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }

        private void PauseGame()
        {
            Time.timeScale = 0f;
            isPaused = true;
            pauseMenuCanvas?.SetActive(true);

            if (shouldLock)
            {
                moveAction?.action.Disable();
                lookAction?.action.Disable();
                jumpAction?.action.Disable();
                playerInput?.SwitchCurrentActionMap("UI");
                UnlockCursor();
            }
            previousCursorState = Cursor.lockState;

            AudioListener.pause = true;

        }

        private void ResumeGame()
        {
            Time.timeScale = 1f;
            isPaused = false;
            AudioListener.pause = false;

            pauseMenuCanvas?.SetActive(false);

            if (shouldLock)
            {
                playerInput?.SwitchCurrentActionMap("Player");
                moveAction?.action.Enable();
                lookAction?.action.Enable();
                jumpAction?.action.Enable();
                LockCursor();
            }
        }

        private void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void UnlockCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void OnResumeButton()
        {
            ResumeGame();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Main menu scene");
        }
    }
}
