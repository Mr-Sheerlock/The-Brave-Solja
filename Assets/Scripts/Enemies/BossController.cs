using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    #region LogicHandling
    public static int Bosslasercount = 1; // a lasercount for making the laser names unique
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform[] Shootinpoints = new Transform[12];
    [SerializeField] Weapon weapontype1;
    [SerializeField] Weapon weapontype2;
    public Weapon[] Weapons;
    [SerializeField] LayerMask LaserMask;
    [SerializeField]GameObject Mine;
    [SerializeField] GameObject DeathEffect;
    [SerializeField] AudioClip DeathSound;
    #endregion

    #region Movement&Direction
    //Movement & Direction
    Vector2 MovingDirection;
    public Transform[] BoundaryPoints; //UP DOWN RIGHT LEFT
    Vector2 randomDirections;
    public Vector2 Offset;
    public Vector2 TargetPosition;
    float LenghtOfBoundingSquare=20;
    public Vector2 OriginalPos;
    #endregion

    #region Shooting&Aiming
    static Transform Player;
    Vector2 PlayerPosition;
    Vector2 aimDirection;
    public GunController gunController;
    float RangeOfSight=60f;
    public bool SpottedPlayer;
    public float timer;
    float TimeActiveShooting=5f;
    float aimOffset=90f;
    float PeriodicTime=0.6f;
    #endregion

    #region Stats
    [SerializeField] int damage=5;
    public float speed=5;
    float TimeIdle =0;
    int numberofBullets=1;
    //Mines
    int Max_N_Mines=3;
    int Current_Mines = 0;
    [SerializeField] float Delay_Between_Mines=1;
    [SerializeField] float Delay_Between_Waves=5;
    [SerializeField] float MineDelay=2f;
    bool SpawnedMines=false;

    [SerializeField] bool DropsMines = false;   // false for bombs, true for mines
    //Memory time ??
    #endregion

    [SerializeField] float RotationIncrease = 1f;
    [SerializeField]float AmountOfRotation= 0.5f;
    enum State
    {
        IDLE = 0,
        MOVE = 1,
        SHOOT = 2
    }
    State CurrentState;


    void Awake()
    {
        Weapons = new Weapon[12];
        SetWeapons();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        gunController.SetGCDamage(damage);
        CurrentState = State.MOVE;
        SpottedPlayer = false;
        timer = 0f;
        //BoundaryPoints= new Transform[4];
        GetBoundsFromParent();
        SetUpBounds();
        TargetPosition = transform.position;
        Offset = new Vector2(0, 0);
        OriginalPos = transform.position;
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
            DontShootWeapons(); 
        }
        else
        {
            if(!SpawnedMines)
            {
                StartCoroutine(SpawnMines());
            }
            
            rb.velocity = new Vector2(0, 0);
            CurrentState = State.SHOOT;
            

            if (timer > TimeActiveShooting)
            {
                CurrentState = State.IDLE;
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
                AimTowardsPlayer();
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
                //gunController.ShootWeapon(Shootinpoints[i], new Vector2(0,1));
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
        aimDirection = PlayerPosition - (Vector2)transform.position;
        //working:
        //////////////////////////Quaternion.LookRotation(forward, upwards);
        Quaternion toRotation = Quaternion.LookRotation(transform.up, aimDirection); //from, to?
        toRotation.x = 0;
        toRotation.y = 0;
        if (((Vector2)transform.up - aimDirection.normalized).magnitude > 0.025)
        {
            toRotation.x = 0;
            toRotation.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, AmountOfRotation * Time.fixedDeltaTime);
        }
        

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
        int i ;
        for (i = 0; i < 11; i++)
        {
            Weapon loler = Instantiate(weapontype1, transform.position, transform.rotation,transform);
            Weapons[i] = loler;
            Weapons[i].SetDamage(damage);
            ((ProjectileGun)Weapons[i]).timer = Time.time;
            ((ProjectileGun)Weapons[i]).SetNumberOfProjectiles(numberofBullets);
            ((ProjectileGun)Weapons[i]).SetPeriodictime(PeriodicTime); 

        }
        
       
        Weapon lol = Instantiate(weapontype2, transform.position, transform.rotation);
        Weapons[i] = lol;
        Weapons[i].SetDamage(damage);
        ((LaserGun)Weapons[i]).SetLaserName("BossLaser" + Bosslasercount.ToString());
        ((LaserGun)Weapons[i]).SetMask(LaserMask);
        DontShootWeapons();
    }

    public void Die()
    {
        GameObject lol = Instantiate(DeathEffect,transform.position,transform.rotation);
        lol.transform.localScale = new Vector3(10, 10, 1);
        AudioSource.PlayClipAtPoint(DeathSound, transform.position);
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

    public void IncRotation()
    {
        AmountOfRotation += RotationIncrease;
    }

    IEnumerator SpawnMines()
    {
        SpawnedMines = true;
        for (int i=0; i < Max_N_Mines; i++)
        {
            GameObject lol = Instantiate(Mine, Player.position, Player.rotation);
            lol.GetComponent<Mine>().SetTimeIdle(MineDelay);
            lol.GetComponent<Mine>().SetIsMine(DropsMines);

            yield return new WaitForSeconds(Delay_Between_Mines);
        }
        yield return new WaitForSeconds(Delay_Between_Waves);
        SpawnedMines = false;
    }



    private void OnDestroy()
    {
        for (int i = 0; i < Weapons.Length; i++)
        {
            if(Weapons[i] != null)
            Destroy(Weapons[i].gameObject);
        }
    }

    public void ToggleDropMines()
    {
        DropsMines=!DropsMines;
    }
}
