using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D box;
    public Weapon weapon;
    public GunController gunController;

    public abstract void Spawn(Vector2 newposition);




}
