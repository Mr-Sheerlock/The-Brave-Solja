using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    ///Specs///
    public GameObject DieEffect;
    public GameObject PlayerLight;
    int damage;
    public float speed;
    public Rigidbody2D rb;
    float Horizontal, Vertical;
    public Transform Shootinpoint;
    
    //shooting and aiming 
    Vector2 mousePosition;
    Vector2 aimDirection;
    public GunController gunController;
    
    
    float aimOffset;

    //Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        damage = 10;
        gunController.SetGCDamage(damage);
        speed = 15;

        aimOffset = 130f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Aim();
        if (Input.GetMouseButton(0))
        { 

            //Shootingpoint = (Vector2)shootinpoint.position;

            gunController.ShootWeapon(Shootinpoint, aimDirection);
        }
        else
        {
            gunController.DontShootWeapon();

        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("lolNoShoot");
            gunController.DontShootWeapon();
        }
        
        


    }

    void Aim()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - aimOffset;
        rb.rotation = aimAngle;

    }
    void Move()
    {
        Horizontal = Input.GetAxisRaw("Horizontal") * speed;
        Vertical = Input.GetAxisRaw("Vertical") * speed;
        rb.velocity = new Vector2(Horizontal, Vertical);

    }

    public void Die()
    {
        Instantiate(DieEffect, transform.position, transform.rotation);
        Instantiate(PlayerLight, transform.position,transform.rotation);
        Destroy(gameObject);
    }
}
