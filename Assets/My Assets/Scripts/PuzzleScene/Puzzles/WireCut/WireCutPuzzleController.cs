using System.Collections;
using System.Collections.Generic;
using System.Linq;
using My_Assets.Puzzles.Logbook;
using My_Assets.PuzzleScene.Bomb;
using UnityEngine;

namespace My_Assets.PuzzleScene.Puzzles.WireCut
{
    public class WireCutPuzzleController : MonoBehaviour, IPuzzle
    {
        public bool IsPuzzleSolved { get; private set; }
        public LogbookController logbookController;
        [SerializeField]
        private TimerController timerController;

        public List<GameObject> wires;
        public AudioClip cutSound;
        public float cutDuration = 0.2f;
        public GameObject cutFlashPrefab;

        private bool _isLocked;
        private int _correctWiresRemaining = 2;

        private List<WireData> _wireDataList = new();
        private WireCutPuzzleGenerator _generator;

        void Start()
        {
            _generator = new WireCutPuzzleGenerator();
            SetupWires();
            GenerateLogbookPage();
        }

        private void SetupWires()
        {
            var colors = _generator.GenerateWireColors(wires.Count);
            var outcomeMap = _generator.GenerateOutcomeMap(wires.Count);

            _correctWiresRemaining = 2;
            _wireDataList.Clear();

            for (int i = 0; i < wires.Count; i++)
            {
                GameObject wire = wires[i];
                Color color = colors[i];

                wire.GetComponent<Renderer>().material.color = color;

                _wireDataList.Add(new WireData
                {
                    wireObject = wire,
                    color = color,
                    outcome = outcomeMap[i],
                    isCut = false
                });
            }
        }

        public void ObjectClicked(GameObject hitGameObject)
        {
            if (_isLocked || IsPuzzleSolved) return;

            WireData wire = _wireDataList.FirstOrDefault(w => w.wireObject == hitGameObject);
            if (wire == null || wire.isCut) return;

            _isLocked = true;

            if (cutSound != null)
                AudioSource.PlayClipAtPoint(cutSound, hitGameObject.transform.position);

            SpawnCutFlash(hitGameObject);

            switch (wire.outcome)
            {
                case WireOutcome.Correct:
                    wire.isCut = true;
                    _correctWiresRemaining--;

                    if (_correctWiresRemaining <= 0)
                    {
                        IsPuzzleSolved = true;
                        StartCoroutine(FlashVictory());
                    }
                    else
                    {
                        _isLocked = false;
                    }
                    break;

                case WireOutcome.WrongTimePenalty:
                    timerController.DeductTime(10f);
                    StartCoroutine(FlashFail(hitGameObject));
                    break;
            }

        }

        private void GenerateLogbookPage()
        {
            var correctColors = _wireDataList
                .Where(w => w.outcome == WireOutcome.Correct)
                .Select(w => w.color)
                .ToList();

            string instructions = _generator.GenerateLogbookPage(correctColors);

            logbookController.AddPage(
                new LogBookPage("Wire Cut Puzzle", "Wire Cut", instructions)
            );
        }

        private void SpawnCutFlash(GameObject wire)
        {
            if (!cutFlashPrefab)
            {
                Debug.LogError("CutFlashPrefab NOT assigned!");
                return;
            }

            var flash = Instantiate(cutFlashPrefab, wire.transform);
            flash.transform.localPosition = new Vector3(0f, -0.1f, -0.01f);
        }



        private IEnumerator FlashFail(GameObject wire, float duration = 0.5f)
        {
            Renderer rend = wire.GetComponent<Renderer>();
            Color original = rend.material.color;

            rend.material.color = Color.red;
            yield return new WaitForSeconds(duration);
            rend.material.color = original;

            _isLocked = false;
        }

        private IEnumerator FlashVictory()
        {
            yield return new WaitForSeconds(0.2f);
            foreach (var wire in wires)
                wire.GetComponent<Renderer>().material.color = Color.green;
        }
    }
}