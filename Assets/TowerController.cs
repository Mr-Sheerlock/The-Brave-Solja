using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{

    ///Specs///

    int Damage;
    public Transform[] Shootinpoints;
    Weapon[] Weapons;
    public Weapon weapontype;
    float PeriodicTime;
    public GunController gunController;


    void Start()
    {
        Damage = 1;
        PeriodicTime = 0.6f;
        Weapons = new Weapon[4];
        SetWeapons(weapontype);
    }

    void SetWeapons(Weapon weapon)
    {
        for (int i = 0; i < 4; i++)
        {
            //Debug.Log("lolerrere");
            Weapons[i] = Instantiate(weapon, transform.position, transform.rotation);
            if ((ProjectileGun)Weapons[i])
            {

            Debug.Log("loleer");
                ((ProjectileGun)Weapons[i]).timer = Time.time;
                ((ProjectileGun)Weapons[i]).SetPeriodictime(PeriodicTime);
            }
        }
    }

    void ShootWeapons()
    {
        for (int i = 0; i < 4; i++)
        {
            gunController.ChangeWeapon(Weapons[i]);
            gunController.ShootWeapon(Shootinpoints[i], Shootinpoints[i].up);
        }

    }


    // Update is called once per frame
    void Update()
    {
        ShootWeapons();
    }
}
