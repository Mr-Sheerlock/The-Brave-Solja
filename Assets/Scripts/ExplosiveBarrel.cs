using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    float timer = 0;
    int Maxhealth { get; set; }
    int CurrentHealth { get; set; }
    int healregen { get; set; }

    public HealthbarScript HB;
    //GameObject Healthbar;
    // Start is called before the first frame update
    void Start()
    {
        Maxhealth = 100;
        CurrentHealth = 100;
        healregen = 0;

        HB = gameObject.GetComponentInChildren<HealthbarScript>();
        //HB = 

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= 1)
        {
            CurrentHealth += healregen;
            timer = 0;
            if (HB)
                (HB)?.SetSize((float)CurrentHealth / Maxhealth);
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        (HB)?.SetSize((float)CurrentHealth / Maxhealth);

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            //Die();
        }
    }

    void Die()
    {

    }

}
