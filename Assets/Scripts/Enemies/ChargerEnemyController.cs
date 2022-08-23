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
    public Vector2 TargetPosition;
    float LenghtOfBoundingSquare;
    Vector2 OriginalPos;

    //shooting and aiming 
    GameObject Player;
    Vector2 PlayerPosition;
    Vector2 aimDirection;
    public float RangeOfSight;
    bool SpottedPlayer;
    public  float timer;
    float TimeStampIdle;
    float TimeStampCharging;

    float aimOffset;

    bool Shot;
    float CheckhitDistance;

    Collider2D EnemyCollider;

    float timeToReachTarget;

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
        CheckhitDistance = 2f;
        EnemyCollider = gameObject.GetComponent<Collider2D>();
        C = SR.color;
        Player = GameObject.FindGameObjectWithTag("Player");
        damage = 10;
        speed = 5;
        CurrentState = State.MOVE;
        SpottedPlayer = false;
        timer = 0f;
        RangeOfSight = 10f;
        LenghtOfBoundingSquare = 20f;
        GetBoundsFromParent();
        SetUpBounds();
        TargetPosition = transform.position;
        Offset = new Vector2(0, 0);
        aimOffset = 130f;
        OriginalPos = transform.position;
        Shot = false;

        ///TimeStamps/////
        TimeStampCharging = 4f;
        TimeStampIdle = 10f;
        timeToReachTarget = 0;
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
            AimTowardsPlayer();
            //CurrentState = State.CHARGE;

            if (timer < TimeStampCharging)
            {
                CurrentState = State.CHARGE;
                Debug.Log("I am charging");
                
            }
            else
            {
                if (!Shot)
                {
                    CurrentState = State.SHOOT;
                }
                else
                {
                    CurrentState = State.IDLE;
                }
            }
            if (timer > timeToReachTarget + TimeStampIdle)
            {
                timer = 0;
                Shot = false;
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
                rb.velocity = Vector2.zero;
                ChangeHuetoRed(timer);
                break;

            case State.SHOOT:
                Shot = true;
                Shoot();
                break;

            case State.IDLE:
                if(Vector2.Distance(TargetPosition, (Vector2)transform.position) <= CheckhitDistance)
                {
                    rb.velocity = Vector3.zero;
                }
                if(timer> TimeStampCharging+ timeToReachTarget)
                {
                    rb.velocity = Vector3.zero;
                }
                ChangeHuetoOriginal(timer);
                break;

            default:
                //do nothing
                break;
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
    void GetBoundsFromParent()
    {
        BoundaryPoints[0] = transform.parent.GetChild(1);
        BoundaryPoints[1] = transform.parent.GetChild(2);
        BoundaryPoints[2] = transform.parent.GetChild(3);
        BoundaryPoints[3] = transform.parent.GetChild(4);
    }
    void Shoot()
    {
        ((CircleCollider2D)EnemyCollider).radius += 0.4f;

        //SpawnSprite
        speed += 10f;
        TargetPosition= Player.transform.position;

        //calculate time to reach that position
        timeToReachTarget = Vector2.Distance(transform.position, TargetPosition)/speed;

        Move(TargetPosition);


        speed -= 10f;
        ((CircleCollider2D)EnemyCollider).radius -= 0.4f;
    }


    void ChangeHuetoRed(float timer)
    {

        //Debug.Log($"shwyt text eshta {esm_el_variable}");
        float current = 1 - timer/ TimeStampCharging;
        C = new Color(C.r, current, current);
        SR.color = C;
    }

    void ChangeHuetoOriginal(float timer)
    {
        float current = (timer-4 )/TimeStampCharging;
        C = new Color(C.r, current, current);
        SR.color = C;
        
    }

    void AimTowardsPosition()
    {

        aimDirection = TargetPosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        rb.rotation = aimAngle - aimOffset;
        rb.angularVelocity = 0;

    }

    void AimTowardsPlayer()
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            if (collision.collider.GetComponent<Health>())
            {
                collision.collider.GetComponent<Health>().TakeDamage(damage);
            }
            rb.velocity = Vector2.zero;
        }
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
