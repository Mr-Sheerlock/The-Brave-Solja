using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBulletsUpgrade : Upgrade
{
    

    private void Start()
    { 
        GameObject Player = GameObject.Find("Player");
        gunController = Player.GetComponent<GunController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Upgrade();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Sr.enabled == true)
        {
            Sr.color = new Color(Sr.color.r, Sr.color.g, Sr.color.b, 1f);
        }
    }

    void Upgrade ()
    {
        if(gunController.weapon as ProjectileGun)
        {
            Sr.enabled = false;
            collider.enabled = false;
            ((ProjectileGun)gunController.weapon).SetNumberOfProjectiles(((ProjectileGun)gunController.weapon).numberofProjectiles+1);

        }
        else
        {
            Sr.color= new Color(Sr.color.r, Sr.color.g, Sr.color.b, 0.4f);
        }
    }
}
