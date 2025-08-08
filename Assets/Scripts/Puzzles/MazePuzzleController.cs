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

    private MazeCell[,] _grid;
    private Vector2Int _playerPos = new(0, 0);
    private Vector2Int _goalPos = new(GRID_WIDTH - 1, GRID_HEIGHT - 1);

    void Start()
    {
        InitializeGrid();
        HighlightPlayer();
        GenerateLogbookPage();
    }

    public void ObjectClicked(GameObject hitGameObject)
    {
        string objectName = hitGameObject.name;

        switch (objectName)
        {
            case "ArrowUp":
                Move(Vector2Int.down); // Visually up
                break;
            case "ArrowDown":
                Move(Vector2Int.up); // Visually down
                break;
            case "ArrowLeft":
                Move(Vector2Int.left);
                break;
            case "ArrowRight":
                Move(Vector2Int.right);
                break;
        }
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

        cubeTransforms = cubeTransforms
            .OrderBy(t => int.Parse(t.name.Split(" ")![1]))
            .ToList();

        _grid = new MazeCell[GRID_WIDTH, GRID_HEIGHT];

        for (int i = 0; i < cubeTransforms.Count; i++)
        {
            int x = i % GRID_WIDTH;
            int y = i / GRID_WIDTH;

            _grid[x, y] = new MazeCell(x, y, cubeTransforms[i].gameObject);
        }
    }


    private void HighlightPlayer()
    {
        for (int y = 0; y < GRID_HEIGHT; y++)
        {
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                var cell = _grid[x, y];
                if (cell == null || cell.visualCube == null) continue;

                var rend = cell.visualCube.GetComponent<Renderer>();
                if (rend == null) continue;

                if (_playerPos == new Vector2Int(x, y)) rend.material.color = Color.red;
                else if (_goalPos == new Vector2Int(x, y)) rend.material.color = Color.green;
                else rend.material.color = Color.white;
            }
        }
    }

    public void Move(Vector2Int dir)
    {
        var newPos = _playerPos + dir;
        if (newPos.x < 0 || newPos.x >= GRID_WIDTH || newPos.y < 0 || newPos.y >= GRID_HEIGHT)
            return;

        _playerPos = newPos;
        HighlightPlayer();

        if (_playerPos == _goalPos)
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