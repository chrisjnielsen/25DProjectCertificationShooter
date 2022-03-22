using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _weaponPrefab;
    [SerializeField]
    private GameObject _enemyWeaponPrefab;

    public void FireWeapon(int weaponIndex)
    {
        GameObject go = Instantiate(_weaponPrefab[weaponIndex], transform.position, Quaternion.identity);
        go.transform.gameObject.layer = 7;
        //StartCoroutine(RapidFire());
    }

    public void EnemyBullet()
    {
        GameObject enemyBullet = Instantiate(_enemyWeaponPrefab, transform.position, Quaternion.Euler(0, -180f, 0));
        enemyBullet.transform.gameObject.layer = 9;
        Bullet[] bullets = enemyBullet.GetComponentsInChildren<Bullet>();
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].MoveLeft();
        }
    }

    public void EnemyMissile()
    {
        GameObject enemyBullet = Instantiate(_enemyWeaponPrefab, transform.position, Quaternion.Euler(0, -90f, 0));
        enemyBullet.transform.gameObject.layer = 9;
        Bullet[] bullets = enemyBullet.GetComponentsInChildren<Bullet>();
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
