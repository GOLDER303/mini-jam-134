using System;
using System.Collections;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private TowerSO towerSO;

    private int currentState;
    private int numberOfStates;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetData(TowerSO towerSO)
    {
        this.towerSO = towerSO;
        currentState = 0;
        numberOfStates = towerSO.statesSprites.Count;
        spriteRenderer.sprite = towerSO.statesSprites[currentState];
    }

    public GameObject GetTowerTileGameObject()
    {
        return gameObject;
    }

    public bool Upgrade()
    {
        if (currentState >= numberOfStates - 1)
        {
            return false;
        }

        spriteRenderer.sprite = towerSO.statesSprites[++currentState];

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