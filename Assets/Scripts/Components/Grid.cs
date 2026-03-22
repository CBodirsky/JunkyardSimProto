using UnityEngine;

public static class Grid
{
    public static float cellSize = 1f;

    public static Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / cellSize) * cellSize;
        float y = Mathf.Round(position.y / cellSize) * cellSize;
        float z = Mathf.Round(position.z / cellSize) * cellSize;

        return new Vector3(x, y, z);
    }
}
