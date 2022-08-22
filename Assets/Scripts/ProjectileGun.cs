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
    [SerializeField] float FireForce;
    public static float Zrotaion;
    Vector3 AnglesOffset;
    Quaternion BulletRotation;



    
    private void Awake()
    {
        damage = 5;
        gunWidth = 3f;
        FireForce = 20f;
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
                //Mabywsl4 le hena
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

                    //GameObject currentbullet = Instantiate(bullet, ShootingPoint.position + (Vector3)Offset, BulletRotation);
                    //BulletBehaviour lol = currentbullet.GetComponent<BulletBehaviour>();
                    //lol.SetDamage(damage);
                    //currentbullet.GetComponent<Rigidbody2D>().AddForce(AimDirection * FireForce, ForceMode2D.Impulse);
                    ShootSingle((Vector2)ShootingPoint.position + Offset, AimDirection);
                }

                if(i % 2 == 0)
                {
                    ShootSingle(ShootingPoint.position, AimDirection);
                }
                else
                {
                    //magnitude += increment;
                    Offset = -magnitude * (ShootingPoint.right);
                    ShootSingle((Vector2)ShootingPoint.position + Offset, AimDirection);
                    //GameObject currentbullet = Instantiate(bullet, ShootingPoint.position + (Vector3)Offset, BulletRotation);
                    //BulletBehaviour lol = currentbullet.GetComponent<BulletBehaviour>();
                    //lol.SetDamage(damage);
                    //currentbullet.GetComponent<Rigidbody2D>().AddForce(AimDirection * FireForce, ForceMode2D.Impulse);
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
