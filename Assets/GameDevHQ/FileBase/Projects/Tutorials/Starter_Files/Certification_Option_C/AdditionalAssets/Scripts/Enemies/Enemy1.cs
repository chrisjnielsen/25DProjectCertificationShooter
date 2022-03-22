using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    public override void Start()
    {
        base.Start();
        waypoints = GameManager.Instance.Waypoints;
        this.transform.position = waypoints[wayPointIndex].transform.position;
        
    }
    public override void Update()
    {
        base.Update();
        if(_player !=null) CalculateMovement();
    }
    public override void CalculateMovement()
    {
        //enemy follows set of waypoints instead of basic movement
        if (waypoints.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[wayPointIndex].transform.position, _speed * Time.deltaTime);
            if (transform.position == waypoints[wayPointIndex].transform.position) wayPointIndex += 1;
            if (wayPointIndex == waypoints.Count) wayPointIndex = 0;
        }
        else transform.Translate(Vector3.zero);
        
    }
}
