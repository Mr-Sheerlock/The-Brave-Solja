using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform Shootinpoint;
    [SerializeField] Weapon weapontype;
    [SerializeField] Weapon CurrentWeapon;

    //Movement & Direction
    Vector2 MovingDirection;
    [SerializeField] Transform[] BoundaryPoints; //UP DOWN RIGHT LEFT
    Vector2 randomDirections;
    Vector2 Offset= Vector2.zero;
    [SerializeField] Vector2 TargetPosition;
    public Vector2 OriginalPos;

    //shooting and aiming 
    GameObject Player;
    Vector2 PlayerPosition;
    Vector2 aimDirection;
    public GunController gunController;
    [SerializeField] bool SpottedPlayer=false;
    public float timer;

    float aimOffset=130f;
    

    public static int laserCount=0;

    //Memory time ??
    #region Stats
    [SerializeField] int damage = 5;
    [SerializeField] float speed = 5f;
    [SerializeField] float RangeOfSight=10f;
    int numberofBullets = 1;
    float PeriodicTime = 0.6f;
    [SerializeField] float LenghtOfBoundingSquare = 5f;
    float TimeActiveShooting = 5f;
    float TimeIdle=5f;

    #endregion


    enum State
    {
        IDLE = 0,
        MOVE = 1,
        SHOOT = 2
    }
    State CurrentState;


    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        SetWeapon();
        GetBoundsFromParent();
        SetUpBounds();
        TargetPosition = transform.position;
        OriginalPos=transform.position;
        CurrentState = State.MOVE;
        timer = 0f;
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

        BoundaryPoints[0].position= transform.position +new Vector3(0,LenghtOfBoundingSquare);
        BoundaryPoints[1].position= transform.position + new Vector3(0, -LenghtOfBoundingSquare);
        BoundaryPoints[2].position= transform.position + new Vector3(LenghtOfBoundingSquare,0);
        BoundaryPoints[3].position= transform.position + new Vector3(-LenghtOfBoundingSquare,0);
    }

    void SetWeapon()
    {
        CurrentWeapon = Instantiate(weapontype, transform);
        gunController.ChangeWeapon(CurrentWeapon, false);
        gunController.SetGCDamage(damage);
        if (CurrentWeapon as ProjectileGun)
        {

            ((ProjectileGun)CurrentWeapon).timer = Time.time;
            ((ProjectileGun)CurrentWeapon).SetNumberOfProjectiles(numberofBullets);
            ((ProjectileGun)CurrentWeapon).SetPeriodictime(PeriodicTime);
        }
        else
        if (CurrentWeapon as LaserGun)
        {
            ((LaserGun)CurrentWeapon).SetLaserName("Enemy Laser" + laserCount);
            laserCount++;
        }
    }
    
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
            AimTowardsPlayer();
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
                //randomize,validate
                gunController.DontShootWeapon(); 
                if (Vector2.Distance(TargetPosition ,(Vector2)transform.position )<=0.1f)
                {
                    if (checkboundary()) { 
                    while (!RandomizeValidPosition()) ;
                    }
                    else
                    {
                        //y7awl yrg3 el mohem 
                        Move(OriginalPos);
                    }
                }
                Move(TargetPosition);
                break;
            case State.SHOOT:
                gunController.ShootWeapon(Shootinpoint, aimDirection);
                break;

            case State.IDLE:
                gunController.DontShootWeapon();
                break;

            default:
                //do nothing
                break;
        }

    }

    void AimTowardsPlayer()
    {
        PlayerPosition = Player.transform.position;
        aimDirection = PlayerPosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        rb.rotation = aimAngle- aimOffset;
        rb.angularVelocity = 0;

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

        if (BoundaryPoints[0].position.y < TargetPosition.y) {TargetPosition-=Offset; return false; }
        if (BoundaryPoints[1].position.y > TargetPosition.y) {TargetPosition-=Offset; return false; }
        if (BoundaryPoints[2].position.x < TargetPosition.x) {TargetPosition-=Offset; return false; }
        if (BoundaryPoints[3].position.x > TargetPosition.x) {TargetPosition-=Offset; return false; }

        
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
        if (collision.collider.tag=="Walls")
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
    private void OnDestroy()
    {
        Destroy(CurrentWeapon);
    }
}
