using UnityEngine;

namespace My_Assets.PuzzleScene.Puzzles.Maze
{
    public class MazeCell
    {
        public readonly int X;
        public readonly int Y;
        public GameObject VisualCube;

        public bool WallTop = true;
        public bool WallBottom = true;
        public bool WallLeft = true;
        public bool WallRight = true;

        public bool Visited = false;

        public MazeCell(int x, int y, GameObject cube)
        {
            X = x;
            Y = y;
            VisualCube = cube;
        }

        public void RemoveWall(MazeCell neighbor)
        {
            int dx = neighbor.X - X;
            int dy = neighbor.Y - Y;

            switch (dx)
            {
                case 1:
                    WallRight = false; neighbor.WallLeft = false;
                    break;
                case -1:
                    WallLeft = false; neighbor.WallRight = false;
                    break;
                default:
                {
                    if (dy == 1) { WallTop = false; neighbor.WallBottom = false; }
                    else if (dy == -1) { WallBottom = false; neighbor.WallTop = false; }

                    break;
                }
            }
            if (dx == 1) { WallRight = false; neighbor.WallLeft = false; }
            else if (dx == -1) { WallLeft = false; neighbor.WallRight = false; }
            else if (dy == 1) { WallTop = false; neighbor.WallBottom = false; }
            else if (dy == -1) { WallBottom = false; neighbor.WallTop = false; }
        }
    }
}
