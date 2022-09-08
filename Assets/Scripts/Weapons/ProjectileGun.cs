using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : Weapon
{
    ////////////Specs//////////
    [SerializeField] int numberofProjectiles = 1;
    [SerializeField] float gunWidth=3f;

    ////////////timer//////////
    public float timer=0;
    float TimeSinceLastShoot=0;
    float PeriodicTime= 0.3f;

    ////////////Bullet//////////
    public GameObject bullet;
    float FireForce=30f;
    public static float Zrotaion=90f;
    Vector3 AnglesOffset=new Vector3(0, 0, Zrotaion);
    Quaternion BulletRotation;

    static int PlayerProjectiles=0;
    static int EnemyProjectiles=0;

    
    private void Awake()
    {   
        timer = Time.time;
        gunWidth = 3f;
    }
    
    public void SetPeriodictime(float newtime)
    {
        PeriodicTime = newtime;
    }

    public void SetNumberOfProjectiles(int newnumber)
    {
        numberofProjectiles = newnumber;
        gunWidth = numberofProjectiles;
    }
    public override void Shoot(Transform ShootingPoint, Vector2 AimDirection)
    {
        
        TimeSinceLastShoot = Time.time - timer;
    
        if (TimeSinceLastShoot >= PeriodicTime)
        {
            timer = Time.time;
            //Attempt 1: (Working, kinda)
            BulletRotation.eulerAngles = AnglesOffset + ShootingPoint.rotation.eulerAngles;
            
            //Debug.Log("Shooting Point rot is " + ShootingPoint.rotation.eulerAngles);
            //Debug.Log("Bullet Rot hence is " + BulletRotation.eulerAngles);
            //BulletRotation.eulerAngles = new Vector3(0,0,0.5f) + ShootingPoint.rotation.eulerAngles;
            
            if (numberofProjectiles == 1)
            {
                ShootSingle(ShootingPoint.position, AimDirection);
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
                    ShootSingle((Vector2)ShootingPoint.position + Offset, AimDirection);
                }

                if(i % 2 == 0)
                {
                    ShootSingle(ShootingPoint.position, AimDirection);
                }
                else
                {
                    Offset = -magnitude * (ShootingPoint.right);
                    ShootSingle((Vector2)ShootingPoint.position + Offset, AimDirection);
                }


            }

        }
    }

    void ShootSingle(Vector2 ShootingPoint, Vector2 AimDirection)
    {
        GameObject currentbullet = Instantiate(bullet, ShootingPoint, BulletRotation);
        //set Damage
        BulletBehaviour lol = currentbullet.GetComponent<BulletBehaviour>();
        lol.SetDamage(damage);
        if (AimDirection == Vector2.zero)
        {
            AimDirection = transform.up;
        }
        currentbullet.GetComponent<Rigidbody2D>().AddForce(AimDirection.normalized * FireForce, ForceMode2D.Impulse);
    }
    


    public void SetBullet(GameObject newBullet)
    {
        bullet = newBullet;
    }
    public override void DontShoot() { }


    private void OnDestroy()
    {
        //Debug.Log("3leek wa7ed lo l");
    }
}
