using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D box;
    public Weapon weapon;
    public GunController gunController;

    protected Vector2 CollectibleSpawnOffset;
    protected float SearchRadius;
    Vector2 randomBools;

    protected Vector3 Position;
    
    protected void CheckforValidPosition ()
    {
        //1000 layer 3 

        // This would cast rays only against colliders in layer 3.
        while (true)
        {
            if (Physics2D.Raycast(Position, CollectibleSpawnOffset.normalized, SearchRadius, 8+(1<<6))) //bitmask 1001000 layer 3&6 "Walls","Collectibles"
            {

                CollectibleSpawnOffset = new Vector2(Random.Range(3f, 4f), Random.Range(4f, 5f));
                randomBools = new Vector2(Random.value > 0.5 ? 1 : -1, Random.value > 0.5 ? 1 : -1);
                CollectibleSpawnOffset *= randomBools;
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
    public void Spawn(Vector2 InitialSpawnPosition)
    {
        //bywsl le hena 3ady 

        Position = InitialSpawnPosition;
        CheckforValidPosition();

        transform.position = Position;
        spriteRenderer.enabled = true;
        box.enabled = true;
    }
}
