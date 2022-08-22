using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Health : MonoBehaviour
{
    float timer = 0;
    public int Maxhealth { get; set; }
    [SerializeField] public float CurrentHealth;
    float healregen { get; set; }
    
    [Header("Events")]
    public UnityEvent CustomEvent;

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

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        //Debug.Log("Damage Taken is" + damage);
        
        (HB)?.SetSize((float)CurrentHealth / Maxhealth);

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            CustomEvent.Invoke();
        }
    }


}
