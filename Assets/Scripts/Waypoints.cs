using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] private Vector3[] points;

    private Vector3 originPosition;

    private void Start()
    {
        originPosition = transform.position;
    }

    public Vector3 GetWaypointPosition(int index)
    {
        return originPosition + points[index];
    }

    public int GetPointsCount()
    {
        return points.Length;
    }

    private void OnDrawGizmos()
    {
        if (transform.hasChanged)
        {
            originPosition = transform.position;
        }

        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(points[i] + originPosition, .5f);
        }
    }

}
