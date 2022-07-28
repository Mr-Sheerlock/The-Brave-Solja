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
 

    Vector2 CollectibleSpawnOffset;
    public float SearchRadius;
    Vector2 CollectibleSpawnPosition;
    Vector2 randomBools;

    void Start()
    {
        Damage = 1;
        CollectibleSpawnOffset =  new Vector2(3,3);
        //weapon = GetComponent<ProjectileGun>();
        SearchRadius = 5;
        collectible = null;
        Destroy(weapon);
        weapon = null;
    }
    private void Update()
    {
        CheckforValidPosition();
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
    void CheckforValidPosition()
    {
           //1000 layer 3 

        // This would cast rays only against colliders in layer 3.
        while (true)
        {
            if (Physics2D.Raycast(transform.position, CollectibleSpawnOffset.normalized, SearchRadius, 8)) //bitmask 1000 layer 3 
            {
            
                CollectibleSpawnOffset = new Vector2(Random.Range(3f, 4f), Random.Range(4f, 5f));
                randomBools = new Vector2(Random.value > 0.5 ? 1:-1, Random.value > 0.5 ? 1 : -1);
                CollectibleSpawnOffset *= randomBools;
                Debug.Log("Did Hit");
            }
            else
            {
                //didn't hit
                CollectibleSpawnPosition= transform.position + (Vector3)CollectibleSpawnOffset;
                break;
                
            }


        }
        return;
    }
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

            CheckforValidPosition();
            temp.Spawn(CollectibleSpawnPosition);

        }
        
    }

    

}
