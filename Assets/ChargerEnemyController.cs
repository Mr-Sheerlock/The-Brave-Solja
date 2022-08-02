using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerEnemyController : MonoBehaviour
{


    int damage;
    public float speed;
    public Rigidbody2D rb;

    //color
    public SpriteRenderer SR;
    Color C;

    //Movement & Direction
    Vector2 MovingDirection;
    public Transform[] BoundaryPoints; //UP DOWN RIGHT LEFT
    Vector2 randomDirections;
    Vector2 Offset;
    Vector2 TargetPosition;
    float LenghtOfBoundingSquare;
    Vector2 OriginalPos;

    //shooting and aiming 
    GameObject Player;
    Vector2 PlayerPosition;
    Vector2 aimDirection;
    public float RangeOfSight;
    bool SpottedPlayer;
    float timer;
    float MovingTimer;
    float TimeActiveMoving;
    float TimeActiveShooting;
    float TimeIdle;

    float aimOffset;


    //Memory time ??

    enum State
    {
        IDLE = 0,
        MOVE = 1,
        CHARGE=2,
        SHOOT = 3
    }
    State CurrentState;


    void Start()
    {
        C = SR.color;
        Player = GameObject.Find("Player");
        damage = 10;
        speed = 5;
        CurrentState = State.MOVE;
        SpottedPlayer = false;
        timer = 0f;
        MovingTimer = 0f;
        //BoundaryPoints= new Transform[4];
        TimeActiveShooting = 5f;
        TimeActiveMoving = 10f;
        TimeIdle = 5f;
        RangeOfSight = 10f;
        LenghtOfBoundingSquare = 20f;
        GetBoundsFromParent();
        SetUpBounds();
        TargetPosition = transform.position;
        Offset = new Vector2(0, 0);
        aimOffset = 130f;
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
        }
        else
        {
            Aim();
            CurrentState = State.SHOOT;
            Debug.Log($"Kelma is {timer}");

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
                
                if (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= 0.1f)
                {
                    if (checkboundary())
                    {
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

            case State.CHARGE:
                
                ChangeHuetoRed();
                break;

            case State.SHOOT:
                Shoot();
                break;

            case State.IDLE:
                ChangeHuetoOriginal();
                break;

            default:
                //do nothing
                break;
        }

    }

    void Aim()
    {
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        PlayerPosition = Player.transform.position;
        aimDirection = PlayerPosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        rb.rotation = aimAngle - aimOffset;
        rb.angularVelocity = 0;

    }
    void Move(Vector2 newPosition)
    {
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            if (collision.collider.GetComponent<Health>())
            {
                collision.collider.GetComponent<Health>().TakeDamage(damage);
            }
        }
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

    void Shoot()
    {
        

        
    }


    void ChangeHuetoRed()
    {
        if (C.g != 0)
        {
            C = new Color(C.r, C.g - 1, C.b - 1);
            SR.color = C;
        }
    }

    void ChangeHuetoOriginal()
    {
        while (C.g != 255)
        {
            C = new Color(C.r, C.g + 1, C.b +1);
            SR.color = C;
        }
    }

}
