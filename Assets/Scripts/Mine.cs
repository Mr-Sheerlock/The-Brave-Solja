using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    float MineDamage=20f;
    float MineRadius=2.6f;
    [SerializeField] CircleCollider2D Collider;
    [SerializeField] MineLight light;
    [SerializeField] SpriteRenderer MineSquareRenderer;
    [SerializeField] GameObject MineEffect;
    [SerializeField] AudioClip MineSound;

    [SerializeField] float TimeIdle = 0f;
    bool isMine=true;


    private void Awake()
    {
         ;
    }
    void Start()
    {
        if (!isMine)
        {
            MineSquareRenderer.enabled = false;
        }
        Collider.radius = MineRadius;
        light?.SetRadius(MineRadius);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMine)
        {
            StartCoroutine(ActivateCollider());
        }
        else{
            StartCoroutine(WaitThenExplode());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Explode();
    }

    IEnumerator ActivateCollider()
    {
        yield return new WaitForSeconds(TimeIdle);
        Collider.enabled = true;
    }

    void Explode()
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
        AudioSource.PlayClipAtPoint(MineSound, transform.position);
        //destroy
        Destroy(gameObject);
    }
    IEnumerator WaitThenExplode()
    {
        yield return new WaitForSeconds(TimeIdle);
        Explode();
    }

    public void SetTimeIdle(float newTime)
    {
        TimeIdle = newTime;
        Debug.Log("Time Idle inside function is " + TimeIdle);
    }

    public void SetDamage(float damage)
    {
        MineDamage = damage;
    }
    public void SetIsMine(bool value)
    {
        isMine = value;
    }

}
