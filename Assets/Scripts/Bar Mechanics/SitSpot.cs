using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using StarterAssets;
using Cinemachine;
using System.Collections;
using Controllers;
using Puzzles.Puzzle_Generation;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider))]
public class SitSpot : MonoBehaviour
{
    [Header("Seat Setup")]
    public Transform seatAnchor;                    // camera attaches here while seated

    [Header("References")]
    public CinemachineVirtualCamera vcam;
    public Transform playerFollowTarget;            // normal follow (e.g., PlayerCameraRoot)
    public GameObject playerCapsule;
    public GameObject capsuleVisual;                // optional: mesh child to hide
    public FirstPersonController fpc;               // stays enabled so LateUpdate runs
    public StarterAssetsInputs starterInputs;
    public Drinkable drinkable;
    public GameSceneManager sceneManager;
    public GameObject qteObject;
    public float qteInterval;

    [Header("UI Prompt")]
    public GameObject promptRoot;
    public TextMeshProUGUI promptText;

    [Header("Input")]
    public Key interactKey = Key.E;

    [Header("Fade UI")]
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 0.5f;

    // State
    bool playerInZone, isSitting;
    Vector3 savedPlayerPos;
    Quaternion savedPlayerRot;
    Transform savedFollow, savedLookAt;

    PlayerInput playerInput;                        // detect mouse vs gamepad
    float seatPitch, seatYaw;                       // seated look angles

    public bool IsSitting => isSitting;
    
    private Drunkness _currentDrunkness;
    private int _numberOfDrinkingImpulses;
    private int _pastDrunkImpulses;
    private float _timeSinceLastQTE = 7.0f;
    private GameObject _QTE;
    private GameObject _nextQTE;
    private bool _newSceneTriggered;

    void Start()
    {
        _currentDrunkness = (Drunkness) PlayerPrefs.GetInt("Drunkness");
        _numberOfDrinkingImpulses = PlayerPrefs.GetInt("Level") * 3;

        _QTE = Instantiate(qteObject, Vector3.zero, Quaternion.identity);
        _QTE.GetComponentInChildren<QuicktimeEventController>().sitSpot =  this;
    }
    
    void Awake()
    {
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;

        // Autofind common refs
        if (!playerCapsule)
        {
            var fp = FindAnyObjectByType<FirstPersonController>();
            if (fp) { fpc = fp; playerCapsule = fp.gameObject; }
        }
        if (!fpc && playerCapsule) fpc = playerCapsule.GetComponent<FirstPersonController>();
        if (!starterInputs && playerCapsule) starterInputs = playerCapsule.GetComponent<StarterAssetsInputs>();
        if (playerCapsule) playerInput = playerCapsule.GetComponent<PlayerInput>();
        if (!vcam) vcam = FindAnyObjectByType<CinemachineVirtualCamera>();
        if (!promptText) promptText = GetComponentInChildren<TextMeshProUGUI>(true);

        TogglePrompt(false, false);

        if (fadeCanvas) fadeCanvas.alpha = 0f; 
    }

    void Update()
    {
        if (_newSceneTriggered) return;
        if (_pastDrunkImpulses >= _numberOfDrinkingImpulses)
        {
            StartCoroutine(NextLevel());
            _newSceneTriggered = true;
            return;
        }
        
        _timeSinceLastQTE += Time.deltaTime;
        
        if (!_QTE.activeInHierarchy && _timeSinceLastQTE > qteInterval)
        {
            _QTE.SetActive(true);
            _nextQTE = Instantiate(qteObject, Vector3.zero, Quaternion.identity);
            _nextQTE.GetComponentInChildren<QuicktimeEventController>().sitSpot =  this;
            _QTE =  _nextQTE;
            _timeSinceLastQTE = 0f;
            _pastDrunkImpulses++;
        }
        
        if (playerInZone && Keyboard.current != null && interactKey != Key.None &&
            Keyboard.current[interactKey].wasPressedThisFrame)
        {
            if (!isSitting) StartCoroutine(SitRoutine());
            else StartCoroutine(StandRoutine());
        }

        if (!isSitting) return;

        // Block movement/jump/sprint but keep look flowing
        if (starterInputs)
        {
            starterInputs.move = Vector2.zero;
            starterInputs.jump = false;
            starterInputs.sprint = false;
        }

        // Rotate the seat anchor from look input and stop FPC from also rotating the body
        ApplySeatedLook();
        if (starterInputs) starterInputs.look = Vector2.zero;
    }

