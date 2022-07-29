using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Weapon
{
    float timer;
    int numberofLasers;
    //float TimeSinceLastShoot;
    public LineRenderer lineRenderer;

    public GameObject HitEffect;
    public GameObject Laser;

    Quaternion rotationoffset;


    float range;
    private void Start()
    {
        range = 20;
        damage = 1;
        //TimeSinceLastShoot = 0f;
        numberofLasers = 1;
    }
    
    public override void Shoot(Transform ShootingPoint, Vector2 AimDirection)
    {

        //linrenderer on 
        if(!lineRenderer)
        {
            GameObject laser = GameObject.Find("Laser(Clone)");
            if(!laser)
            laser = Instantiate(Laser, ShootingPoint.position, rotationoffset);
            lineRenderer=laser.GetComponent<LineRenderer>();

        }

        lineRenderer.enabled = true;
        lineRenderer.widthMultiplier = 0.5f;
        lineRenderer.SetPosition(0, ShootingPoint.position); //start
        lineRenderer.SetPosition(1, (Vector2)ShootingPoint.position+ AimDirection.normalized * range); //end

        RaycastHit2D hit=Physics2D.CircleCast(ShootingPoint.position, lineRenderer.widthMultiplier/2, AimDirection, range);

        if (hit)
        {
            if(hit.collider.tag== "Walls")
            {
                lineRenderer.SetPosition(1, hit.point);
                GameObject lol = Instantiate(HitEffect, hit.point, hit.transform.rotation);

            }
        }

    
    }


    private void OnDestroy()
    {
        DontShoot();
    }

    public override void DontShoot() { 
         
        if(lineRenderer)
        lineRenderer.enabled = false;
    }

    

}
