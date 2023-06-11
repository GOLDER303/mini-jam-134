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
    private GameObject[,] gridArray;

    private Vector2Int startDrag = new Vector2Int(-1, -1);
    private Vector2Int endDrag = new Vector2Int(-1, -1);

    public TowerGridScript()
    {
        this.gridArray = new GameObject[gridWidth, gridHeight];
    }

    void Start()
    {
        GameObject gridTilesEmpty = new GameObject("GridTiles");

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Instantiate(emptyGridTilePrefab, GetWorldPosition(x, y) + new Vector3(cellSize / 2, cellSize / 2, .1f), Quaternion.identity, gridTilesEmpty.transform);
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

        towerGameObject.GetComponent<TowerManager>().SetData(newTowerSO);

        gridArray[towerXPos, towerYPos] = towerGameObject;
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
        GameObject towerToUpgrade = gridArray[towerPosition.x, towerPosition.y];
        return towerToUpgrade.GetComponent<TowerManager>().Upgrade();
    }

    private void MoveTower(Vector2Int startDrag, Vector2Int endDrag)
    {
        if (!IsValidTilePosition(startDrag) || !IsValidTilePosition(endDrag))
            return;

        GameObject movedTower = gridArray[startDrag.x, startDrag.y];
        GameObject destinationTile = gridArray[endDrag.x, endDrag.y];

        if (movedTower == null)
            return;


        if (destinationTile != null)
        {
            TowerManager movedTowerManagementComponent = movedTower.GetComponent<TowerManager>();
            TowerManager destinationTowerManagementComponent = destinationTile.GetComponent<TowerManager>();

            if (movedTowerManagementComponent.GetTowerType() == destinationTowerManagementComponent.GetTowerType()
                && movedTowerManagementComponent.GetCurrentState() == destinationTowerManagementComponent.GetCurrentState()
                && destinationTowerManagementComponent.CanBeUpgraded())
            {
                Destroy(movedTower);
                gridArray[startDrag.x, startDrag.y] = null;
                destinationTowerManagementComponent.Upgrade();
            }
            else
            {
                ChangeTowerPosition(startDrag, endDrag);
            }
        }
        else
        {
            ChangeTowerPosition(startDrag, endDrag);
        }
    }

    private void ChangeTowerPosition(Vector2Int firstTowerPosition, Vector2Int secondTowerPosition)
    {
        GameObject firstTower = gridArray[firstTowerPosition.x, firstTowerPosition.y];
        GameObject secondTower = gridArray[secondTowerPosition.x, secondTowerPosition.y];

        firstTower.transform.position = GetWordPositionOfCellCenter(secondTowerPosition);
        gridArray[secondTowerPosition.x, secondTowerPosition.y] = firstTower;

        if (secondTower != null)
        {
            secondTower.transform.position = GetWordPositionOfCellCenter(firstTowerPosition);
            gridArray[firstTowerPosition.x, firstTowerPosition.y] = secondTower;
        }
        else
        {
            gridArray[firstTowerPosition.x, firstTowerPosition.y] = null;
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
