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

    [SerializeField] bool fiftypercent = false;  

    [Header("Events")]
    public UnityEvent OnDeath;
    public UnityEvent On50percent;
    public UnityEvent OnTakingDamage;

    public HealthbarScript HB;
    

    public void IncMaxHealhth(int Addend)
    {
        Maxhealth += Addend;
    }

    public void IncCurrentHealth (float Addend)
    {
        if(CurrentHealth < Maxhealth)
        {
            CurrentHealth += Addend;
        }

    }

    void Start()
    {

        HB = gameObject.GetComponentInChildren<HealthbarScript>();

        
        //if(gameObject.CompareTag("Enemies"))
        //{
            
        //}else if (gameObject.CompareTag("Towers"))
        //{

        //}else if ( gameObject.CompareTag("Player"))
        //{
            
        //}
    }


    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >=1 )
        {
            if(CurrentHealth< Maxhealth)
            {
                CurrentHealth += healregen;
            }

            timer=0; 
            if(HB)
            (HB)?.SetSize((float)CurrentHealth / Maxhealth);
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        //Debug.Log("Damage Taken is" + damage);
        OnTakingDamage.Invoke();
        (HB)?.SetSize((float)CurrentHealth / Maxhealth);
        if (CurrentHealth / Maxhealth <= 0.5f && !fiftypercent)
        {
            fiftypercent = true;
            On50percent.Invoke();
        }

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDeath.Invoke();
        }
    }


}
