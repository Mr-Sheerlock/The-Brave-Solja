using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected int damage=5;

    public virtual void SetDamage(int x)
    {
        damage = x;
    }

    public abstract void Shoot(Transform ShootingPoint,Vector2 AimDirection);
    public abstract void DontShoot();

    


}
