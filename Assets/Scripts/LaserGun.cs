using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Weapon
{
    ///Specs/// 
    float timer;
    float LaserWidth{get;set;}
    
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
        timer = 0;
        LaserWidth = 0.5f;
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
        lineRenderer.widthMultiplier = LaserWidth;
        lineRenderer.SetPosition(0, ShootingPoint.position); //start
        lineRenderer.SetPosition(1, (Vector2)ShootingPoint.position+ AimDirection.normalized * range); //end

        RaycastHit2D hit=Physics2D.CircleCast(ShootingPoint.position, lineRenderer.widthMultiplier/2, AimDirection, range,(1<<3) + (1<<7) );

        if (hit)
        {

            lineRenderer.SetPosition(1, hit.point);
            GameObject lol = Instantiate(HitEffect, hit.point, hit.transform.rotation);

            if (hit.collider.GetComponent<Health>())
            {
                hit.collider.GetComponent<Health>().TakeDamage(damage);
            }

        }


    }


    private void OnDestroy()
    {
        DontShoot();
    }

    public override void DontShoot() {

        timer = 0;
        if (lineRenderer)
        lineRenderer.enabled = false;
    }

    

}
