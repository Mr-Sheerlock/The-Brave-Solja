using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    int damage;
    public float speed;
    public Rigidbody2D rb;
    public Transform Shootinpoint;
    public Weapon weapontype;
    public Weapon CurrentWeapon;

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


    //charger logic
    float radiusOffset;

    
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
        Player = GameObject.Find("Player");
        damage = 5;
        gunController.SetDamage(damage);
        speed = 5;
        CurrentState = State.MOVE;
        SpottedPlayer = false;
        timer = 0f;
        SetWeapon();
        //BoundaryPoints= new Transform[4];
        TimeActiveShooting = 5f;
        TimeIdle = 5f;
        RangeOfSight = 10f;
        LenghtOfBoundingSquare = 5f;
        GetBoundsFromParent();
        SetUpBounds();
        TargetPosition = transform.position;
        Offset = new Vector2(0, 0);
        aimOffset = 130f;
        OriginalPos=transform.position;
        radiusOffset = 0.2f;
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
    void SetWeapon()
    {
        CurrentWeapon = Instantiate(weapontype);
        gunController.ChangeWeapon(CurrentWeapon, false);
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
}
