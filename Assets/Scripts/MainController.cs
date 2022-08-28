using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    
    #region Specs
    [SerializeField]float damage=10;
    int numberofProjectiles=1;
    [SerializeField]float speed=15;
    #endregion


    #region Logic Handling
    public GameObject DieEffect;
    public GameObject PlayerLight;
    public Rigidbody2D rb;
    float Horizontal, Vertical;
    public Transform Shootinpoint;
    [SerializeField] AudioClip DieSound;
    #endregion

    #region Shooting&Aiming
    Vector2 mousePosition;
    Vector2 aimDirection;
    public GunController gunController;
    float aimOffset=130f;
    #endregion


    //shooting and aiming 
    #region Dashing
    [SerializeField] private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;
    #endregion


    void Start()
    {
        gunController.SetGCDamage((int)damage);
        tr.emitting = false;
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
            gunController.DontShootWeapon();
        }

        if (Input.GetKey(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
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
        AudioSource.PlayClipAtPoint(DieSound, transform.position);
        Destroy(gameObject);
    }

    public void  IncDamage(float addend)
    {
        damage += addend;
        gunController.SetGCDamage((int)damage);
    }
    
    public void IncNProjectiles(int addend)
    {
        numberofProjectiles+= addend;
        if (numberofProjectiles > 15)
        {
            numberofProjectiles = 15;  //Max number 
        }

        gunController.SetNprojectiles(numberofProjectiles);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        speed *= 2;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        speed /= 2;
        tr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
