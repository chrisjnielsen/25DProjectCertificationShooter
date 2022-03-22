using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWave.asset", menuName = "ScriptableObjects/new Wave")]
public class Wave : ScriptableObject
{
    public List<GameObject> sequence;

}