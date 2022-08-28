using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurhealthUpgrade : Upgrade
{



    [SerializeField] int CurHealthToAdd = 10;

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
        if(Player.GetComponent<MainController>())
        {
            Sr.enabled = false;
            collider.enabled = false;
            PlayerHealth.IncCurrentHealth(CurHealthToAdd);
            Destroy(gameObject);
        }
        else
        {
            Sr.color= new Color(Sr.color.r, Sr.color.g, Sr.color.b, 0.4f);
        }
    }
}
