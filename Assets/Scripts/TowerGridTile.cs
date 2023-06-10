using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGridTile
{
    private int xPos;
    private int yPos;
    private GameObject towerTileGameObject;
    private bool isOccupied = false;
    private TowerSO towerSO;
    private int currentState;
    private int numberOfStates;

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

    public int Upgrade()
    {
        if (currentState >= numberOfStates)
        {
            return -1;
        }

        return ++currentState;
    }

    public int GetCurrentState()
    {
        return currentState;
    }

    public int GetNumberOfStates()
    {
        return numberOfStates;
    }

    public void SetTowerSO(TowerSO towerSO)
    {
        this.towerSO = towerSO;
        this.numberOfStates = towerSO.statesSprites.Count;
    }

    public TowerSO GetTowerSO()
    {
        return towerSO;
    }
}
