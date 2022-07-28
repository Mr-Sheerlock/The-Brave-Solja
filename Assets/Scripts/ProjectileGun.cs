using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : Weapon
{
    float minimumDelay;
    float ForceofProjectile;
    int numberofProjectiles;


    ////////////timer//////////
    public float timer=0;
    float TimeSinceLastShoot;

    public GameObject bullet;

    public float FireForce = 50f;

    public static float Zrotaion = 90f;
    Vector3 Euleroffset;//= new Vector3(0, 0, Zrotaion);
    Quaternion rotationoffset;

    
    private void Start()
    {
        damage = 1;
        TimeSinceLastShoot = 0f;
        Euleroffset = new Vector3(0, 0, Zrotaion);
        numberofProjectiles = 1;
        timer = Time.time;
    }
    

    public override void Shoot(Transform shootingPoint)
    {
        TimeSinceLastShoot = Time.time - timer;


        if (TimeSinceLastShoot >= 0.2f)
        {
           
            Debug.Log("ShootGOWA");
            timer = Time.time;
            //rotationoffset.eulerAngles = Euleroffset ;

            rotationoffset.eulerAngles = Euleroffset + shootingPoint.rotation.eulerAngles;
            if (numberofProjectiles == 1)
            {
                
                GameObject currentbullet = Instantiate(bullet, shootingPoint.position, rotationoffset);
                currentbullet.GetComponent<Rigidbody2D>().AddForce(shootingPoint.up * FireForce, ForceMode2D.Impulse);


            }
            else
            {
              
            }

        }
    }

    
    
    public override void DontShoot() { }

    

}
