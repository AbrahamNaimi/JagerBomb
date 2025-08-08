using UnityEngine;

public class MazeCell
{
    public readonly int x;
    public readonly int y;
    public GameObject visualCube;

    public bool wallTop = true;
    public bool wallBottom = true;
    public bool wallLeft = true;
    public bool wallRight = true;

    public bool visited = false;

    public MazeCell(int x, int y, GameObject cube)
    {
        this.x = x;
        this.y = y;
        visualCube = cube;
    }

    public void RemoveWall(MazeCell neighbor)
    {
        int dx = neighbor.x - x;
        int dy = neighbor.y - y;

        if (dx == 1) { wallRight = false; neighbor.wallLeft = false; }
        else if (dx == -1) { wallLeft = false; neighbor.wallRight = false; }
        else if (dy == 1) { wallTop = false; neighbor.wallBottom = false; }
        else if (dy == -1) { wallBottom = false; neighbor.wallTop = false; }
    }
}
