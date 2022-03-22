using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBossFire : MonoBehaviour
{
    
    [SerializeField]
    private GameObject _enemyWeaponPrefab;


    public void MidBossFireWeapon()
    {
        GameObject enemyMissile = Instantiate(_enemyWeaponPrefab, transform.position, Quaternion.Euler(0, -90f, 0));
        enemyMissile.transform.gameObject.layer = 9;
        Bullet[] bullets = enemyMissile.GetComponentsInChildren<Bullet>();
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].MoveLeft();
        }
    }

    IEnumerator RapidFire()
    {
        yield return new WaitForSeconds(0.01f);
    }
}
