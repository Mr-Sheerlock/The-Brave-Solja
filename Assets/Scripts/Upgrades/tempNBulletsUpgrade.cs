using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempNBulletsUpgrade : Upgrade
{
    [SerializeField] int BulletsToAdd = 3;
    float timer;
    public float upgradeTime;
    bool Active;


    private void Start()
    {
        timer = 0f;
        upgradeTime = 5f;
        Active = false;
    }

    private void Update()
    {
        //timer += Time.deltaTime;
        if (Active)
        {
            StartCoroutine(WaitForDegrade());
        }
        //if (timer > upgradeTime)
        //{
        //    Degrade();
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Upgrade();
            Active = true; 
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
        if (Player.GetComponent<MainController>())
        {
            Sr.enabled = false;
            collider.enabled = false;
            mainController.IncNProjectiles(BulletsToAdd); 
            timer = 0f;
            Active = true;

        }
        else
        {
            Sr.color = new Color(Sr.color.r, Sr.color.g, Sr.color.b, 0.4f);
        }
    }


    IEnumerator WaitForDegrade()
    {
        yield return new WaitForSeconds(upgradeTime);
        Degrade();
    }

    void Degrade()
    {
        mainController.IncNProjectiles(-BulletsToAdd);
        Destroy(gameObject);
    }
}
