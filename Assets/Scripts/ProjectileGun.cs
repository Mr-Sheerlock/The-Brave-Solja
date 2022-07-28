using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : Weapon
{
    float minimumDelay;
    float ForceofProjectile;
    int numberofProjectiles;
    float TimeSinceLastShoot;

    public GameObject bullet;

    public float FireForce = 50f;

    public static float Zrotaion = 90f;
    Vector3 Euleroffset;//= new Vector3(0, 0, Zrotaion);
    Quaternion rotationoffset;

    public Transform test1;
    public Transform test2;
    

    private void Start()
    {
        TimeSinceLastShoot = 0f;
        Euleroffset = new Vector3(0, 0, Zrotaion);
        numberofProjectiles = 1;
    }
    private void Update()
    {
        TimeSinceLastShoot += Time.deltaTime;
    }

    public override void Shoot(Transform shootingPoint)
    {
        if(TimeSinceLastShoot >= 0.2f)
        {

            Debug.Log("PJ lol ");
            //rotationoffset.eulerAngles = Euleroffset ;
        
            rotationoffset.eulerAngles = Euleroffset + shootingPoint.rotation.eulerAngles;
            if (numberofProjectiles == 1)
            {
                TimeSinceLastShoot = 0f;
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
