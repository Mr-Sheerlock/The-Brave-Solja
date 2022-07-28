using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Weapon weapon;
    Vector2 mousePosition;
    Vector2 aimDirection;


    private void Start()
    {
        //weapon = GetComponent<ProjectileGun>();
    }


    public void ShootWeapon(Transform shootingpoint)
    {
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //aimDirection = mousePosition - (Vector2) transform.position;
        //aimDirection = aimDirection.normalized;

        weapon.Shoot(shootingpoint);

    }

    public void DontShootWeapon()
    {
        weapon.DontShoot();
    }

    //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    aimDirection = mousePosition - rb.position;
    //    float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - aimOffset;
    //rb.rotation = aimAngle;

}
