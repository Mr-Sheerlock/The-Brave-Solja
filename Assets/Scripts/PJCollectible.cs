using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJCollectible : Collectible
{
    private void Start()
    {
        
        GameObject Player=GameObject.Find("Player");
        gunController = Player.GetComponent< GunController > ();    
        CollectibleSpawnOffset = new Vector2(3, 3);
        SearchRadius = 5;
        _light = transform.GetChild(0).gameObject;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Player")
        {
            //first tell the gunController to spawn the other object
            Destroy(_light);
            spriteRenderer.enabled = false;
            box.enabled = false;
            Weapon temp = Instantiate(weapon, transform.position, transform.rotation);
            ((ProjectileGun)temp).timer = Time.time;

            gunController.ChangeWeapon(temp,true);
            gunController.ChangeCollectible(this);
        }

    }

    


}
