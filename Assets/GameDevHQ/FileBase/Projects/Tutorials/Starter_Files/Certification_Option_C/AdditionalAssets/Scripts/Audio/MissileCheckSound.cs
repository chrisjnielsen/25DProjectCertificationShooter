using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCheckSound : MonoBehaviour
{
    private bool isPicked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Missile" && isPicked == false)
        {
            isPicked = true;
            AudioPlayer.Instance.playMissileBy();
            isPicked = false;
        }
    }

}
