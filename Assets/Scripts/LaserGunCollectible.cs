using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGunCollectible : Collectible
{
    float delay;
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
            Debug.Log("TrgLsr");
            spriteRenderer.enabled = false;
            box.enabled = false;
            Weapon lol = Instantiate(weapon, transform.position, transform.rotation);
            
            gunController.ChangeWeapon(lol);
            gunController.ChangeCollectible(this);
        }

    }




}
