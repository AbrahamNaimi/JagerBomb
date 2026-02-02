using UnityEngine;

namespace My_Assets.PuzzleScene.Puzzles.WireCut
{
    [System.Serializable]
    public class WireData
    {
        public GameObject wireObject;
        public Color color;
        public WireOutcome outcome;
        public bool isCut;
    }
}
