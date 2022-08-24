using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] float MineDamage=20f;
    float MineRadius=2.6f;
    [SerializeField] CircleCollider2D Collider;
    [SerializeField] MineLight light;
    [SerializeField] GameObject MineEffect;

    [SerializeField] float TimeIdle = 0f;



    private void Awake()
    {
         ;
    }
    void Start()
    {
        
        Collider.radius = MineRadius;   
        light?.SetRadius(MineRadius);
        Debug.Log("Time Idle is " + TimeIdle);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StartCoroutine(ActivateCollider());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //damage
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, MineRadius, 1 << 8);

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Health>())
            {
                collider.GetComponent<Health>().TakeDamage(MineDamage);
            }
        }

        //spawn effect
        Instantiate(MineEffect, transform.position, transform.rotation);
        //destroy
        Destroy(gameObject);
    }

    IEnumerator ActivateCollider()
    {
        yield return new WaitForSeconds(TimeIdle);
        Collider.enabled = true;
    }

    public void SetTimeIdle(float newTime)
    {
        TimeIdle = newTime;
        Debug.Log("Time Idle inside function is " + TimeIdle);
    }
}
