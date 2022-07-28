using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected int damage;

    void SetDamage(int x)
    {
        damage = x;
    }

    public abstract void Shoot(Transform shootingPoint);
    public abstract void DontShoot();

    


}
