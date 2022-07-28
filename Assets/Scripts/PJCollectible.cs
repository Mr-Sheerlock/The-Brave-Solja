using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJCollectible : Collectible
{
    float delay;
    private void Start()
    {
        //ReferencedScript refScript = GetComponent<ReferencedScript>();
        GameObject Player=GameObject.Find("Player");
        gunController = Player.GetComponent< GunController > ();

        //gunController = GameObject.Find("Gun Controller");
    }

    private void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Player")
        {
            //first tell the gunController to spawn the other object
            Debug.Log("Trg");
            spriteRenderer.enabled = false;
            box.enabled = false;
            Weapon lol = Instantiate(weapon, transform.position, transform.rotation);
            ((ProjectileGun)weapon).timer = Time.time;

            gunController.ChangeWeapon(weapon);
            gunController.ChangeCollectible(this);
        }

    }

    public override void  Spawn(Vector2 newposition)
    {
        //bywsl le hena 3ady 


        transform.position = newposition;
        spriteRenderer.enabled = true;
        box.enabled = true;
    }


}
