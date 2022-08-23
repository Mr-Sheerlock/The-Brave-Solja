using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public int Damage;


    //Collectible related stuff
    public Weapon weapon;
    public Collectible collectible;
    [SerializeField] GameObject bullet;

    
    public void SetGCDamage(int damage)
    {
        Damage = damage;
    }

    void Awake()
    {
        Damage = 1;
        collectible = null;
        if (weapon)
        {
            weapon.SetDamage(Damage);
        }
    }


    public void ShootWeapon(Transform shootingpoint,Vector2 aimDirection)
    {
        if (weapon)
            weapon.Shoot(shootingpoint, aimDirection);

        //Debug.Log(shootingpoint.position);

    }

    public void DontShootWeapon()
    {
        if(weapon)
        weapon.DontShoot();
    }

    
    
    
    public void ChangeWeapon(Weapon collectedweapon, bool destroy)
    {
        if (weapon && destroy)
        {
            Destroy(weapon.gameObject);

        }
        weapon = collectedweapon;
        weapon.SetDamage(Damage);
        if (gameObject.tag == "Player" && bullet!=null)
        {
            if (weapon.GetComponent<ProjectileGun>())
            {
                ((ProjectileGun)weapon).SetBullet(bullet);  
            }
        }
        
    }

    public void ChangeCollectible(Collectible newcollectible)
    {
        if (!collectible)
        {
            collectible = newcollectible;

        }
        else
        {
            Collectible temp = collectible;
            collectible = newcollectible;
            
            temp.Spawn(transform.position);

        }
        
    }

    

}
