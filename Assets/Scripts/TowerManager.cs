using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private TowerSO towerSO;

    private int currentState;
    private int numberOfStates;

    private List<Enemy> enemiesInRange = new List<Enemy>();
    private Enemy currentTarget;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        GetTarget();
        ShootTarget();
    }

    private void GetTarget()
    {
        if (enemiesInRange.Count > 0)
        {
            currentTarget = enemiesInRange[0];
            return;
        }

        currentTarget = null;
    }

    private void ShootTarget()
    {
        if (currentTarget == null)
        {
            return;
        }

        Debug.DrawLine(transform.position, currentTarget.transform.position, Color.red);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesInRange.Contains(other.GetComponent<Enemy>()))
            {
                enemiesInRange.Remove(other.GetComponent<Enemy>());
            }
        }
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