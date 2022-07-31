using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempNBulletsUpgrade : Upgrade
{

    float timer;
    public float upgradeTime;
    bool Active;


    private void Start()
    {
        timer = 0f;
        upgradeTime = 5f;
        GameObject Player = GameObject.Find("Player");
        gunController = Player.GetComponent<GunController>();
        Active = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (!Active) timer = 0f;
        if (timer > upgradeTime)
        {
            Degrade();
        }
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

    void Upgrade()
    {
        if (gunController.weapon as ProjectileGun)
        {
            Sr.enabled = false;
            collider.enabled = false;
            ((ProjectileGun)gunController.weapon).SetNumberOfProjectiles(((ProjectileGun)gunController.weapon).numberofProjectiles + 3);
            timer = 0f;
            Active = true;

        }
        else
        {
            Sr.color = new Color(Sr.color.r, Sr.color.g, Sr.color.b, 0.4f);
        }
    }
    void Degrade()
    {
        ((ProjectileGun)gunController.weapon).SetNumberOfProjectiles(((ProjectileGun)gunController.weapon).numberofProjectiles - 3);
        Destroy(gameObject);   
    }
}
