using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private Enemy enemy;
    private Animator animator;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
    }

    private void PlayHurtAnimation(Enemy enemy)
    {
        animator.SetTrigger("Hurt");
    }

    private void PlayDeathAnimation(Enemy enemy)
    {
        animator.SetTrigger("Death");
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyHit += PlayHurtAnimation;
        EnemyHealth.OnEnemyDeath += PlayDeathAnimation;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyHit -= PlayHurtAnimation;
        EnemyHealth.OnEnemyDeath -= PlayDeathAnimation;
    }
}
