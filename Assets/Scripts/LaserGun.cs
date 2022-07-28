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

    public float FireForce = 50f;

    public static float Zrotaion = 90f;
    Vector3 Euleroffset;//= new Vector3(0, 0, Zrotaion);
    Quaternion rotationoffset;


    float range;
    private void Start()
    {
        range = 20;
        damage = 1;
        //TimeSinceLastShoot = 0f;
        //Euleroffset = new Vector3(0, 0, Zrotaion);
        numberofLasers = 1;
    }
    
    public override void Shoot(Transform shootingPoint)
    {

        //linrenderer on 
        //rotationoffset.eulerAngles = Euleroffset + shootingPoint.rotation.eulerAngles;
        lineRenderer.enabled = true;
        lineRenderer.widthMultiplier = 0.5f;
        lineRenderer.SetPosition(0, shootingPoint.position);

        var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)shootingPoint.position;


        lineRenderer.SetPosition(0, shootingPoint.position); //start

        
        
        lineRenderer.SetPosition(1, (Vector2)shootingPoint.position+direction.normalized * range); //end

        


        //lineRenderer.SetPosition(1, shootingPoint.up* range);
        RaycastHit2D hit=Physics2D.CircleCast(shootingPoint.position, lineRenderer.widthMultiplier/2, direction, range);

        if (hit)
        {
            if(hit.collider.tag== "Walls")
            {
                lineRenderer.SetPosition(1, hit.point);
                GameObject lol = Instantiate(HitEffect, hit.point, hit.transform.rotation);

            }
        }





    }

    
    
    public override void DontShoot() { 
            
        lineRenderer.enabled = false;
    }

    

}
