using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossFire : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyWeaponPrefab;
    [SerializeField]
    private GameObject _upperSpawn;
    [SerializeField]
    private GameObject _lowerSpawn;

    public void FinalBossFireWeapon()
    {
        GameObject enemyFire1 = Instantiate(_enemyWeaponPrefab, _upperSpawn.transform.position, Quaternion.Euler(0, -180f, 0));
        GameObject enemyFire2 = Instantiate(_enemyWeaponPrefab, _lowerSpawn.transform.position, Quaternion.Euler(0, -180f, 0));
        MoveToLayer(enemyFire1.transform, 9);
        MoveToLayer(enemyFire2.transform, 9);
    }

    void MoveToLayer(Transform root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            MoveToLayer(child, layer);
    }

    IEnumerator RapidFire()
    {
        yield return new WaitForSeconds(0.01f);
    }
}
