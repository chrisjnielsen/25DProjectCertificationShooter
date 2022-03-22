using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointStyle3 : MonoBehaviour
{
    [SerializeField]
    private GameObject[] waypoints = new GameObject[6];

    private void OnDrawGizmos()
    {
        for (int i = 0, j = i + 1; i < waypoints.Length - 1; i++, j++)
        {   //Draw waypoints from point a to b sequentially
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(waypoints[i].transform.position, 0.4f);
            Gizmos.DrawLine(waypoints[i].transform.position, waypoints[j].transform.position);
            if (j + 1 == waypoints.Length)
            {
                
                Gizmos.DrawSphere(waypoints[j].transform.position, 0.4f);
                Gizmos.DrawLine(waypoints[j].transform.position, waypoints[0].transform.position);
            }

        }
    }
}
