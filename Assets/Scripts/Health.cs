using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Health : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent CustomEvent;
    float timer = 0;
    public int Maxhealth { get; set; }
    public int CurrentHealth { get; set; }
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
        if (timer >=1 )
        {
            CurrentHealth += healregen;
            timer=0; 
            if(HB)
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
            CustomEvent.Invoke();
        }
    }


}
