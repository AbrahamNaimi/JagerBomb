using System.Collections;
using System.Collections.Generic;
using System.Linq;
using My_Assets.Puzzles.Logbook;
using Puzzles;
using UnityEngine;

namespace My_Assets.Puzzles.Controllers
{
    public class MazePuzzleController : MonoBehaviour, IPuzzle
    {
        public bool IsPuzzleSolved { get; private set; }
        public LogbookController logbookController;

        private const int GRID_WIDTH = 6;
        private const int GRID_HEIGHT = 5;

        private MazeCell[,] _grid;
        private Vector2Int _playerPos = new(0, 0);
        private Vector2Int _goalPos = new(GRID_WIDTH - 1, GRID_HEIGHT - 1);
        private List<Vector2Int> _solutionPath = new();
        private bool _isLocked = false;

        void Start()
        {
            InitializeGrid();
            GenerateMaze(_playerPos.x, _playerPos.y);
            HighlightPlayer();
            GenerateLogbookPage();
        }

        public void ObjectClicked(GameObject hitGameObject)
        {
            if (_isLocked || IsPuzzleSolved) return;

            string objectName = hitGameObject.name;

            switch (objectName)
            {
                case "ArrowUp":
                    AttemptMove(Vector2Int.down);
                    break;
                case "ArrowDown":
                    AttemptMove(Vector2Int.up);
                    break;
                case "ArrowLeft":
                    AttemptMove(Vector2Int.left);
                    break;
                case "ArrowRight":
                    AttemptMove(Vector2Int.right);
                    break;
            }
        }

        private void InitializeGrid()
        {
            var cubeTransforms = GetComponentsInChildren<Transform>()
                .Where(t => t != transform && t.name.ToLower().Contains("cube"))
                .OrderBy(t => int.Parse(t.name.Split(" ")[1]))
                .ToList();

            if (cubeTransforms.Count != GRID_WIDTH * GRID_HEIGHT)
            {
                Debug.LogError($"Expected {GRID_WIDTH * GRID_HEIGHT} cubes, found {cubeTransforms.Count}");
                return;
            }

            _grid = new MazeCell[GRID_WIDTH, GRID_HEIGHT];

            for (int i = 0; i < cubeTransforms.Count; i++)
            {
                int x = i % GRID_WIDTH;
                int y = i / GRID_WIDTH;
                _grid[x, y] = new MazeCell(x, y, cubeTransforms[i].gameObject);
            }
        }

        private void GenerateMaze(int startX, int startY)
        {
            Stack<MazeCell> stack = new();
            MazeCell start = _grid[startX, startY];
            start.Visited = true;
            stack.Push(start);

            Dictionary<MazeCell, MazeCell> cameFrom = new();

            while (stack.Count > 0)
            {
                MazeCell current = stack.Peek();
                var neighbors = GetUnvisitedNeighbors(current);

                if (neighbors.Count > 0)
                {
                    MazeCell next = neighbors[Random.Range(0, neighbors.Count)];
                    current.RemoveWall(next);
                    next.Visited = true;
                    cameFrom[next] = current;
                    stack.Push(next);
                }
                else
                {
                    stack.Pop();
                }
            }

            _solutionPath.Clear();
            MazeCell goal = _grid[_goalPos.x, _goalPos.y];
            while (goal != start)
            {
                _solutionPath.Add(new Vector2Int(goal.X, goal.Y));
                goal = cameFrom[goal];
            }
            _solutionPath.Add(new Vector2Int(start.X, start.Y));
            _solutionPath.Reverse();

            foreach (var cell in _grid)
                cell.Visited = false;
        }

        private List<MazeCell> GetUnvisitedNeighbors(MazeCell cell)
        {
            List<MazeCell> neighbors = new();
            int x = cell.X;
            int y = cell.Y;

            if (x > 0 && !_grid[x - 1, y].Visited) neighbors.Add(_grid[x - 1, y]);
            if (x < GRID_WIDTH - 1 && !_grid[x + 1, y].Visited) neighbors.Add(_grid[x + 1, y]);
            if (y > 0 && !_grid[x, y - 1].Visited) neighbors.Add(_grid[x, y - 1]);
            if (y < GRID_HEIGHT - 1 && !_grid[x, y + 1].Visited) neighbors.Add(_grid[x, y + 1]);

            return neighbors;
        }

        private void AttemptMove(Vector2Int dir)
        {
            MazeCell current = _grid[_playerPos.x, _playerPos.y];
            if (HasWall(current, dir))
            {
                _playerPos = new Vector2Int(0, 0);
                HighlightPlayer();
                StartCoroutine(FlashAllCubesRed());
                return;
            }

            Vector2Int newPos = _playerPos + dir;
            if (newPos.x < 0 || newPos.x >= GRID_WIDTH || newPos.y < 0 || newPos.y >= GRID_HEIGHT)
                return;

            _playerPos = newPos;
            HighlightPlayer();

            if (_playerPos == _goalPos)
            {
                IsPuzzleSolved = true;
                _isLocked = true;
                StartCoroutine(FlashVictory());
            }
        }

        private bool HasWall(MazeCell cell, Vector2Int dir)
        {
            if (dir == Vector2Int.up) return cell.WallTop;
            if (dir == Vector2Int.down) return cell.WallBottom;
            if (dir == Vector2Int.left) return cell.WallLeft;
            if (dir == Vector2Int.right) return cell.WallRight;
            return true;
        }

        private IEnumerator FlashAllCubesRed(float duration = 0.3f)
        {
            List<Renderer> renderers = new();

            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    var cell = _grid[x, y];
                    if (cell?.VisualCube == null) continue;

                    var rend = cell.VisualCube.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        renderers.Add(rend);
                        rend.material.color = Color.red;
                    }
                }
            }

            yield return new WaitForSeconds(duration);
            HighlightPlayer();
        }

        private IEnumerator FlashVictory()
        {
            yield return new WaitForSeconds(0.2f);

            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    var cell = _grid[x, y];
                    if (cell?.VisualCube == null) continue;

                    var rend = cell.VisualCube.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        rend.material.color = Color.green;
                    }
                }
            }
        }

        private void HighlightPlayer()
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    var cell = _grid[x, y];
                    if (cell == null || cell.VisualCube == null) continue;

                    var rend = cell.VisualCube.GetComponent<Renderer>();
                    if (rend == null) continue;

                    if (_playerPos == new Vector2Int(x, y)) rend.material.color = Color.red;
                    else if (_goalPos == new Vector2Int(x, y)) rend.material.color = Color.green;
                    else rend.material.color = Color.white;
                }
            }
        }

        private void GenerateLogbookPage()
        {
            string instructions = "Ohh how challenging! A maze!\n\n" +
                                  "Use the buttons but do not mess up! That would end in an early end of your shift! Haha!\n" +
                                  "Guide yourself to the end of the made using the instructions below!\n" +
                                  GetFormattedSolution();

            logbookController.AddPage(
                new LogBookPage("Maze puzzle", "Maze", instructions)
            );
        }

        private string GetFormattedSolution()
        {
            List<string> directions = new();

            for (int i = 1; i < _solutionPath.Count; i++)
            {
                Vector2Int from = _solutionPath[i - 1];
                Vector2Int to = _solutionPath[i];
                Vector2Int dir = to - from;

                if (dir == Vector2Int.up) directions.Add("↓");
                else if (dir == Vector2Int.down) directions.Add("↑");
                else if (dir == Vector2Int.left) directions.Add("←");
                else if (dir == Vector2Int.right) directions.Add("→");
            }

            return string.Join(" ", directions);
        }
    }
}
