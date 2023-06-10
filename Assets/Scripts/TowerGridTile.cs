using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGridTile
{
    private int xPos;
    private int yPos;
    private GameObject towerTileGameObject;
    private bool isOccupied = false;

    public TowerGridTile(int xPos, int yPos, GameObject towerTileGameObject)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        this.towerTileGameObject = towerTileGameObject;
    }

    public GameObject GetTowerTileGameObject()
    {
        return towerTileGameObject;
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void SetIsOccupied(bool isOccupied)
    {
        this.isOccupied = isOccupied;
    }
}
