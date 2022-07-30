using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : Weapon
{
    ////////////Specs//////////
    public int numberofProjectiles;
    public float gunWidth;

    ////////////timer//////////
    public float timer=0;
    float TimeSinceLastShoot;
    float PeriodicTime;

    ////////////Bullet//////////
    public GameObject bullet;
    public float FireForce;
    public static float Zrotaion;
    Vector3 AnglesOffset;
    Quaternion BulletRotation;


    
    private void Start()
    {
        damage = 5;
        gunWidth = 3f;
        FireForce = 5f;
        Zrotaion = 90f;
        TimeSinceLastShoot = 0f;
        AnglesOffset = new Vector3(0, 0, Zrotaion);
        numberofProjectiles = 1;
        timer = Time.time;
        PeriodicTime = 0.3f;
    }
    
    public void SetPeriodictime(float newtime)
    {
        PeriodicTime = newtime;
    }

    public void SetNumberOfProjectiles(int newnumber)
    {
        numberofProjectiles = newnumber;
    }
    public override void Shoot(Transform ShootingPoint, Vector2 AimDirection)
    {
        TimeSinceLastShoot = Time.time - timer;


        if (TimeSinceLastShoot >= PeriodicTime)
        {
            timer = Time.time;
            BulletRotation.eulerAngles = AnglesOffset + ShootingPoint.rotation.eulerAngles;
            
            if (numberofProjectiles == 1)
            {
                ShootSingle(ShootingPoint, AimDirection);
            }
            else
            {
                Vector2 Offset = new Vector2(0, 0);
                float magnitude = 0;
                float increment = gunWidth / numberofProjectiles;
                int i;
                for (i = 0; i < numberofProjectiles-1; i++)
                {
                    if (i % 2 == 0)
                    {
                        magnitude += increment;
                        Offset = magnitude * (ShootingPoint.right);
                    }
                    else
                    {
                        Offset = -magnitude * (ShootingPoint.right);

                    }
                    GameObject currentbullet = Instantiate(bullet, ShootingPoint.position + (Vector3)Offset, BulletRotation);
                    BulletBehaviour lol = currentbullet.GetComponent<BulletBehaviour>();
                    lol.SetDamage(damage);
                    currentbullet.GetComponent<Rigidbody2D>().AddForce(AimDirection * FireForce, ForceMode2D.Impulse);
                }

                if(i % 2 == 1)
                {
                    ShootSingle(ShootingPoint, AimDirection);
                }
                else
                {
                    magnitude += increment;
                    Offset = magnitude * (ShootingPoint.right);
                    GameObject currentbullet = Instantiate(bullet, ShootingPoint.position + (Vector3)Offset, BulletRotation);
                    BulletBehaviour lol = currentbullet.GetComponent<BulletBehaviour>();
                    lol.SetDamage(damage);
                    currentbullet.GetComponent<Rigidbody2D>().AddForce(AimDirection * FireForce, ForceMode2D.Impulse);
                }


            }

        }
    }

    void ShootSingle(Transform ShootingPoint, Vector2 AimDirection)
    {
        GameObject currentbullet = Instantiate(bullet, ShootingPoint.position, BulletRotation);

        //set Damage
        BulletBehaviour lol = currentbullet.GetComponent<BulletBehaviour>();
        lol.SetDamage(damage);
        currentbullet.GetComponent<Rigidbody2D>().AddForce(AimDirection * FireForce, ForceMode2D.Impulse);
    }
    
    public override void DontShoot() { }


    private void OnDestroy()
    {
        Debug.Log("3leek wa7ed lo l");
    }
}
