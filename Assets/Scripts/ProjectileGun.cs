using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : Weapon
{

    int numberofProjectiles;

    ////////////timer//////////
    public float timer=0;
    float TimeSinceLastShoot;

    ////////////Bullet//////////
    public GameObject bullet;
    public float FireForce;
    public static float Zrotaion;
    Vector3 AnglesOffset;
    Quaternion BulletRotation;

    
    private void Start()
    {
        FireForce = 5f;
        Zrotaion = 90f;
        damage = 1;
        TimeSinceLastShoot = 0f;
        AnglesOffset = new Vector3(0, 0, Zrotaion);
        numberofProjectiles = 1;
        timer = Time.time;
    }
    

    public override void Shoot(Transform ShootingPoint, Vector2 AimDirection)
    {
        TimeSinceLastShoot = Time.time - timer;


        if (TimeSinceLastShoot >= 0.3f)
        {
            timer = Time.time;
            BulletRotation.eulerAngles = AnglesOffset + ShootingPoint.rotation.eulerAngles;
            
            if (numberofProjectiles == 1)
            {   
                GameObject currentbullet = Instantiate(bullet, ShootingPoint.position, BulletRotation);
                currentbullet.GetComponent<Rigidbody2D>().AddForce(AimDirection * FireForce, ForceMode2D.Impulse);
            }
            else
            {
              
            }

        }
    }

    
    
    public override void DontShoot() { }

    

}
