using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGridScript : MonoBehaviour
{
    [SerializeField] private GameObject emptyGridTilePrefab;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private List<TowerSO> towerSOs;
    private Vector3 gridOriginPosition = new Vector3(-7.5f, -6f, 0);

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
                Instantiate(emptyGridTilePrefab, GetWorldPosition(x, y) + new Vector3(cellSize / 2, cellSize / 2, .1f), Quaternion.identity);
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

        while (gridArray[towerXPos, towerYPos] != null)
        {
            towerXPos = Random.Range(0, gridArray.GetLength(0) - 1);
            towerYPos = Random.Range(0, gridArray.GetLength(1) - 1);
        }

        TowerSO newTowerSO = towerSOs[Random.Range(0, towerSOs.Count - 1)];

        GameObject towerGameObject = Instantiate(towerPrefab, GetWorldPosition(towerXPos, towerYPos) + new Vector3(cellSize / 2, cellSize / 2), Quaternion.identity);

        gridArray[towerXPos, towerYPos] = new TowerGridTile(towerXPos, towerYPos, towerGameObject, newTowerSO);
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

    private bool UpgradeTower(Vector2Int towerPosition)
    {
        TowerGridTile towerToUpgrade = gridArray[towerPosition.x, towerPosition.y];
        return towerToUpgrade.Upgrade();
    }

    private void MoveTower(Vector2Int startDrag, Vector2Int endDrag)
    {
        if (!IsValidTilePosition(startDrag) || !IsValidTilePosition(endDrag))
            return;

        TowerGridTile movedTile = gridArray[startDrag.x, startDrag.y];
        TowerGridTile destinationTile = gridArray[endDrag.x, endDrag.y];

        if (movedTile == null && destinationTile == null)
            return;


        if (movedTile != null
            && destinationTile != null
            && movedTile.GetTowerType() == destinationTile.GetTowerType()
            && movedTile.GetCurrentState() == destinationTile.GetCurrentState()
            && destinationTile.CanBeUpgraded())
        {
            Destroy(movedTile.GetTowerTileGameObject());
            gridArray[startDrag.x, startDrag.y] = null;
            destinationTile.Upgrade();
        }
        else
        {
            if (destinationTile == null)
            {
                Vector3 movedTilePosition = movedTile.GetTowerTileGameObject().transform.position;
                movedTile.GetTowerTileGameObject().transform.position = GetWorldPosition(endDrag) + new Vector3(cellSize / 2, cellSize / 2);

                gridArray[startDrag.x, startDrag.y] = null;
                gridArray[endDrag.x, endDrag.y] = movedTile;
            }
        }
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

    private Vector3 GetWordPositionOfCellCenter(Vector2Int position)
    {
        return (new Vector3(position.x, position.y) * cellSize + gridOriginPosition) + new Vector3(cellSize / 2, cellSize / 2);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + gridOriginPosition;
    }

    private Vector3 GetWorldPosition(Vector2Int position)
    {
        return new Vector3(position.x, position.y) * cellSize + gridOriginPosition;
    }

    private Vector2Int GetTileFromWorldPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - gridOriginPosition).x / cellSize);
        int y = Mathf.FloorToInt((worldPosition - gridOriginPosition).y / cellSize);

        return new Vector2Int(x, y);
    }
}
