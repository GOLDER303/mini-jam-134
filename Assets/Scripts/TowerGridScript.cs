using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGridScript : MonoBehaviour
{
    [SerializeField] private GameObject emptyTowerGridTilePrefab;

    private int gridWidth = 5;
    private int gridHeight = 4;
    private float cellSize = 3f;
    private TowerGridTile[,] gridArray;

    public TowerGridScript()
    {
        this.gridArray = new TowerGridTile[gridWidth, gridHeight];
    }

    void Start()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                GameObject towerTileGameObject = Instantiate(emptyTowerGridTilePrefab, new Vector3(x * cellSize + cellSize / 2, y * cellSize + cellSize / 2), Quaternion.identity);
                gridArray[x, y] = new TowerGridTile(x, y, towerTileGameObject);
            }
        }

        // Debug
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, gridHeight), GetWorldPosition(gridWidth, gridHeight), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(gridWidth, 0), GetWorldPosition(gridWidth, gridHeight), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }
}
