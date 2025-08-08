using Puzzles.Logbook;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Puzzles;

public class MazePuzzleController : MonoBehaviour, IPuzzle
{
    public bool IsPuzzleSolved { get; private set; }
    public LogbookController logbookController;

    private const int GRID_WIDTH = 6;
    private const int GRID_HEIGHT = 5;

    private MazeCell[,] grid;
    private Vector2Int playerPos = new Vector2Int(0, 0);
    private Vector2Int goalPos = new Vector2Int(GRID_WIDTH - 1, GRID_HEIGHT - 1);

    void Start()
    {
        InitializeGrid();
        HighlightPlayer();
        GenerateLogbookPage();
    }

    public void ObjectClicked(GameObject hitGameObject)
    {
        string name = hitGameObject.name.ToLower();
        if (name.Contains("up")) Move(Vector2Int.down);   // visually up
        if (name.Contains("down")) Move(Vector2Int.up);   // visually down
        if (name.Contains("left")) Move(Vector2Int.left);
        if (name.Contains("right")) Move(Vector2Int.right);
    }

    private void InitializeGrid()
    {
        var cubeTransforms = GetComponentsInChildren<Transform>()
            .Where(t => t != transform && t.name.ToLower().Contains("cube"))
            .ToList();

        if (cubeTransforms.Count != GRID_WIDTH * GRID_HEIGHT)
        {
            Debug.LogError($"Expected {GRID_WIDTH * GRID_HEIGHT} cubes, found {cubeTransforms.Count}");
            return;
        }

        // Sort by Z (rows), then X (columns)
        cubeTransforms = cubeTransforms
            .OrderByDescending(t => t.localPosition.z) // top to bottom
            .ThenBy(t => t.localPosition.x)            // left to right
            .ToList();

        grid = new MazeCell[GRID_WIDTH, GRID_HEIGHT];

        for (int i = 0; i < cubeTransforms.Count; i++)
        {
            int x = i % GRID_WIDTH;
            int y = i / GRID_WIDTH;

            grid[x, y] = new MazeCell(x, y, cubeTransforms[i].gameObject);
        }
    }



    private void HighlightPlayer()
    {
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                var cell = grid[x, y];
                if (cell == null || cell.visualCube == null) continue;

                var rend = cell.visualCube.GetComponent<Renderer>();
                if (rend == null) continue;

                if (playerPos == new Vector2Int(x, y)) rend.material.color = Color.red;
                else if (goalPos == new Vector2Int(x, y)) rend.material.color = Color.green;
                else rend.material.color = Color.white;
            }
        }
    }

    public void Move(Vector2Int dir)
    {
        var newPos = playerPos + dir;
        if (newPos.x < 0 || newPos.x >= GRID_WIDTH || newPos.y < 0 || newPos.y >= GRID_HEIGHT)
            return;

        // Optional: add wall check here using grid[playerPos.x, playerPos.y].HasWall(dir)
        playerPos = newPos;
        HighlightPlayer();

        if (playerPos == goalPos)
        {
            IsPuzzleSolved = true;
            Debug.Log("Maze opgelost!");
        }
    }

    private void GenerateLogbookPage()
    {
        logbookController.AddPage(
            new LogBookPage("Maze puzzle", "Maze", "Vind de weg naar het doel (groen).")
        );
    }
}
