using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Action<Enemy> OnEndPointReached;

    [SerializeField] private float movementSpeed;

    private Waypoints waypoints;
    private Vector3 destinationPointPosition;
    private Vector3 previousPointPosition;
    private int destinationPointIndex;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        waypoints = GameObject.FindWithTag("Spawner").GetComponent<Waypoints>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        destinationPointPosition = waypoints.GetWaypointPosition(0);
        destinationPointIndex = 0;
    }

    void Update()
    {
        Move();
        Rotate();

        if (IsDestinationPointReached())
        {
            UpdateDestinationPoint();
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destinationPointPosition, movementSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        if (destinationPointPosition.x > previousPointPosition.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    private bool IsDestinationPointReached()
    {
        float distanceToDestinationPoint = (transform.position - destinationPointPosition).magnitude;
        if (distanceToDestinationPoint < 0.1f)
        {
            return true;
        }

        return false;
    }

    private void UpdateDestinationPoint()
    {
        destinationPointIndex++;

        if (destinationPointIndex < waypoints.GetPointsCount())
        {
            previousPointPosition = transform.position;
            destinationPointPosition = waypoints.GetWaypointPosition(destinationPointIndex);
        }
        else
        {
            HandleEndPointReached();
        }
    }

    private void HandleEndPointReached()
    {
        Destroy(gameObject);
        OnEndPointReached?.Invoke(this);
    }
}