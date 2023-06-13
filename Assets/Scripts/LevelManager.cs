using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int lives;

    private void ReduceLives(Enemy enemy)
    {
        lives--;
        if (lives <= 0)
        {
            lives = 0;
            GameOver();
        }
    }

    private void GameOver()
    {
        //TODO: 
        return;
    }

    private void OnEnable()
    {
        Enemy.OnEndPointReached += ReduceLives;
    }

    private void OnDisable()
    {
        Enemy.OnEndPointReached -= ReduceLives;
    }

}
