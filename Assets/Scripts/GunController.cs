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
    
    public void SetDamage(int damage)
    {
        Damage = damage;
    }

    void Start()
    {
        Damage = 1;
        collectible = null;
    }

    public void ShootWeapon(Transform shootingpoint,Vector2 aimDirection)
    {
        if (weapon)
            weapon.Shoot(shootingpoint, aimDirection);
        //else
        //    Debug.Log("Lol no shoot");

    }

    public void DontShootWeapon()
    {
        if(weapon)
        weapon.DontShoot();
    }

    
    
    
    public void ChangeWeapon(Weapon collectedweapon)
    {
        if (weapon)
        {
            Destroy(weapon.gameObject);

        }
        weapon = collectedweapon;
        weapon.SetDamage(Damage);
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
