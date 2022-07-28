using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{

    public float speed = 15f;
    public Rigidbody2D rb;
    float Horizontal, Vertical;
    Vector2 mousePosition;
    Vector2 aimDirection;
    public Transform Shootinpoint;
    //Vector2 Shootingpoint;
    public GunController gunController;
    
    
    float aimOffset;

    //Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        aimOffset = 130f;
        //offset = new Vector2(-1.49f, 1.64f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Aim();
        if (Input.GetMouseButton(0))
        { 

            //Shootingpoint = (Vector2)shootinpoint.position;

            gunController.ShootWeapon(Shootinpoint);
        }
        if (Input.GetMouseButtonUp(0))
        {
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
}
