using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public static Action<Enemy> OnEnemyDeath;
    public static Action<Enemy> OnEnemyHit;

    [SerializeField] private Image healthFillImage;
    [SerializeField] float maxHealth;

    private float currentHealth;
    private Enemy enemy;

    private void Start()
    {
        currentHealth = maxHealth;
        healthFillImage.fillAmount = 1;
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("deal damage");
            DealDamage(1f);
        }
    }

    private void DealDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            HandleDeath();
        }
        else
        {
            OnEnemyHit?.Invoke(enemy);
        }
        healthFillImage.fillAmount = currentHealth / maxHealth;
    }

    private void HandleDeath()
    {
        OnEnemyDeath?.Invoke(enemy);
    }
}
