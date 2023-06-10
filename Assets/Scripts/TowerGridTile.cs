using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGridTile
{
    private int xPos;
    private int yPos;
    private GameObject towerTileGameObject;
    private TowerSO towerSO;
    private int currentState;
    private int numberOfStates;

    public TowerGridTile(int xPos, int yPos, GameObject towerTileGameObject, TowerSO towerSO)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        this.towerTileGameObject = towerTileGameObject;
        this.towerSO = towerSO;
        currentState = 0;
        numberOfStates = towerSO.statesSprites.Count;
        towerTileGameObject.GetComponent<SpriteRenderer>().sprite = towerSO.statesSprites[currentState];
    }

    public GameObject GetTowerTileGameObject()
    {
        return towerTileGameObject;
    }

    public bool Upgrade()
    {
        if (currentState >= numberOfStates - 1)
        {
            return false;
        }

        towerTileGameObject.GetComponent<SpriteRenderer>().sprite = towerSO.statesSprites[++currentState];

        return true;
    }

    public int GetCurrentState()
    {
        return currentState;
    }

    public bool CanBeUpgraded()
    {
        return currentState < numberOfStates - 1;
    }

    public TowerTypeEnum GetTowerType()
    {
        return towerSO.towerType;
    }
}
