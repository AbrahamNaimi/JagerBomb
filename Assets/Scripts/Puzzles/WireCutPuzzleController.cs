using Puzzles;
using Puzzles.Logbook;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WireCutPuzzleController : MonoBehaviour, IPuzzle
{
    public bool IsPuzzleSolved { get; private set; }
    public LogbookController logbookController;

    public List<GameObject> wires; // Assign in Inspector (5 wires)
    public AudioClip cutSound; // assign in Inspector
    public float cutDuration = 0.2f;
    public GameObject cutFlashPrefab; // assign in Inspector

    private GameObject _correctWire;
    private bool _isLocked = false;

    void Start()
    {
        if (wires == null || wires.Count == 0)
        {
            wires = GetComponentsInChildren<Transform>()
                .Where(t => t != transform && t.name.ToLower().Contains("wire"))
                .Select(t => t.gameObject)
                .ToList();
        }

        if (wires.Count == 0)
        {
            Debug.LogError("No wires found for WireCutPuzzleController.");
            return;
        }

        // Pick random correct wire
        _correctWire = wires[Random.Range(0, wires.Count)];

        // Optional: shuffle colors
        Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.black };
        for (int i = 0; i < wires.Count && i < colors.Length; i++)
        {
            Renderer rend = wires[i].GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = colors[i];
        }

        GenerateLogbookPage();
    }

public void ObjectClicked(GameObject hitGameObject)
{
    if (_isLocked || IsPuzzleSolved) return;
    if (!wires.Contains(hitGameObject)) return;

    _isLocked = true;

    // Play cut sound
    if (cutSound != null)
        AudioSource.PlayClipAtPoint(cutSound, hitGameObject.transform.position);
        Debug.Log(cutFlashPrefab);
    // ✅ Spawn CutFlash prefab at wire's position with slight offset
    if (cutFlashPrefab != null)
    {
        Vector3 spawnPos = hitGameObject.transform.position + hitGameObject.transform.up * 0.1f;
        GameObject flashInstance = Instantiate(cutFlashPrefab, spawnPos, Quaternion.identity);
        // ✅ Optional: parent it to the wire so it moves with it
        flashInstance.transform.SetParent(hitGameObject.transform);
    }

    // Check result
    if (hitGameObject == _correctWire)
    {
        IsPuzzleSolved = true;
        StartCoroutine(FlashVictory());
        Debug.Log("Correct wire cut!");
    }
    else
    {
        StartCoroutine(FlashFail(hitGameObject));
        Debug.Log("Wrong wire cut!");
    }
}


    private IEnumerator FlashFail(GameObject wire, float duration = 0.5f)
    {
        Renderer rend = wire.GetComponent<Renderer>();
        if (rend != null)
        {
            Color originalColor = rend.material.color;
            rend.material.color = Color.red;
            yield return new WaitForSeconds(duration);
            rend.material.color = originalColor;
        }
        _isLocked = false; // allow retry if you want
    }

    private IEnumerator FlashVictory()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (var wire in wires)
        {
            Renderer rend = wire.GetComponent<Renderer>();
            if (rend != null)
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
