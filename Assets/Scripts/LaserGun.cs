using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Weapon
{
    ///Specs/// 
    float LaserWidth{get;set;}
    
    public LineRenderer lineRenderer;

    public List<GameObject> HitEffects;
    public List<GameObject> ShootEffects;
    public GameObject Laser;

    Quaternion rotationoffset;

    public void AddHitEffect(string PrefName)
    {
        HitEffects.Add( GameObject.Find(PrefName));
    }
    public void AddShootEffect(string PrefName)
    {
        ShootEffects.Add(GameObject.Find(PrefName));
    }

    public override void  SetDamage(int x)
    {
        damage = x;
        LaserWidth = Mathf.Max(0.6f,0.2f+ damage/15); //0.2 -> 0.6
    }



    float range;
    private void Awake()
    {
        range = 20;
        SetDamage(1);
        //TimeSinceLastShoot = 0f;
        //LaserWidth = 0.5f;
    }

    //for Towers and Enemies
    public void SetLaserName(string LaserName)
    {
        GameObject laser = GameObject.Find(LaserName);
        if (!laser)
        {
            laser = Instantiate(Laser, transform.position, rotationoffset,transform);
            laser.name =LaserName;
        }
        lineRenderer = laser.GetComponent<LineRenderer>();
        lineRenderer.name = LaserName;
    }
    public override void Shoot(Transform ShootingPoint, Vector2 AimDirection)
    {

        //linrenderer on 
        

        lineRenderer.enabled = true;
        //lineRenderer.widthMultiplier = LaserWidth;
        lineRenderer.SetPosition(0, ShootingPoint.position-ShootingPoint.up*0.5f); //start

        for (int i = 0; i < ShootEffects.Count; i++)
        {
            GameObject lol = Instantiate(ShootEffects[i], ShootingPoint.position - ShootingPoint.up * 0.5f, ShootingPoint.transform.rotation);
        }
        lineRenderer.SetPosition(1, (Vector2)ShootingPoint.position+ AimDirection.normalized * range); //end

        RaycastHit2D hit = Physics2D.CircleCast(ShootingPoint.position, lineRenderer.widthMultiplier / 2, AimDirection, range, (1 << 3) + (1 << 7) + (1 << 8) + (1 << 9));
        //RaycastHit2D hit = Physics2D.CircleCast(ShootingPoint.position, lineRenderer.widthMultiplier / 2, AimDirection, range, 0b1110001000);

        if (hit)
        {

            lineRenderer.SetPosition(1, hit.point);
            for(int i =0; i < HitEffects.Count; i++)
            {
                GameObject lol = Instantiate(HitEffects[i], hit.point, hit.transform.rotation);
            }

            if (hit.collider.GetComponent<Health>())
            {
                float LaserDamage = damage*Time.deltaTime*2;
                hit.collider.GetComponent<Health>().TakeDamage(LaserDamage);
            }
            if (hit.collider.tag == "Projectile")
            {
                if(hit.collider.gameObject.GetComponent< BulletBehaviour>())
                {
                    BulletBehaviour loler = hit.collider.gameObject.GetComponent<BulletBehaviour>();
                    loler.Collide();
                }
            }
        }


    }


    private void OnDestroy()
    {
        DontShoot();
    }

    public override void DontShoot() {

        if (lineRenderer)
        lineRenderer.enabled = false;
    }

    

}
