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
    }
}

