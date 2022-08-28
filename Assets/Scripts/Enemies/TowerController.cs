using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{

    ///Specs///
    public static int  lasercount=1; // a lasercount for making the laser names unique
    [SerializeField] int Damage=5;
    int numberofBullets = 1;
    public Transform[] Shootinpoints= new Transform[4];
    Weapon[] Weapons;
    public Weapon weapontype;
    float PeriodicTime = 0.6f;
    public GunController gunController;

    float timer;
    float TimeShooting = 0.6f;
    float AmountOfRotation = 50f;

    [SerializeField] GameObject DeathEffect;
    [SerializeField] AudioClip DeathSound;

    void Start()
    {
        timer = 0;
        Weapons = new Weapon[4];
        SetWeapons(weapontype);
        //are shooting points initialized ?? 
    }

    void SetWeapons(Weapon weapon)
    {
        for (int i = 0; i < 4; i++)
        {
            Weapon lol = Instantiate(weapon, transform.position, transform.rotation,transform);
            Weapons[i] = lol;
            Weapons[i].SetDamage(Damage);
            if (Weapons[i] as ProjectileGun)
            {

                ((ProjectileGun)Weapons[i]).timer = Time.time;
                ((ProjectileGun)Weapons[i]).SetNumberOfProjectiles(numberofBullets);
                ((ProjectileGun)Weapons[i]).SetPeriodictime(PeriodicTime);
            }
            if (Weapons[i] as LaserGun)
            {
                ((LaserGun)Weapons[i]).SetLaserName("LaserTower"+lasercount.ToString());
                lasercount++;
                ((LaserGun)Weapons[i]).gameObject.GetComponent<AudioSource>().enabled=false;
            }
        }
    }

    void ShootWeapons()
    {
        for (int i = 0; i < 4; i++)
        {
            gunController.ChangeWeapon(Weapons[i],false);
            gunController.ShootWeapon(Shootinpoints[i], Shootinpoints[i].up);
        }

    }

    void DontShootWeapons()
    {
        for (int i = 0; i < 4; i++)
        {
            gunController.ChangeWeapon(Weapons[i], false);
            gunController.DontShootWeapon();
        }

    }


    // Make a loop. One for each weapon type;
    void Update()
    {
        //PJ Loop

        //Laser Loop
        timer += Time.deltaTime;
        if (timer < TimeShooting)
        {
            ShootWeapons();
        }

        if (timer > TimeShooting)
        {
            DontShootWeapons();
        }
        if (timer > 1f)
            timer = 0;
        transform.eulerAngles += Vector3.forward * AmountOfRotation*Time.deltaTime;

    }

    private void FixedUpdate()
    {
    }

    public void  Die()
    {
        GameObject lol = Instantiate(DeathEffect, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(DeathSound, transform.position);

        Destroy(gameObject);
    }


    private void OnDestroy()
    {
        for (int i = 0; i < 4; i++)
        {
            Destroy(Weapons[i]);
        }
    }
}
