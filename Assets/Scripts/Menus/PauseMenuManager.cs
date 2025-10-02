using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    void Start()
    {
        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(false);

        LockCursor();
    }

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed += OnPausePressed;
            pauseAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPausePressed;
            pauseAction.action.Disable();
        }
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

        previousCursorState = Cursor.lockState;

        moveAction?.action.Disable();
        lookAction?.action.Disable();
        jumpAction?.action.Disable();

        playerInput?.SwitchCurrentActionMap("UI");

        UnlockCursor();
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuCanvas?.SetActive(false);

        playerInput?.SwitchCurrentActionMap("Player");

        moveAction?.action.Enable();
        lookAction?.action.Enable();
        jumpAction?.action.Enable();

        LockCursor();
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
