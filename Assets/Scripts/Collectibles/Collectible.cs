using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D box;
    public Weapon weapon;
    public GunController gunController;


    [SerializeField] protected GameObject light_pref;
    [SerializeField]protected GameObject _light;

    protected Vector2 CollectibleSpawnOffset;
    protected float SearchRadius;
    Vector2 randomDirections;

    protected Vector3 Position;
    protected Vector3 tempPosition;

    protected void CheckforValidPosition ()
    {
        //1000 layer 3 

        // This would cast rays only against colliders in layer 3.
        while (true)
        {

            RaycastHit2D Ray = Physics2D.Raycast(Position, CollectibleSpawnOffset.normalized, SearchRadius, 8 + (1 << 6));
            
            

            if (Ray) //bitmask 1001000 layer 3&6 "Walls","Collectibles"
            {

                RandomizeVector2D(ref CollectibleSpawnOffset);
            }
            else
            {
                //didn't hit
                //CollectibleSpawnPosition = transform.position + (Vector3)CollectibleSpawnOffset;
                
                Position += (Vector3)CollectibleSpawnOffset;
                break;

            }


        }
        return;
    }

    void RandomizeVector2D(ref Vector2 Vec)
    {
        Vec = new Vector2(Random.Range(3f, 4f), Random.Range(5f, 6f));
        randomDirections = new Vector2(Random.value > 0.5 ? 1 : -1, Random.value > 0.5 ? 1 : -1);
        Vec*= randomDirections;
    }
    public void Spawn(Vector2 InitialSpawnPosition)
    {
        //bywsl le hena 3ady 

        Position = InitialSpawnPosition;
        CheckforValidPosition();
        transform.position = Position;
        _light=Instantiate(light_pref,transform.position,transform.rotation);
        spriteRenderer.enabled = true;
        box.enabled = true;

    }

    
}
