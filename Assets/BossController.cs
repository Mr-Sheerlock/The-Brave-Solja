using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    int numberOfWeapons;
    public static int Bosslasercount = 1; // a lasercount for making the laser names unique
    int damage;
    public float speed;
    public Rigidbody2D rb;
    public Transform[] Shootinpoints = new Transform[12];
    public Weapon weapontype1;
    public Weapon weapontype2;
    public Weapon[] Weapons;

    //Movement & Direction
    Vector2 MovingDirection;
    public Transform[] BoundaryPoints; //UP DOWN RIGHT LEFT
    Vector2 randomDirections;
    public Vector2 Offset;
    public Vector2 TargetPosition;
    float LenghtOfBoundingSquare;
    public Vector2 OriginalPos;

    //shooting and aiming 
    GameObject Player;
    Vector2 PlayerPosition;
    Vector2 aimDirection;
    public GunController gunController;
    public float RangeOfSight;
    public bool SpottedPlayer;
    public float timer;
    float TimeActiveShooting;
    float TimeIdle;

    float aimOffset;

    int numberofBullets;
    float PeriodicTime;

    //charger logic
    float radiusOffset;

    public float AmountOfRotation;
    //Memory time ??

    enum State
    {
        IDLE = 0,
        MOVE = 1,
        SHOOT = 2
    }
    State CurrentState;


    void Start()
    {
        AmountOfRotation = 25f;
        Weapons = new Weapon[12];
        SetWeapons();
        Player = GameObject.Find("Player");
        damage = 5;
        gunController.SetDamage(damage);
        speed = 5;
        CurrentState = State.MOVE;
        SpottedPlayer = false;
        timer = 0f;
        //BoundaryPoints= new Transform[4];
        TimeActiveShooting = 5f;
        TimeIdle = 3f;
        RangeOfSight = 60f; //The assumption is that the boss has a very high range of sight and wont be active until some point
        LenghtOfBoundingSquare = 20f; //keda keda doesn't move 
        GetBoundsFromParent();
        SetUpBounds();
        TargetPosition = transform.position;
        Offset = new Vector2(0, 0);
        aimOffset = 90f;
        OriginalPos = transform.position;
        radiusOffset = 0.2f;
        numberofBullets = 1;
        PeriodicTime = 0.6f;
    }

    void GetBoundsFromParent()
    {
        BoundaryPoints[0] = transform.parent.GetChild(1);
        BoundaryPoints[1] = transform.parent.GetChild(2);
        BoundaryPoints[2] = transform.parent.GetChild(3);
        BoundaryPoints[3] = transform.parent.GetChild(4);
    }
    void SetUpBounds()
    {

        BoundaryPoints[0].position = transform.position + new Vector3(0, LenghtOfBoundingSquare);
        BoundaryPoints[1].position = transform.position + new Vector3(0, -LenghtOfBoundingSquare);
        BoundaryPoints[2].position = transform.position + new Vector3(LenghtOfBoundingSquare, 0);
        BoundaryPoints[3].position = transform.position + new Vector3(-LenghtOfBoundingSquare, 0);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        SpotPlayer();
        //Adjust timer and determine state
        if (!SpottedPlayer)
        {
            timer = 0;
            //MovingTimer+=Time.fixedDeltaTime;
            CurrentState = State.MOVE;
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
            //AimTowardsPlayer();
            CurrentState = State.SHOOT;
            //Debug.Log($"Kelma is {timer}");

            if (timer > TimeActiveShooting)
            {
                CurrentState = State.IDLE;
                Debug.Log("LOLER");
            }
            if (timer > TimeActiveShooting + TimeIdle)
            {
                timer = 0;
            }
        }

        //Search for player

        switch (CurrentState)
        {
            case State.MOVE:
                ////randomize,validate
                //if (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= 0.1f)
                //{
                //    if (checkboundary())
                //    {
                //        while (!RandomizeValidPosition()) ;
                //    }
                //    else
                //    {
                //        //y7awl yrg3 el mohem 
                //        Move(OriginalPos);
                //    }
                //}
                //Move(TargetPosition);
                break;
            case State.SHOOT:
                rb.angularVelocity = 0;
                ShootWeapons();
                break;

            case State.IDLE:
                DontShootWeapons();
                break;

            default:
                //do nothing
                break;
        }

    }

    void ShootWeapons()
    {
        for (int i = 0; i <12; i++)
        {
            if (Shootinpoints[i])
            {
                gunController.ChangeWeapon(Weapons[i], false);
                gunController.ShootWeapon(Shootinpoints[i], Shootinpoints[i].up);
            }
        }
    }
    void DontShootWeapons()
    {
        for (int i = 0; i < 12; i++)
        {
            gunController.ChangeWeapon(Weapons[i], false);
            gunController.DontShootWeapon();
        }

    }
    void AimTowardsPlayer()
    {
        PlayerPosition = Player.transform.position;
        aimDirection = PlayerPosition - rb.position;
        aimDirection = aimDirection.normalized - new Vector2(Mathf.Cos(aimOffset), Mathf.Sin(aimOffset));
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        //aimAngle = aimAngle - aimOffset; //target angle
        if(Mathf.Abs(rb.rotation - aimAngle )>=10)
        transform.eulerAngles += new Vector3(0,0, aimAngle * AmountOfRotation*Time.fixedDeltaTime);
        //rb.angularVelocity = 0;

    }


    void AimTowardsPosition()
    {

        aimDirection = TargetPosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        rb.rotation = aimAngle - aimOffset;
        rb.angularVelocity = 0;

    }
    void Move(Vector2 newPosition)
    {
        AimTowardsPosition();
        MovingDirection = newPosition - (Vector2)transform.position;
        rb.velocity = MovingDirection.normalized * speed;

    }

    bool RandomizeValidPosition()
    {
        //UP DOWN RIGHT LEFT
        PlayerPosition = transform.position;
        RandomizeVector2D(ref Offset);
        TargetPosition = PlayerPosition + Offset;

        if (BoundaryPoints[0].position.y < TargetPosition.y) { TargetPosition -= Offset; return false; }
        if (BoundaryPoints[1].position.y > TargetPosition.y) { TargetPosition -= Offset; return false; }
        if (BoundaryPoints[2].position.x < TargetPosition.x) { TargetPosition -= Offset; return false; }
        if (BoundaryPoints[3].position.x > TargetPosition.x) { TargetPosition -= Offset; return false; }


        return true;

    }

    void SpotPlayer()
    {
        SpottedPlayer = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, RangeOfSight, 1 << 8);

        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Player")
            {
                SpottedPlayer = true;
                Debug.Log("Spotted Player");
            }
        }
    }

    void RandomizeVector2D(ref Vector2 Vec)
    {
        Vec = new Vector2(Random.Range(3f, LenghtOfBoundingSquare), Random.Range(3f, LenghtOfBoundingSquare));
        randomDirections = new Vector2(Random.value > 0.5 ? 1 : -1, Random.value > 0.5 ? 1 : -1);
        Vec *= randomDirections;
    }
    void SetWeapons()
    {
        int i = 0;
        for (i = 0; i < 11; i++)
        {
            //Debug.Log("lolerrere");
            Weapon loler = Instantiate(weapontype1, transform.position, transform.rotation);
            Weapons[i] = loler;

            ((ProjectileGun)Weapons[i]).timer = Time.time;
            ((ProjectileGun)Weapons[i]).SetNumberOfProjectiles(numberofBullets);
            ((ProjectileGun)Weapons[i]).SetPeriodictime(PeriodicTime);

        }

        Weapon lol = Instantiate(weapontype2, transform.position, transform.rotation);
        Weapons[i] = lol;
        ((LaserGun)Weapons[i]).SetLaserName("BossLaser" + Bosslasercount.ToString());

    }

    public void Die()
    {
        Destroy(gameObject);
    }

    bool checkboundary()
    {
        if (BoundaryPoints[0].position.y <= transform.position.y) { TargetPosition -= Offset; return false; }
        if (BoundaryPoints[1].position.y >= transform.position.y) { TargetPosition -= Offset; return false; }
        if (BoundaryPoints[2].position.x <= transform.position.x) { TargetPosition -= Offset; return false; }
        if (BoundaryPoints[3].position.x >= transform.position.x) { TargetPosition -= Offset; return false; }
        return true;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Walls")
        {
            if (checkboundary())
            {
                while (!RandomizeValidPosition()) ;
            }
            else
            {
                //y7awl yrg3 el mohem 
                Move(OriginalPos);
                return;
            }
        }
        Move(TargetPosition);
    }
}
