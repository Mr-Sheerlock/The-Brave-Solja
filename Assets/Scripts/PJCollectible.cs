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
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Player")
        {
            //first tell the gunController to spawn the other object
            Debug.Log("TrgPJ");
            spriteRenderer.enabled = false;
            box.enabled = false;
            Weapon lol = Instantiate(weapon, transform.position, transform.rotation);
            ((ProjectileGun)lol).timer = Time.time;

            gunController.ChangeWeapon(lol);
            gunController.ChangeCollectible(this);
        }

    }

    


}
