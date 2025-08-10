using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using StarterAssets;
using Cinemachine;

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

    [Header("UI Prompt")]
    public GameObject promptRoot;
    public TextMeshProUGUI promptText;

    [Header("Input")]
    public Key interactKey = Key.E;

    // State
    bool playerInZone, isSitting;
    Vector3 savedPlayerPos;
    Quaternion savedPlayerRot;
    Transform savedFollow, savedLookAt;

    PlayerInput playerInput;                        // detect mouse vs gamepad
    float seatPitch, seatYaw;                       // seated look angles

    public bool IsSitting => isSitting;


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
    }

    void Update()
    {
        if (playerInZone && Keyboard.current != null && interactKey != Key.None &&
            Keyboard.current[interactKey].wasPressedThisFrame)
        {
            if (!isSitting) Sit();
            else Stand();
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

        // Move player right to the seat (so collisions/ears match)
        playerCapsule.transform.SetPositionAndRotation(seatAnchor.position, seatAnchor.rotation);
        Physics.SyncTransforms();

        // Seed seated angles from current seat rotation
        var e = seatAnchor.rotation.eulerAngles;
        seatPitch = NormalizePitch(e.x);
        seatYaw   = e.y;

        SetPlayerVisible(false);
        isSitting = true;
        TogglePrompt(true, true); // "Press E to stand up"
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
        TogglePrompt(playerInZone, false); // "Press E to sit down" if still in trigger
    }

    void ApplySeatedLook()
    {
        if (!starterInputs || !seatAnchor || !fpc) return;

        var look = starterInputs.look;
        if (look.sqrMagnitude <= 0.0001f) return;

        // Match Starter Assets feel: mouse (no dt), gamepad (scaled by dt)
        bool isMouse = playerInput != null && playerInput.currentControlScheme == "KeyboardMouse";
        float dtMul = isMouse ? 1f : Time.deltaTime;

        seatPitch += look.y * fpc.RotationSpeed * dtMul;
        seatYaw   += look.x * fpc.RotationSpeed * dtMul;

        seatPitch = ClampAngle(seatPitch, fpc.BottomClamp, fpc.TopClamp);
        seatAnchor.rotation = Quaternion.Euler(seatPitch, seatYaw, 0f);
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
