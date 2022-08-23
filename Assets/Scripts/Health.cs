using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Health : MonoBehaviour
{
    float timer = 0;
    [SerializeField] int Maxhealth=100;
    [SerializeField]  float CurrentHealth=100;
    [SerializeField] float healregen=0;
    
    [Header("Events")]
    public UnityEvent OnDeath;
    public UnityEvent On50percent;

    public HealthbarScript HB;
    //GameObject Healthbar;
    
    void Start()
    {

        HB = gameObject.GetComponentInChildren<HealthbarScript>();        
    }


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
        if (CurrentHealth / Maxhealth <= 0.5f)
        {
            On50percent.Invoke();
            //gets invoked once.
            On50percent.RemoveAllListeners();
        }
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDeath.Invoke();
        }
    }


}
