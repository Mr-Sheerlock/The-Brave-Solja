using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerEnemyController : MonoBehaviour
{

    #region Logic Handling
    public Rigidbody2D rb;
    //color
    public SpriteRenderer SR;
    Color C;
    public Transform[] BoundaryPoints; //UP DOWN RIGHT LEFT
    [SerializeField] private TrailRenderer tr;
    #endregion

    #region Movement
    Vector2 MovingDirection;
    Vector2 randomDirections;
    Vector2 Offset;
    Vector2 TargetPosition;
    [SerializeField] float LenghtOfBoundingSquare=20f;
    Vector2 OriginalPos;

    #endregion

    #region Aiming & Charging
    [SerializeField]float damage=10f;
    [SerializeField] float speed=5;
    [SerializeField] float speedInc=10;
    //shooting and aiming 
    GameObject Player;
    Vector2 PlayerPosition;
    Vector2 aimDirection;
    [SerializeField] float RangeOfSight=10f;
    bool SpottedPlayer=false;
    [SerializeField] float timer;
    [SerializeField] float TimeStampIdle=10f;
    [SerializeField] float TimeStampCharging=4f;
    float aimOffset=130f;
    bool Shot=false;
    [SerializeField]float CheckhitDistance=2f;
    Collider2D EnemyCollider;

    float timeToReachTarget=0;

    #endregion

    //Memory time ??
    [SerializeField] GameObject DeathEffect;
    [SerializeField] AudioClip DeathSound;

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
        EnemyCollider = gameObject.GetComponent<Collider2D>();
        C = SR.color;
        Player = GameObject.FindGameObjectWithTag("Player");
        CurrentState = State.MOVE;
        timer = 0f;
        GetBoundsFromParent();
        SetUpBounds();
        TargetPosition = transform.position;
        Offset = new Vector2(0, 0);
        OriginalPos = transform.position;
        tr.emitting = false;
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
                //stops either by hitting the target or waiting for the supposed time to hit the position
                if (Vector2.Distance(TargetPosition, (Vector2)transform.position) <= CheckhitDistance)
                {
                    tr.emitting = false;
                    rb.velocity = Vector3.zero;
                    Debug.Log("Distance reached");
                }
                if(timer> TimeStampCharging+ timeToReachTarget)
                {
                    tr.emitting = false;
                    rb.velocity = Vector3.zero;
                    //Debug.Log("Time reached");
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
        GameObject lol = Instantiate(DeathEffect,transform.position, transform.rotation);
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
    void GetBoundsFromParent()
    {
        BoundaryPoints[0] = transform.parent.GetChild(1);
        BoundaryPoints[1] = transform.parent.GetChild(2);
        BoundaryPoints[2] = transform.parent.GetChild(3);
        BoundaryPoints[3] = transform.parent.GetChild(4);
    }
    void Shoot()
    {
        tr.emitting = true;
        ((CircleCollider2D)EnemyCollider).radius += 0.4f;

        //SpawnSprite
        speed += speedInc;
        TargetPosition= Player.transform.position;

        //calculate time to reach that position
        timeToReachTarget = Vector2.Distance(transform.position, TargetPosition)/speed;

        Move(TargetPosition);


        speed -= speedInc;
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

        aimDirection = TargetPosition - (Vector2)transform.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimAngle = aimAngle - aimOffset;
        transform.rotation = Quaternion.Euler(0, 0, aimAngle);

    }

    void AimTowardsPlayer()
    {
        PlayerPosition = Player.transform.position;
        aimDirection = PlayerPosition - (Vector2)transform.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimAngle = aimAngle - aimOffset;
        transform.rotation = Quaternion.Euler(0, 0, aimAngle);

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
        //????
        //Move(TargetPosition);
    }



}
