using System.Collections;
using System.Collections.Generic;
using My_Assets.Puzzles.Logbook;
using Puzzles;
using UnityEngine;

namespace My_Assets.Puzzles.Controllers
{
    public class WireCutPuzzleController : MonoBehaviour, IPuzzle
    {
        public bool IsPuzzleSolved { get; private set; }
        public LogbookController logbookController;

        public List<GameObject> wires;
        public AudioClip cutSound;
        public float cutDuration = 0.2f;
        public GameObject cutFlashPrefab;

        private List<Color> _wireColors = new() { Color.white, Color.blue, Color.green, Color.yellow, Color.black };
        private GameObject _correctWire;
        private bool _isLocked;

        void Start()
        {
            _correctWire = wires[Random.Range(0, wires.Count)];

            foreach (GameObject wire in wires)
            {
                Renderer wireRenderer = wire.GetComponent<Renderer>();
                Color wireColor = _wireColors[Random.Range(0, _wireColors.Count)];
                wireRenderer.material.color = wireColor;
                _wireColors.Remove(wireColor);
            }

            GenerateLogbookPage();
        }

        public void ObjectClicked(GameObject hitGameObject)
        {
            if (_isLocked || IsPuzzleSolved) return;
            if (!wires.Contains(hitGameObject)) return;

            _isLocked = true;

            if (cutSound != null)
                AudioSource.PlayClipAtPoint(cutSound, hitGameObject.transform.position);

            if (cutFlashPrefab != null)
            {
                Vector3 spawnPos = hitGameObject.transform.position;
                spawnPos.z -= 0.2f;
                spawnPos.y -= 0.1f;

                GameObject flashInstance = Instantiate(cutFlashPrefab, spawnPos, Quaternion.identity);

                flashInstance.transform.SetParent(hitGameObject.transform);
                CutFlashEffect effect = flashInstance.GetComponent<CutFlashEffect>();
                if (effect != null)
                {
                    effect.shouldFade = hitGameObject != _correctWire;
                }
            }

            if (hitGameObject == _correctWire)
            {
                IsPuzzleSolved = true;
                StartCoroutine(FlashVictory());
            }
            else
            {
                StartCoroutine(FlashFail(hitGameObject));
            }
        }


        private IEnumerator FlashFail(GameObject wire, float duration = 0.5f)
        {
            Renderer rend = wire.GetComponent<Renderer>();

            Color originalColor = rend.material.color;
            rend.material.color = Color.red;
            yield return new WaitForSeconds(duration);
            rend.material.color = originalColor;

            _isLocked = false;
        }

        private IEnumerator FlashVictory()
        {
            yield return new WaitForSeconds(0.2f);
            foreach (var wire in wires)
            {
                Renderer rend = wire.GetComponent<Renderer>();
                rend.material.color = Color.green;
            }
        }

        private void GenerateLogbookPage()
        {
            string instructions = "Knip de juiste draad door om het systeem te ontmantelen.\n" +
                                  "Hint: Slechts één draad is correct.";

            logbookController.AddPage(
                new LogBookPage("Wire Cut Puzzle", "Bomb Defusal", instructions)
            );
        }
    }
}