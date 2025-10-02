using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
// for Mouse/Gamepad

namespace My_Assets.Bar_Mechanics
{
    public class LookInteractor : MonoBehaviour
    {
        [Header("Setup")]
        public SitSpot sitSpot;                   // drag your SitSpot here (must have seatAnchor)
        public LayerMask interactMask = ~0;       // optionally set to an "Interactable" layer
        public float maxDistance = 2.0f;          // reach distance while seated

        [Header("Prompt UI")]
        public GameObject promptRoot;             // small canvas / GO
        public TextMeshProUGUI promptText;        // TMP text on that canvas
        public string drinkPrompt = "Click to Drink";

        Camera cam;

        void Awake()
        {
            cam = GetComponent<Camera>();
            if (!cam) cam = Camera.main;
            if (promptRoot) promptRoot.SetActive(false);
        }

        void Update()
        {
            // Only interact when seated and seatAnchor exists
            if (!sitSpot || !sitSpot.IsSitting || !sitSpot.seatAnchor)
            {
                HidePrompt();
                return;
            }
            if (!cam) { HidePrompt(); return; }

            // Center-screen ray
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out var hit, maxDistance, interactMask, QueryTriggerInteraction.Ignore))
            {
                var drinkable = hit.collider.GetComponentInParent<Drinkable>();
                if (drinkable != null && !drinkable.IsBusy)
                {
                    ShowPrompt(drinkPrompt);

                    bool clicked =
                        (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
                        (Gamepad.current != null && (Gamepad.current.buttonSouth.wasPressedThisFrame ||
                                                     Gamepad.current.rightTrigger.wasPressedThisFrame));

                    if (clicked)
                    {
                        HidePrompt();
                        // Pass Main Camera (for local animation space) and the SeatAnchor (hold target)
                        drinkable.Drink(cam.transform);
                    }
                    return;
                }
            }

            HidePrompt();
        }

        void ShowPrompt(string text)
        {
            if (promptText) promptText.text = text;
            if (promptRoot && !promptRoot.activeSelf) promptRoot.SetActive(true);
        }

        void HidePrompt()
        {
            if (promptRoot && promptRoot.activeSelf) promptRoot.SetActive(false);
        }
    }
}
