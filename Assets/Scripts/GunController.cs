using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField]
    ProjectileController projectilePrefab;

    public void Shoot()
    {
        ProjectileController projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

        projectile.speed = 20;
    }

}
