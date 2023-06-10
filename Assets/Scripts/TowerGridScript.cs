using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGridScript : MonoBehaviour
{
    [SerializeField] private GameObject emptyTowerGridTilePrefab;
    [SerializeField] private List<TowerSO> towerSOs;

    private int gridWidth = 5;
    private int gridHeight = 4;
    private float cellSize = 3f;
    private TowerGridTile[,] gridArray;

    private Vector2Int startDrag = new Vector2Int(-1, -1);
    private Vector2Int endDrag = new Vector2Int(-1, -1);

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

        SpawnRandomTower();
    }

    private void SpawnRandomTower()
    {
        int towerXPos = Random.Range(0, gridArray.GetLength(0) - 1);
        int towerYPos = Random.Range(0, gridArray.GetLength(1) - 1);

        while (gridArray[towerXPos, towerYPos].IsOccupied())
        {
            towerXPos = Random.Range(0, gridArray.GetLength(0) - 1);
            towerYPos = Random.Range(0, gridArray.GetLength(1) - 1);
        }

        TowerSO newTowerSO = towerSOs[Random.Range(0, towerSOs.Count - 1)];

        gridArray[towerXPos, towerYPos].GetTowerTileGameObject().GetComponent<SpriteRenderer>().sprite = newTowerSO.statesSprites[newTowerSO.currentState];

        gridArray[towerXPos, towerYPos].SetIsOccupied(true);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            startDrag = GetTileFromWorldPosition(mouseWorldPosition);
            Debug.Log("Start drag: " + startDrag.ToString());
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            endDrag = GetTileFromWorldPosition(mouseWorldPosition);
            Debug.Log("End drag: " + endDrag.ToString());

            if (startDrag.x == endDrag.x && startDrag.y == endDrag.y)
            {
                return;
            }

            MoveTower(startDrag, endDrag);
        }
    }

    private void MoveTower(Vector2Int startDrag, Vector2Int endDrag)
    {
        if (!IsValidTilePosition(startDrag) || !IsValidTilePosition(endDrag))
        {
            return;
        }

        TowerGridTile movedTile = gridArray[startDrag.x, startDrag.y];
        TowerGridTile destinationTile = gridArray[endDrag.x, endDrag.y];

        Sprite destinationTileSprite = destinationTile.GetTowerTileGameObject().GetComponent<SpriteRenderer>().sprite;
        destinationTile.GetTowerTileGameObject().GetComponent<SpriteRenderer>().sprite = movedTile.GetTowerTileGameObject().GetComponent<SpriteRenderer>().sprite;
        movedTile.GetTowerTileGameObject().GetComponent<SpriteRenderer>().sprite = destinationTileSprite;
    }

    private bool IsValidTilePosition(Vector2Int position)
    {
        if (position.x < 0 || position.y < 0 || position.x >= gridWidth || position.y >= gridHeight)
        {
            return false;
        }
        return true;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        return mouseWorldPosition;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    private Vector2Int GetTileFromWorldPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition).x / cellSize);
        int y = Mathf.FloorToInt((worldPosition).y / cellSize);

        return new Vector2Int(x, y);
    }
}
