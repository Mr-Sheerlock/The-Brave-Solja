using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    Vector2 mousePosition;
    Vector2 aimDirection;

    int Damage;


    //Collectible related stuff
    public Weapon weapon;
    public Collectible collectible;
 

    void Start()
    {
        Damage = 1;
        collectible = null;
        Destroy(weapon);
        weapon = null;
    }

    public void ShootWeapon(Transform shootingpoint)
    {
        if(weapon)
        weapon.Shoot(shootingpoint);

    }

    public void DontShootWeapon()
    {
        if(weapon)
        weapon.DontShoot();
    }

    //RaycastHit hit;
    //public LayerMask layermask;
    
    
    
    public void ChangeWeapon(Weapon collectedweapon)
    {
        if (weapon)
        {
            Destroy(weapon.gameObject);

        }
        weapon = collectedweapon;
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
            //hanb3tlo el transform.position instead w howa hay3ml kol 7aga gowa 
            temp.Spawn(transform.position);

        }
        
    }

    

}