    public IEnumerator SitAndDrink()
    {
        if (!isSitting)
        {
            yield return Fade(1f);
        
            Sit();
        
            yield return Fade(0f);
        }
        
        drinkable.Drink(vcam.transform);
        
        PlayerPrefs.SetInt("Drunkness", (int)_currentDrunkness + 1);
        PlayerPrefs.Save();
        
        _currentDrunkness = (Drunkness) PlayerPrefs.GetInt("Drunkness");

        if (_currentDrunkness == Drunkness.Heavy)
        {
            StartCoroutine(NextLevel());
        }
    }

    private IEnumerator NextLevel()
    {
        _newSceneTriggered = true;
        yield return new WaitForSeconds(2f);
        yield return Fade(1f);
        sceneManager.GoToNextLevel();
    }

    IEnumerator SitRoutine()
    {
        yield return Fade(1f); // fade to black

        Sit();

        yield return Fade(0f); // fade back in
    }

    IEnumerator StandRoutine()
    {
        yield return Fade(1f); // fade to black

        Stand();

        yield return Fade(0f); // fade back in
    }

    void Sit()
    {
        if (!playerCapsule || !fpc || !starterInputs || !vcam || !seatAnchor) return;

        // Save player + camera state
        savedPlayerPos = playerCapsule.transform.position;
        savedPlayerRot = playerCapsule.transform.rotation;
        savedFollow = vcam.Follow;
        savedLookAt = vcam.LookAt;

        // Anchor camera to the seat
        vcam.Follow = seatAnchor;
        vcam.LookAt = seatAnchor;

        // Move player right to the seat
        playerCapsule.transform.SetPositionAndRotation(seatAnchor.position, seatAnchor.rotation);
        Physics.SyncTransforms();

        // Seed seated angles from current seat rotation
        var e = seatAnchor.rotation.eulerAngles;
        seatPitch = NormalizePitch(e.x);
        seatYaw   = e.y;

        SetPlayerVisible(false);
        isSitting = true;
        TogglePrompt(true, true);
    }

    void Stand()
    {
        if (!playerCapsule || !vcam) return;

        // Restore camera targets
        vcam.Follow = savedFollow ? savedFollow : playerFollowTarget;
        vcam.LookAt = savedLookAt ? savedLookAt : playerFollowTarget;

        // Restore player transform
        playerCapsule.transform.SetPositionAndRotation(savedPlayerPos, savedPlayerRot);
        Physics.SyncTransforms();

        SetPlayerVisible(true);
        isSitting = false;
        TogglePrompt(playerInZone, false);
    }

    void ApplySeatedLook()
    {
        if (!starterInputs || !seatAnchor || !fpc) return;

        var look = starterInputs.look;
        if (look.sqrMagnitude <= 0.0001f) return;

        bool isMouse = playerInput != null && playerInput.currentControlScheme == "KeyboardMouse";
        float dtMul = isMouse ? 1f : Time.deltaTime;

        seatPitch += look.y * fpc.RotationSpeed * dtMul;
        seatYaw   += look.x * fpc.RotationSpeed * dtMul;

        seatPitch = ClampAngle(seatPitch, fpc.BottomClamp, fpc.TopClamp);
        seatAnchor.rotation = Quaternion.Euler(seatPitch, seatYaw, 0f);
    }

    IEnumerator Fade(float targetAlpha)
    {
        if (!fadeCanvas) yield break;

        float startAlpha = fadeCanvas.alpha;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            fadeCanvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        fadeCanvas.alpha = targetAlpha;
    }

    // Helpers
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle >  360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
    static float NormalizePitch(float x)
    {
        if (x > 180f) x -= 360f;
        return x;
    }
    void SetPlayerVisible(bool visible)
    {
        if (capsuleVisual) { capsuleVisual.SetActive(visible); return; }
        foreach (var r in playerCapsule.GetComponentsInChildren<Renderer>())
            r.enabled = visible;
    }
    void TogglePrompt(bool show, bool showStandText)
    {
        if (promptRoot) promptRoot.SetActive(show);
        if (promptText) promptText.text = showStandText ? "Press E to stand up" : "Press E to sit down";
    }
    void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;
        playerInZone = true;
        if (!isSitting) TogglePrompt(true, false);
    }
    void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;
        playerInZone = false;
        TogglePrompt(false, false);
    }
    bool IsPlayer(Collider other)
    {
        return playerCapsule && other.transform.root.gameObject == playerCapsule.transform.root.gameObject;
    }
}
